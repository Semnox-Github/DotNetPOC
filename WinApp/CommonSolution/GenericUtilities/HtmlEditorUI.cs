/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - HtmlEditorUI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                     Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 * 2.80         25-Jun-2020            Deeksha        Modified to Make Product module 
 *                                                    read only in Windows Management Studio.
 *2.100.0       23-Aug-2020            Deeksha        Modified for Recipe MAnagement Enhancement
 ********************************************************************************************/
#region Using directives

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

#endregion

namespace Semnox.Core.GenericUtilities
{

    /// <summary>
    /// HtmlEditorForm Form
    /// </summary>
    public partial class HtmlEditorUI : Form
    {

        private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ManagementStudioSwitch managementStudioSwitch;

        private string translationObject;
        private int translationObjectKeyId;
        private string translationElement;
        private string translationObjectGuid;
        private string applicability = string.Empty;

        // working directory variable
        private string workingDirectory = string.Empty;
        private Semnox.Core.Utilities.Utilities utilities;
        private const string INVENTORY_PRODUCTS = "PRODUCT";

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public HtmlEditorUI(Semnox.Core.Utilities.Utilities _Utilities, string Object, int objectKeyId, string Element, string ElementGuid, bool showTranslation , string applicability = "")
        {
            log.LogMethodEntry(_Utilities, Object, objectKeyId, Element, ElementGuid, showTranslation);
            InitializeComponent();
            utilities = _Utilities;
            this.applicability = applicability;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);

            translationObject = Object;
            translationObjectKeyId = objectKeyId;
            translationElement = Element;
            translationObjectGuid = ElementGuid;

            if (showTranslation)
                BindLanguage("Default");
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private List<LanguagesDTO> GetLanguages()
        {
            log.LogMethodEntry();
            List<LanguagesDTO> languageList = new List<LanguagesDTO>();
            log.LogMethodEntry();
            try
            {

                List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                languageList = new Semnox.Parafait.Languages.Languages(machineUserContext).GetAllLanguagesList(searchParameters);
                if(languageList == null)
                {
                    languageList = new List<LanguagesDTO>();
                }
                languageList.Insert(0, new LanguagesDTO()
                {
                    LanguageId = -1,
                    LanguageName = "Default"
                });
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-GetLanguages method with exception." + ex.Message);
            }

            log.LogMethodExit(languageList);
            return languageList;

        }

        private void BindLanguage(string choosedLanguage = "")
        {
            log.LogMethodEntry(choosedLanguage);
            try
            {
                cmbLanguages.DataSource = GetLanguages();
                cmbLanguages.DisplayMember = "LanguageName";
                cmbLanguages.ValueMember = "LanguageId";
                if (!string.IsNullOrEmpty(choosedLanguage))
                {
                    cmbLanguages.SelectedText = choosedLanguage;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error-BindLanguage(string choosedLanguage) method.", ex);
            }

            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.Debug("btnClose_Click clicked.");
            this.Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SaveHTMLData(false);
            log.LogMethodExit();
        }

        private void cmbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                LanguagesDTO languagesDTO = (LanguagesDTO)cmbLanguages.SelectedItem;
                string elementHtmlData = "";
                if (languagesDTO.LanguageId == -1)
                {
                    elementHtmlData = GetObjectElementDefaultData();
                }
                else
                {
                    ObjectTranslationsDTO objectTranslationsDTO = GetObjectTransalationDTO(languagesDTO.LanguageId, translationObjectGuid);
                    if (objectTranslationsDTO != null)
                        elementHtmlData = objectTranslationsDTO.Translation;
                }

                SetHtmlString(elementHtmlData);

            }
            catch (Exception ex)
            {
                log.Log("Error-cmbLanguages_SelectedIndexChanged(object sender, EventArgs e) method.", ex);
            }
            log.LogMethodExit();
        }


        private ObjectTranslationsDTO GetObjectTransalationDTO(int languageId, string guid)
        {
            log.LogMethodEntry(languageId, guid);
            ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(machineUserContext);
            List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> objSearchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
            objSearchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID, languageId.ToString()));
            objSearchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT, translationObject));
            objSearchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT, translationElement));
            objSearchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, guid));
            List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTranslationsList.GetAllObjectTranslations(objSearchParameters);
            if (objectTranslationsDTOList != null && objectTranslationsDTOList.Count > 0)
            {
                log.LogMethodExit(objectTranslationsDTOList[0]);
                return objectTranslationsDTOList[0];
            }
            else
            {
                log.LogMethodExit();
                return null;
            }

        }


        private string GetObjectElementDefaultData()
        {
            log.LogMethodEntry();
            string objectElementData = "";

            try
            {

                if (translationObjectKeyId == -1 || translationObject == "" || translationElement == "" || translationObjectGuid == "")
                {
                    log.Debug("Ends-GetObjectElementDefaultData() method - element Properties not set ");
                    MessageBox.Show(utilities.MessageUtils.getMessage(1356));
                    this.btnSave.Enabled = false;
                }
                else
                {
                    this.btnSave.Enabled = true;
                    string elementQuery;


                    if (translationObject == "PRODUCTS")
                    {

                        elementQuery = @"SELECT isnull(WebDescription, '' ) WebDescription 
                                        FROM products WHERE product_id = @productId ";

                        object objProd = utilities.executeScalar(elementQuery, new SqlParameter("@productId", translationObjectKeyId));

                        objectElementData = objProd != null ? objProd.ToString() : "";
                    }
                    if (translationObject == "PRODUCT")
                    {

                        elementQuery = @"SELECT isnull(RecipeDescription, '' ) RecipeDescription 
                                        FROM product WHERE productId = @productId ";

                        object objProd = utilities.executeScalar(elementQuery, new SqlParameter("@productId", translationObjectKeyId));

                        objectElementData = objProd != null ? objProd.ToString() : "";
                    }
                }

                log.LogMethodExit(objectElementData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("ERROR"));
                log.Log("Error-GetObjectElementDefaultData() method with exception.", ex);
                this.btnSave.Enabled = false;
            }

            return objectElementData;
        }

        private void SaveHTMLData(bool closeForm)
        {
            try
            {
                log.LogMethodEntry(closeForm);

                if (translationObjectKeyId == -1 || translationObject == "" || translationElement == "" || translationObjectGuid == "")
                {
                    log.LogMethodExit();
                    MessageBox.Show(utilities.MessageUtils.getMessage(1356));
                    return;
                }

                LanguagesDTO languagesDTO = (LanguagesDTO)cmbLanguages.SelectedItem;
                if (languagesDTO.LanguageId == -1)
                    SaveObjectElementData();
                else
                    SaveObjectTransalation(languagesDTO.LanguageId);

                if (closeForm)
                    this.Close();
            }
            catch (Exception ex)
            {
                log.Log("Error-SaveHTMLData() method with exception.", ex);
                utilities.MessageUtils.getMessage("ERROR");
            }
            log.LogMethodExit();
        }

        private void SaveObjectElementData()
        {

            log.LogMethodEntry();
            try
            {
                string elementUpdateQuery;
                if (translationObject == "PRODUCTS")
                {
                    elementUpdateQuery = @"UPDATE products set WebDescription = @WebDescription ,
                                                           last_updated_user = @user,
                                                           last_updated_date = getdate()
                                        WHERE product_id = @productId ";

                    utilities.executeNonQuery(elementUpdateQuery, new SqlParameter("@productId", translationObjectKeyId),
                                                                  new SqlParameter("@WebDescription", GetHtmlString()),
                                                                  new SqlParameter("@user", machineUserContext.GetUserId()));

                    MessageBox.Show(utilities.MessageUtils.getMessage(122));
                }
                if (translationObject == "PRODUCT")
                {
                    elementUpdateQuery = @"UPDATE product set RecipeDescription = @RecipeDescription ,
                                                           LastModUserId = @user,
                                                           LastModDttm = getdate()
                                        WHERE productId = @productId ";

                    utilities.executeNonQuery(elementUpdateQuery, new SqlParameter("@productId", translationObjectKeyId),
                                                                  new SqlParameter("@RecipeDescription", GetHtmlString()),
                                                                  new SqlParameter("@user", machineUserContext.GetUserId()));

                    MessageBox.Show(utilities.MessageUtils.getMessage(122));
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Log("Error-SaveObjectElementData() method with exception.", ex);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw;
            }

        }

        private void SaveObjectTransalation(int languageId)
        {
            log.LogMethodEntry(languageId);
            try
            {
                ObjectTranslationsDTO objectTranslationsDTO = GetObjectTransalationDTO(languageId, translationObjectGuid);
                if (objectTranslationsDTO == null)
                {
                    objectTranslationsDTO = new ObjectTranslationsDTO()
                    {
                        LanguageId = languageId,
                        ElementGuid = translationObjectGuid,
                        TableObject = translationObject,
                        Element = translationElement
                    };
                }
                objectTranslationsDTO.Translation = GetHtmlString();
                ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
                ObjectTranslations objectTranslations = new ObjectTranslations(machineUserContext, objectTranslationsDTO);
                objectTranslations.Save();
                MessageBox.Show(utilities.MessageUtils.getMessage(122));
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Log("Error-SaveObjectTransalation() method with exception.", ex);
                log.LogMethodExit("Throwing Exception" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// GetHtmlString getter
        /// </summary>
        /// <returns> returns string</returns>
        public string GetHtmlString()
        {
            log.LogMethodEntry();
            string returnValue = this.htmlEditorControl.InnerHtml.ToString();// DocumentHtml;
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// SetHtmlString method
        /// </summary>
        /// <param name="htmlString">htmlString</param>
        public void SetHtmlString(string htmlString)
        {
            log.LogMethodEntry(htmlString);
            this.htmlEditorControl.BodyHtml = htmlString;
            log.LogMethodExit();
        }

        #region Event Processing
        private void bToolbar_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.ToolbarVisible = !this.htmlEditorControl.ToolbarVisible;
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bBackground_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            using (ColorDialog dialog = new ColorDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Color color = dialog.Color;
                    this.htmlEditorControl.BodyBackColor = color;
                }
            }
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bForeground_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            using (ColorDialog dialog = new ColorDialog())
            {

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Color color = dialog.Color;
                    this.htmlEditorControl.BodyForeColor = color;
                }
            }
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bEditHTML_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.HtmlContentsEdit();
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bViewHtml_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.HtmlContentsView();
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bStyle_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            string cssFile = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Resources\default.css");
            if (File.Exists(cssFile))
            {
                this.htmlEditorControl.StylesheetUrl = cssFile;
                MessageBox.Show(this, cssFile, "Style Sheet Linked", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, cssFile, "Style Sheet Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bScript_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            string scriptFile = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Resources\default.js");
            if (File.Exists(scriptFile))
            {
                this.htmlEditorControl.ScriptSource = scriptFile;
                MessageBox.Show(this, scriptFile, "Script Source Linked", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, scriptFile, "Script Source Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        // obtains the text resource from an embedded file
        private string GetResourceText(string filename)
        {
            log.LogMethodEntry(filename);
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resource = string.Empty;

            // resources are named using a fully qualified name
            string streamName = this.GetType().Namespace + @"." + filename;
            using (Stream stream = assembly.GetManifestResourceStream(streamName))
            {
                // read the contents of the embedded file
                using (StreamReader reader = new StreamReader(stream))
                {
                    resource = reader.ReadToEnd(); ;
                }
            }
            log.LogMethodExit(resource);
            return resource;
        }

        private void readonlyCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.ReadOnly = this.readonlyCheck.Checked;
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bOverWrite_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.ToggleOverWrite();
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bSaveHtml_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.SaveFilePrompt();
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bOpenHtml_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.OpenFilePrompt();
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bHeading_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            int headingRef = this.listHeadings.SelectedIndex + 1;
            if (headingRef > 0)
            {
                HtmlHeadingType headingType = (HtmlHeadingType)headingRef;
                this.htmlEditorControl.InsertHeading(headingType);
            }
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bFormatted_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.InsertFormattedBlock();
            log.LogMethodExit();
        }

        private void bNormal_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.InsertNormalBlock();
            log.LogMethodExit();
        }

        private void bInsertHtml_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.InsertHtmlPrompt();
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bImage_Click(object sender, System.EventArgs e)
        {
            // set initial value states
            log.LogMethodEntry();
            string fileName = string.Empty;
            string filePath = string.Empty;

            // define the file dialog
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select Image";
                dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                dialog.CheckFileExists = true;
                if (workingDirectory != String.Empty) dialog.InitialDirectory = workingDirectory;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = Path.GetFileName(dialog.FileName);
                    filePath = Path.GetFullPath(dialog.FileName);
                    workingDirectory = Path.GetDirectoryName(dialog.FileName);

                    if (fileName != "")
                    {
                        // have a path for a image I can insert
                        this.htmlEditorControl.InsertImage(filePath);
                    }
                }
            }
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bBasrHref_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.AutoWordWrap = !this.htmlEditorControl.AutoWordWrap;
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        private void bPaste_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.htmlEditorControl.InsertTextPrompt();
            this.htmlEditorControl.Focus();
            log.LogMethodExit();
        }

        // set the flat style of the dialog based on the user setting
        private void SetFlatStyleSystem(Control parent)
        {
            // iterate through all controls setting the flat style
            log.LogMethodEntry(parent);
            foreach (Control control in parent.Controls)
            {
                // Only these controls have a FlatStyle property
                ButtonBase button = control as ButtonBase;
                GroupBox group = control as GroupBox;
                Label label = control as Label;
                TextBox textBox = control as TextBox;
                if (button != null) button.FlatStyle = FlatStyle.System;
                else if (group != null) group.FlatStyle = FlatStyle.System;
                else if (label != null) label.FlatStyle = FlatStyle.System;

                // Set contained controls FlatStyle, too
                SetFlatStyleSystem(control);
            }
            log.LogMethodExit();
        }

        private void EditorTestForm_Load(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            SetFlatStyleSystem(this);
            log.LogMethodExit();
        }

        private void bMicrosoft_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void bUrl_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            string href = Microsoft.VisualBasic.Interaction.InputBox("Enter Href for Navigation:", "Href", string.Empty, -1, -1);
            if (href != string.Empty) this.htmlEditorControl.LoadFromUrl(href);
            log.LogMethodExit();
        }

        private void htmlEditorControl_HtmlException(object sender, Semnox.Core.GenericUtilities.HtmlExceptionEventArgs args)
        {
            // obtain the message and operation
            // concatenate the message with any inner message
            log.LogMethodEntry();
            string operation = args.Operation;
            Exception ex = args.ExceptionObject;
            string message = ex.Message;
            if (ex.InnerException != null)
            {
                if (ex.InnerException.Message != null)
                {
                    message = string.Format("{0}\n{1}", message, ex.InnerException.Message);
                }
            }
            // define the title for the internal message box
            string title;
            if (operation == null || operation == string.Empty)
            {
                title = "Unknown Error";
            }
            else
            {
                title = operation + " Error";
            }
            // display the error message box
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            log.LogMethodExit();
        }

        private void bLoadFile_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "html";
                dialog.Title = "Open FIle";
                dialog.AddExtension = true;
                dialog.Filter = "Html files (*.html,*.htm)|*.html;*htm|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (workingDirectory != String.Empty) dialog.InitialDirectory = workingDirectory;
                // show the dialog and see if the users enters OK
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.htmlEditorControl.LoadFromFile(dialog.FileName);
                }
            }
            log.LogMethodExit();
        }

        private void htmlEditorControl_HtmlNavigation(object sender, Semnox.Core.GenericUtilities.HtmlNavigationEventArgs e)
        {
            log.LogMethodEntry();
            e.Cancel = false;
            log.LogMethodExit();
        }

        #endregion
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule || applicability == INVENTORY_PRODUCTS)
            {
                btnSave.Enabled = true;
                bEditHTML.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                bEditHTML.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
