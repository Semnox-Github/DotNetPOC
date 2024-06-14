/********************************************************************************************
 * Project Name - Customer
 * Description  - user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.4.0       25-Nov-2018      Raghuveera       Created
 *2.70.2      10-Aug-2019      Girish kundar    Modified :Removed Unused namespace's.
 *2.90.0      10-Jul-2020      Girish kundar    Modified : Phase -2 REST api changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    public partial class CustomerTermsandConditionsUI : Form
    {
        private Utilities utilities;
        private string applicability;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CustomerTermsandConditionsUI(Utilities utilities,string applicability)
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.utilities = utilities;
            this.applicability = applicability;
            LoadRichContent();
            log.LogMethodExit();
        }
        bool LoadRichContent()
        {
            log.LogMethodEntry();
            try
            {
                int richcontentId = -1;
                List<RichContentDTO> richContentDTOList = new List<RichContentDTO>();
                List<ApplicationContentDTO> applicationContentDTOList;
                ApplicationContentListBL applicationContentListBL = new ApplicationContentListBL(utilities.ExecutionContext);
                List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchApplicationParams = new List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>>();
                searchApplicationParams.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.MODULE, "REGISTRATION"));
                searchApplicationParams.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.APPLICATION, applicability));
                searchApplicationParams.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                applicationContentDTOList = applicationContentListBL.GetApplicationContentDTOList(searchApplicationParams,true,true);
                if (applicationContentDTOList != null && applicationContentDTOList.Count > 0)
                {
                    if (applicationContentDTOList[0].ApplicationContentTranslatedDTOList != null && applicationContentDTOList[0].ApplicationContentTranslatedDTOList.Count > 0)
                    {
                        if (applicationContentDTOList[0].ApplicationContentTranslatedDTOList.Exists(x => (bool)(x.LanguageId == utilities.ParafaitEnv.LanguageId)))
                        {
                            richcontentId = applicationContentDTOList[0].ApplicationContentTranslatedDTOList.Where(x => (bool)(x.LanguageId == utilities.ParafaitEnv.LanguageId)).ToList<ApplicationContentTranslatedDTO>()[0].ContentId;
                        }
                    }
                    if (richcontentId == -1)
                    {
                        richcontentId = applicationContentDTOList[0].ContentId;
                    }
                }

                RichContentListBL richContentListBL = new RichContentListBL(utilities.ExecutionContext);
                List<KeyValuePair<RichContentDTO.SearchByParameters, string>> searchRichParams = new List<KeyValuePair<RichContentDTO.SearchByParameters, string>>();
                searchRichParams.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.ID, richcontentId.ToString()));
                richContentDTOList = richContentListBL.GetRichContentDTOList(searchRichParams);

                if (richContentDTOList == null || (richContentDTOList != null && richContentDTOList.Count == 0))
                {
                    wbTerms.Url = null;
                }
                else
                {
                    try
                    {
                        byte[] bytes = richContentDTOList[0].Data;
                        if (bytes != null)
                        {
                            string extension = richContentDTOList[0].FileName;
                            try
                            {
                                extension = (new FileInfo(extension)).Extension;
                            }
                            catch { }
                            string tempFile = System.IO.Path.GetTempPath() + "ParafaitPOSTerms" + Guid.NewGuid().ToString() + extension;
                            using (FileStream file = new System.IO.FileStream(tempFile, FileMode.Create, System.IO.FileAccess.Write))
                            {
                                file.Write(bytes, 0, bytes.Length);
                            }
                            
                            wbTerms.Url = new Uri(tempFile);
                            this.Tag = richContentDTOList[0];
                            return true;
                        }
                        else
                        {
                            throw new Exception("Content data is not available");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        btnYes.Enabled = false;
                    }
                }
                log.LogMethodExit();
                return false;
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return false;
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (wbTerms.Document != null)
                {
                    int x = wbTerms.Document.Body.ScrollRectangle.Height;
                    if(vScrollBar.Maximum!=x)
                    vScrollBar.Maximum = x;
                    wbTerms.Document.Window.ScrollTo(new Point(0, e.NewValue));
                } 
            }
            catch { }
            log.LogMethodExit();
        }
    }
}
