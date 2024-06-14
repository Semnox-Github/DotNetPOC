/********************************************************************************************
 * Project Name - Waiver
 * Description  - waiverSetUI class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Girish kundar    Modified :Added Logger methods and Removed Unused namespace's. 
 *2.70.2        15-Oct-2019   GUru S A         Waiver phase 2 changes
 *********************************************************************************************/
using Semnox.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    /// WaiverSet  UI
    /// </summary>
    public partial class waiverSetUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        BindingSource waiverSetListBS = new BindingSource();
        string waiverImageName = string.Empty;
        string waiverImageFileName = string.Empty;
        /// <summary>
        /// Constructor of WaiverSetUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public waiverSetUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvWaiverSet);
            utilities.setupDataGridProperties(ref dgvWaiverDetail);
            utilities.setupDataGridProperties(ref dgvLanguage);

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
            LoadLanguages();
            log.LogMethodExit(null);
        }

        private void WaiverSetUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            RefreshData();
            log.LogMethodExit(null);
        }


        private int getWaiverId()
        {
            log.LogMethodEntry();
            int id = -1;
            try
            {
                if (dgvWaiverSet.RowCount > 0 && dgvWaiverSet.CurrentRow != null)
                {
                    log.Debug("Get the WaiverId from the grid cell");
                    if (dgvWaiverSet.Rows[dgvWaiverSet.CurrentRow.Index].Cells["waiverSetIdDataGridViewTextBoxColumn"].Value != null)
                    {
                        int.TryParse(dgvWaiverSet.Rows[dgvWaiverSet.CurrentRow.Index].Cells["waiverSetIdDataGridViewTextBoxColumn"].Value.ToString(), out id);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit(id);
            return id;
        }

        private string getGuid()
        {
            log.LogMethodEntry();
            string ElementGuid = "";
            try
            {
                if (dgvWaiverDetail.RowCount > 0 && dgvWaiverDetail.CurrentRow != null)
                {
                    log.Debug("Get the Guid from the grid cell");
                    if (dgvWaiverDetail.Rows[dgvWaiverDetail.CurrentRow.Index].Cells["waiverSetDetailGUIDDataGridViewTextBoxColumn"].Value != null)
                    {
                        ElementGuid = dgvWaiverDetail.Rows[dgvWaiverDetail.CurrentRow.Index].Cells["waiverSetDetailGUIDDataGridViewTextBoxColumn"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit(ElementGuid);
            return ElementGuid;
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadWaiverSet();
            LoadWaiverSetDetails(getWaiverId());
            LoadWaiverSetDetailLanguages(getGuid());
            log.LogMethodExit(null);
        }


        private void LoadLanguages()
        {
            log.LogMethodEntry();
            Semnox.Parafait.Languages.Languages languages = new Semnox.Parafait.Languages.Languages(machineUserContext);
            List<Semnox.Parafait.Languages.LanguagesDTO> languagesDTOList = new List<Semnox.Parafait.Languages.LanguagesDTO>();

            List<KeyValuePair<Semnox.Parafait.Languages.LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<Semnox.Parafait.Languages.LanguagesDTO.SearchByParameters, string>>();

            if (machineUserContext.GetSiteId() > 0)
                searchParameters.Add(new KeyValuePair<Semnox.Parafait.Languages.LanguagesDTO.SearchByParameters, string>(Semnox.Parafait.Languages.LanguagesDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            languagesDTOList = languages.GetAllLanguagesList(searchParameters);
            if (languagesDTOList == null)
            {
                languagesDTOList = new List<Semnox.Parafait.Languages.LanguagesDTO>();
            }
            languagesDTOList.Insert(0, new Semnox.Parafait.Languages.LanguagesDTO());
            languagesDTOList[0].LanguageName = "<SELECT>";
            languageIdDataGridViewTextBoxColumn.DataSource = languagesDTOList;
            languageIdDataGridViewTextBoxColumn.ValueMember = "LanguageId";
            languageIdDataGridViewTextBoxColumn.DisplayMember = "LanguageName";
            log.LogMethodExit(null);
        }


        private void LoadWaiverSet()
        {
            log.LogMethodEntry();

            try
            {
                WaiverSetListBL waversetListBL = new WaiverSetListBL(machineUserContext);

                List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParameters = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
                searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                if (chbShowActiveEntries.Checked)
                    searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, "1"));

                List<WaiverSetDTO> waiverSetDTOList = waversetListBL.GetWaiverSetDTOList(searchParameters);
                SortableBindingList<WaiverSetDTO> waiverSetDTOSortableList;
                if (waiverSetDTOList != null)
                {
                    waiverSetDTOSortableList = new SortableBindingList<WaiverSetDTO>(waiverSetDTOList);
                }
                else
                {
                    waiverSetDTOSortableList = new SortableBindingList<WaiverSetDTO>();
                }
                waiverSetListBS.DataSource = waiverSetDTOSortableList;
                dgvWaiverSet.DataSource = waiverSetListBS;
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex, "Error occurred while loading WaiverSetDTO list");
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(null);

        }

        private void LoadWaiverSetDetails(int waiverId)
        {

            log.LogMethodEntry(waiverId);

            List<WaiverSetDetailDTO> waiverSetDetailDTOList = new List<WaiverSetDetailDTO>();
            List<KeyValuePair<WaiverSetDetailDTO.SearchByWaiverSetDetail, string>> searchParameters = new List<KeyValuePair<WaiverSetDetailDTO.SearchByWaiverSetDetail, string>>();

            if (chbShowActiveEntries.Checked)
                searchParameters.Add(new KeyValuePair<WaiverSetDetailDTO.SearchByWaiverSetDetail, string>(WaiverSetDetailDTO.SearchByWaiverSetDetail.IS_ACTIVE, "1"));

            SortableBindingList<WaiverSetDetailDTO> waiverSetDTOSortableList = new SortableBindingList<WaiverSetDetailDTO>();
            if (waiverId > 0)
            {
                WaiverSetDetailListBL waiverSetdetailListBL = new WaiverSetDetailListBL(machineUserContext);
                searchParameters.Add(new KeyValuePair<WaiverSetDetailDTO.SearchByWaiverSetDetail, string>(WaiverSetDetailDTO.SearchByWaiverSetDetail.WAIVERSET_ID, waiverId.ToString()));
                List<WaiverSetDetailDTO> waiverSetDTOList = waiverSetdetailListBL.GetWaiverSetDetailDTOList(searchParameters);

                if (waiverSetDTOList != null)
                {
                    waiverSetDTOSortableList = new SortableBindingList<WaiverSetDetailDTO>(waiverSetDTOList);
                }
                else
                {
                    waiverSetDTOSortableList = new SortableBindingList<WaiverSetDetailDTO>();
                }
            }

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = waiverSetDTOSortableList;
            dgvWaiverDetail.DataSource = bindingSource;

            log.LogMethodExit();

        }


        private void LoadWaiverSetDetailLanguages(string guid)
        {
            log.LogMethodEntry(guid);
            if (guid == "")
            {
                guid = Guid.NewGuid().ToString();
            }

            List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
            SortableBindingList<ObjectTranslationsDTO> objectTranslationsDTOSortableList = new SortableBindingList<ObjectTranslationsDTO>();

            if (guid != null && guid != "")
            {
                ObjectTranslationsList objectTransaltionListBL = new ObjectTranslationsList(machineUserContext);
                searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, guid.ToString()));
                List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTransaltionListBL.GetAllObjectTranslations(searchParameters);
                if (objectTranslationsDTOList != null)
                {
                    objectTranslationsDTOSortableList = new SortableBindingList<ObjectTranslationsDTO>(objectTranslationsDTOList);
                }
            }

            BindingSource bindingSources = new BindingSource();
            bindingSources.DataSource = objectTranslationsDTOSortableList;
            dgvLanguage.DataSource = bindingSources;
            log.LogMethodExit();
        }





        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshData();
            log.LogMethodExit(null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit(null);
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            SaveWaiverSet();
            SaveWaiverSetDetail();
            SaveWaiverSetDetailLanguage();
            RefreshData();
            log.LogMethodExit(null);
        }

        private bool SaveWaiverSet()
        {
            dgvWaiverSet.EndEdit();
            SortableBindingList<WaiverSetDTO> waiverSetDTOSortableList = waiverSetListBS.DataSource as SortableBindingList<WaiverSetDTO>;
            string message = string.Empty;
            if (waiverSetDTOSortableList != null && waiverSetDTOSortableList.Count > 0)
            {
                for (int i = 0; i < waiverSetDTOSortableList.Count; i++)
                {
                    if (waiverSetDTOSortableList[i].IsChanged)
                    {
                        message = ValidateWaiverSetDTO(waiverSetDTOSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                WaiverSetBL waiverSetBL = new WaiverSetBL(machineUserContext, waiverSetDTOSortableList[i]);
                                waiverSetBL.Save();
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occurred while saving the record", ex);
                                dgvWaiverSet.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1440));
                                break;
                            }
                        }
                        else
                        {
                            dgvWaiverSet.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }

            return true;
        }




        private bool SaveWaiverSetDetail()
        {
            log.LogMethodEntry();
            int waiverSetId = -1;
            string message = string.Empty;

            try
            {
                log.Debug("Get the waiverSetId from the grid cell");
                if (dgvWaiverSet.RowCount > 0 && dgvWaiverSet.CurrentRow.Cells["waiverSetIdDataGridViewTextBoxColumn"].Value != null)
                {
                    int.TryParse(dgvWaiverSet.CurrentRow.Cells["waiverSetIdDataGridViewTextBoxColumn"].Value.ToString(), out waiverSetId);
                }

                if (waiverSetId <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            string directory = utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            BindingSource bindingSource = new BindingSource();
            bindingSource = (BindingSource)dgvWaiverDetail.DataSource;
            SortableBindingList<WaiverSetDetailDTO> waiverDetailList = (SortableBindingList<WaiverSetDetailDTO>)bindingSource.DataSource;
            for (int i = 0; i < waiverDetailList.Count; i++)
            {
                if (waiverDetailList[i].IsChanged)
                {
                    message = ValidateWaiverDetailDTO(waiverDetailList[i]);
                    if (string.IsNullOrEmpty(message))
                    {
                        try
                        {
                            WaiverSetDetailBL waiverSetDetailBL;
                            waiverSetDetailBL = new WaiverSetDetailBL(machineUserContext, waiverDetailList[i]);
                            waiverDetailList[i].WaiverSetId = waiverSetId;
                            waiverSetDetailBL.Save();
                            //Begin: save Waiver image
                            foreach (DataGridViewRow row in dgvWaiverDetail.Rows)
                            {
                                if (row.Cells["waiverFileNameDataGridViewTextBoxColumn"] != null && row.Cells["waiverFileNameDataGridViewTextBoxColumn"].Value != null)
                                {
                                    if (row.Cells["dgTxtWaiverFileLocation"].Value != null && File.Exists(row.Cells["dgTxtWaiverFileLocation"].Value.ToString()))
                                    {
                                        waiverImageName = row.Cells["waiverFileNameDataGridViewTextBoxColumn"].Value.ToString();
                                        waiverImageFileName = row.Cells["dgTxtWaiverFileLocation"].Value.ToString();
                                        if (waiverImageName != "" && waiverImageFileName != "")
                                        {
                                            SqlCommand commandImage = utilities.getCommand();
                                            commandImage.CommandText = "exec SaveBinaryDataToFile @Image, @FileName";
                                            commandImage.Parameters.AddWithValue("@Image", System.IO.File.ReadAllBytes(waiverImageFileName));
                                            commandImage.Parameters.AddWithValue("@FileName", directory + "\\" + waiverImageName);
                                            commandImage.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while saving the record", ex);
                            dgvWaiverDetail.Rows[i].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(1437));
                            break;
                        }

                    }
                    else
                    {
                        dgvWaiverDetail.Rows[i].Selected = true;
                        MessageBox.Show(message);
                        break;
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }


        private bool SaveWaiverSetDetailLanguage()
        {

            log.LogMethodEntry();
            string ElementGuid = "";
            string message = string.Empty;

            if (dgvWaiverDetail.CurrentRow == null || dgvLanguage.CurrentRow == null)
            {
                return false;
            }

            string directory = utilities.getParafaitDefaults("IMAGE_DIRECTORY");


            BindingSource bindingSource = new BindingSource();
            bindingSource = (BindingSource)dgvLanguage.DataSource;
            SortableBindingList<ObjectTranslationsDTO> ObjectTranslationsList = (SortableBindingList<ObjectTranslationsDTO>)bindingSource.DataSource;
            if (ObjectTranslationsList != null)
            {
                int LanguageId = -1;
                for (int i = 0; i < ObjectTranslationsList.Count; i++)
                {
                    if (ObjectTranslationsList[i].IsChanged)
                    {
                        message = ValidateWaiverDetailLanguageDTO(ObjectTranslationsList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                if (dgvLanguage.RowCount > 0 && dgvLanguage.CurrentRow.Cells["languageIdDataGridViewTextBoxColumn"].Value != null)
                                {
                                    int.TryParse(dgvLanguage.CurrentRow.Cells["languageIdDataGridViewTextBoxColumn"].Value.ToString(), out LanguageId);
                                }

                                if (dgvWaiverDetail.RowCount > 0 && dgvWaiverDetail.CurrentRow.Cells["waiverSetDetailGUIDDataGridViewTextBoxColumn"].Value != null)
                                {
                                    ElementGuid = dgvWaiverDetail.CurrentRow.Cells["waiverSetDetailGUIDDataGridViewTextBoxColumn"].Value.ToString();
                                }

                                if (LanguageId == -1)
                                {
                                    return false;
                                }

                                ObjectTranslations ObjectTranslationsBL;
                                ObjectTranslationsList[i].ElementGuid = ElementGuid;
                                ObjectTranslationsList[i].TableObject = "WAIVERSETDETAILS";
                                ObjectTranslationsList[i].Element = "WAIVERFILENAME";

                                ObjectTranslationsBL = new ObjectTranslations(machineUserContext, ObjectTranslationsList[i]);
                                ObjectTranslationsBL.Save();

                                //Begin: save WaiverLanguage image
                                foreach (DataGridViewRow row in dgvLanguage.Rows)
                                {
                                    if (row.Cells["translationDataGridViewTextBoxColumn"] != null && row.Cells["translationDataGridViewTextBoxColumn"].Value != null)
                                    {
                                        if (row.Cells["dgTxtWaiverLanguageFileLocation"].Value != null && File.Exists(row.Cells["dgTxtWaiverLanguageFileLocation"].Value.ToString()))
                                        {
                                            waiverImageName = row.Cells["translationDataGridViewTextBoxColumn"].Value.ToString();
                                            waiverImageFileName = row.Cells["dgTxtWaiverLanguageFileLocation"].Value.ToString();
                                            if (waiverImageName != "" && waiverImageFileName != "")
                                            {
                                                SqlCommand commandImage = utilities.getCommand();
                                                commandImage.CommandText = "exec SaveBinaryDataToFile @Image, @FileName";
                                                commandImage.Parameters.AddWithValue("@Image", System.IO.File.ReadAllBytes(waiverImageFileName));
                                                commandImage.Parameters.AddWithValue("@FileName", directory + "\\" + waiverImageName);
                                                log.Debug("Execute SaveBinaryDataToFile DB procedure");
                                                commandImage.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occurred while saving the record", ex);
                                break;
                            }
                        }
                        else
                        {
                            dgvLanguage.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit("Ends-SaveWaiverSetDetailLanguage().");
            return true;

        }

        private string ValidateWaiverSetDTO(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(waiverSetDTO.Name) || string.IsNullOrWhiteSpace(waiverSetDTO.Name))
            {
                message = utilities.MessageUtils.getMessage(249);
                message = message.Replace("&1", waiverSetNameDataGridViewTextBoxColumn.HeaderText);

            }
            log.LogMethodExit(message);
            return message;
        }


        private string ValidateWaiverDetailDTO(WaiverSetDetailDTO waiverSetDetailDTO)
        {

            log.LogMethodEntry(waiverSetDetailDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(waiverSetDetailDTO.Name) || string.IsNullOrWhiteSpace(waiverSetDetailDTO.Name))
            {
                message = utilities.MessageUtils.getMessage(249);
                message = message.Replace("&1", waiverSetDetailNameDataGridViewTextBoxColumn.HeaderText);
            }
            else if (string.IsNullOrEmpty(waiverSetDetailDTO.WaiverFileName) || string.IsNullOrWhiteSpace(waiverSetDetailDTO.WaiverFileName))
            {
                message = utilities.MessageUtils.getMessage(249);
                message = message.Replace("&1", waiverFileNameDataGridViewTextBoxColumn.HeaderText);
            }


            log.LogMethodExit(message);
            return message;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvWaiverSet_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            int id = -1;
            try
            {
                log.Debug("Get the waiverSetId from the Grid cell");
                if (dgvWaiverSet.RowCount > 0 && dgvWaiverSet.Rows[e.RowIndex].Cells["waiverSetIdDataGridViewTextBoxColumn"].Value != null)
                {
                    int.TryParse(dgvWaiverSet.Rows[e.RowIndex].Cells["waiverSetIdDataGridViewTextBoxColumn"].Value.ToString(), out id);
                }
                log.LogVariableState("waiverSetId", id);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            try
            {
                LoadWaiverSetDetails(id);
                //LoadWaiverSetDetailLanguages(ElementGuid);
                LoadWaiverSetDetailLanguages(getGuid());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            log.LogMethodExit();

        }

        private void dgvWaiverSet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvWaiverSet.Columns[e.ColumnIndex].Name == "btnWaiverSignedOption")
            {
                int waiverSetId = -1;
                log.Debug("Get the waiverSetId from the Grid cell");
                if (dgvWaiverSet.CurrentRow.Cells["waiverSetIdDataGridViewTextBoxColumn"].Value != null)
                    int.TryParse(dgvWaiverSet.CurrentRow.Cells["waiverSetIdDataGridViewTextBoxColumn"].Value.ToString(), out waiverSetId);

                if (waiverSetId > 0)
                {
                    WaiverSigningOptionUI waiverSigningOptionUI = new WaiverSigningOptionUI(waiverSetId, utilities);
                    waiverSigningOptionUI.ShowDialog();
                }
            }

            if (dgvWaiverSet.Columns[e.ColumnIndex].Name == "isActiveDataGridViewCheckBoxColumn")
            {
                int waiverSetId = -1;
                if (dgvWaiverSet.CurrentRow.Cells["waiverSetIdDataGridViewTextBoxColumn"].Value != null)
                    int.TryParse(dgvWaiverSet.CurrentRow.Cells["waiverSetIdDataGridViewTextBoxColumn"].Value.ToString(), out waiverSetId);
                WaiverSetBL waiverSetBL = new WaiverSetBL(machineUserContext);
                int count = waiverSetBL.RefWaiverCount(waiverSetId);
                if (count >= 1)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1509));
                }
            }
            log.LogMethodExit();
        }


        private void btnFile_Click(object sender, EventArgs e)
        {

        }

        private void dgvWaiverDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvWaiverDetail.Columns[e.ColumnIndex].Name == "btnFileImage")
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Upload Files";
                fileDialog.Filter = "Pdf Files|*.pdf";

                if (fileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                try
                {
                    dgvWaiverDetail.CurrentRow.Cells["waiverFileNameDataGridViewTextBoxColumn"].Value = (new System.IO.FileInfo(fileDialog.FileName)).Name;
                    dgvWaiverDetail.CurrentRow.Cells["dgTxtWaiverFileLocation"].Value = (new System.IO.FileInfo(fileDialog.FileName)).FullName;
                }
                catch
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(601), utilities.MessageUtils.getMessage("Image File Error"));
                }
            }
            log.LogMethodExit();
        }


        private void dgvWaiverDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == validForDaysDataGridViewTextBoxColumn.Index)
            {
                if (e.Value != null)
                {
                    int value = Convert.ToInt32(e.Value);
                    if (value == -1)
                    {
                        e.Value = "";
                        e.FormattingApplied = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvLanguage_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvLanguage.Columns[e.ColumnIndex].Name == "btnfileUpload")
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Upload Files";
                fileDialog.Filter = "Pdf Files|*.pdf";
                if (fileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                try
                {
                    dgvLanguage.CurrentRow.Cells["translationDataGridViewTextBoxColumn"].Value = (new System.IO.FileInfo(fileDialog.FileName)).Name;
                    dgvLanguage.CurrentRow.Cells["dgTxtWaiverLanguageFileLocation"].Value = (new System.IO.FileInfo(fileDialog.FileName)).FullName;
                }
                catch
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(601), utilities.MessageUtils.getMessage("Image File Error"));
                }
            }
            log.LogMethodExit();
        }


        private void dgvWaiverSetDetail_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            log.LogMethodEntry();
            string ElementGuid = "";
            try
            {
                if (dgvWaiverDetail.RowCount > 0 && dgvWaiverDetail.Rows[e.RowIndex].Cells["waiverSetDetailGUIDDataGridViewTextBoxColumn"].Value != null && dgvWaiverDetail.Rows[e.RowIndex].Cells["waiverSetDetailIdDataGridViewTextBoxColumn"].Value != null)
                {
                    ElementGuid = dgvWaiverDetail.Rows[e.RowIndex].Cells["waiverSetDetailGUIDDataGridViewTextBoxColumn"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            try
            {
                LoadWaiverSetDetailLanguages(ElementGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            log.LogMethodExit();

        }


        private void btnSearch_Click(object sender, EventArgs e)
        {

        }


        private void chbShowActiveEntries_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadWaiverSet();
            log.LogMethodEntry(null);
        }

        private void dgvWaiverSetDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DGVDataError("Sequences", dgvWaiverDetail, e);
        }



        public void DGVDataError(string Name, DataGridView dgv, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(585, Name, e.RowIndex + 1, dgv.Columns[e.ColumnIndex].DataPropertyName) + ": " + e.Exception.Message, utilities.MessageUtils.getMessage("Data Error"));
            }
            catch { }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private string ValidateWaiverDetailLanguageDTO(ObjectTranslationsDTO objectTranslationsDTO)
        {
            log.LogMethodEntry(objectTranslationsDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(objectTranslationsDTO.Translation) || string.IsNullOrWhiteSpace(objectTranslationsDTO.Translation))
            {
                message = utilities.MessageUtils.getMessage(249);
                message = message.Replace("&1", translationDataGridViewTextBoxColumn.HeaderText);

            }
            else if (objectTranslationsDTO.LanguageId == -1)
            {
                message = utilities.MessageUtils.getMessage(249);
                message = message.Replace("&1", languageIdDataGridViewTextBoxColumn.HeaderText);

            }
            log.LogMethodExit(message);
            return message;
        }

    }
}
