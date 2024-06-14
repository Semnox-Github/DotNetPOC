/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - ObjectTranslation UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    ///  Object translations User interface 
    /// </summary>
    public partial class ObjectTranslationsUI : Form
    {
        private Semnox.Core.Utilities.Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ObjectTranslationsDTO objectTranslationsDTO;
        private string Element;
        private string Object;
        private string ElementGuid;
        /// <summary>
        /// Constructor used during object creation.
        /// </summary>
        public ObjectTranslationsUI(Semnox.Core.Utilities.Utilities _Utilities,string EnglishText,string Element,string Object,string ElementGuid )
        {
            log.LogMethodEntry(_Utilities, EnglishText, Element, Object, ElementGuid);
            InitializeComponent();
            utilities = _Utilities;

            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            this.Element = Element;
            this.Object = Object;
            txtEnglish.Text = EnglishText;
            this.ElementGuid = ElementGuid;
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            LoadLanguages();
            log.LogMethodExit();
        }
        private void LoadLanguages()
        {
            log.LogMethodEntry();
            Languages languages = new Languages(machineUserContext);
            List<LanguagesDTO> languagesDTOList=new List<LanguagesDTO>();
            languagesDTOList=languages.GetAllLanguagesList(null);
            if (languagesDTOList==null)
            {
                languagesDTOList = new List<LanguagesDTO>();
            }
            languagesDTOList.Insert(0, new LanguagesDTO());
            languagesDTOList[0].LanguageName = "<SELECT>";
            cmbLanguage.DataSource = languagesDTOList;
            cmbLanguage.DisplayMember="LanguageName";
            cmbLanguage.ValueMember="LanguageId";
            cmbLanguage.SelectedValue = utilities.ParafaitEnv.LanguageId;
            if (cmbLanguage.SelectedValue == null)
                cmbLanguage.SelectedValue = -1;
            log.LogMethodExit();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(txtTranslation.Text))
            {
                log.LogMethodExit();
                return;
            }
            if (!string.IsNullOrEmpty(txtTranslation.Text) && (cmbLanguage.SelectedValue == null) && (cmbLanguage.SelectedValue != null && Convert.ToInt32(cmbLanguage.SelectedValue)==-1))
            {
                MessageBox.Show("Please select the language.");
                log.LogMethodExit();
                return;
            }
            if (objectTranslationsDTO == null)
            {
                objectTranslationsDTO = new ObjectTranslationsDTO();
            }
            objectTranslationsDTO.ElementGuid = ElementGuid;
            objectTranslationsDTO.Element = Element;
            objectTranslationsDTO.TableObject = Object;
            objectTranslationsDTO.Translation = txtTranslation.Text;
            objectTranslationsDTO.LanguageId = Convert.ToInt32(cmbLanguage.SelectedValue);
            ObjectTranslations objectTranslations=new ObjectTranslations(machineUserContext, objectTranslationsDTO);
            objectTranslations.Save();
            if (objectTranslationsDTO.Id==-1)
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(122));
            }
            log.LogMethodExit();
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(machineUserContext);
            List<ObjectTranslationsDTO>  objectTranslationsDTOList = new List<ObjectTranslationsDTO>();
            List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchByObjectTranslationsParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
            if (!cmbLanguage.Text.Equals("Semnox.Parafait.Languages.LanguagesDTO") && cmbLanguage.SelectedValue!=null)
            {
                searchByObjectTranslationsParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, ElementGuid));
                searchByObjectTranslationsParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT, Element));
                searchByObjectTranslationsParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT, Object));
                searchByObjectTranslationsParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID, Convert.ToInt32(cmbLanguage.SelectedValue).ToString()));
                searchByObjectTranslationsParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                objectTranslationsDTOList = objectTranslationsList.GetAllObjectTranslations(searchByObjectTranslationsParameters);
                if (objectTranslationsDTOList != null)
                {
                    objectTranslationsDTO = objectTranslationsDTOList[0];
                    txtTranslation.Text = objectTranslationsDTO.Translation;
                }
                else
                {
                    objectTranslationsDTO = null;
                    txtTranslation.Text = null;
                }
            }
            else
            {
                objectTranslationsDTO = null;
                txtTranslation.Text = null;
            }
            log.LogMethodExit();
        }
    }
}
