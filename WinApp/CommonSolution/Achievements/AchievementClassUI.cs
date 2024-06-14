using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Product;
using System.Linq;
using Semnox.Parafait.Publish;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AchievementClassUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<AchievementClassDTO> achievementClassDTOList;
        bool classGridEdited = true;
        bool classLevelGridEdited = true;
        bool scoreConvertionEdited = true;



        //bool classGridAtLoad = true;
        //bool classLevelGridAtLoad = true;
        //bool scoreConvertionAtLoad = true;
        bool editedForm = false;


        /// <summary>
        /// Constructor used during object creation.
        /// </summary>
        public AchievementClassUI(Utilities _Utilities, int projectId)
        {
            log.Debug("Starts-AchivementClassUI(Utilities) parameterized constructor.");
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvAchievementClass);
            utilities.setupDataGridProperties(ref dgvAchievementClassLevel);
            utilities.setupDataGridProperties(ref dgvAchievementScoreConversion);
            ShowHidePublishLink();
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

            SetGridSettings();
            try
            {
                LoadProjects();
                if (projectId > 0)
                    drpProjects.SelectedValue = projectId;


                classGridEdited = true;
                classLevelGridEdited = true;
                scoreConvertionEdited = true;
                LoadAchievementClass(getProjectId());
                GetProduct();
                GetGames();
                LoadQualifyingLevel();
                GetLoyaltyAttributes();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            log.Debug("Ends-AchivementClassUI(Utilities) parameterized constructor.");
        }
        private void ShowHidePublishLink()
        {
            log.LogMethodEntry();
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublish.Visible = true;
            }
            else
            {
                lnkPublish.Visible = false;
            }
            log.LogMethodExit();
        }

        private void SetGridSettings()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleRight;
            QualifyingScore.DefaultCellStyle = BonusAmount.DefaultCellStyle = Ratio.DefaultCellStyle = style;
        }
        private int getProjectId()
        {
            int projectId = -1;
            try
            {
                int.TryParse(drpProjects.SelectedValue.ToString(), out projectId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return projectId;
        }

        private int getClassId()
        {

            int id = -1;
            try
            {

                if (dgvAchievementClass.RowCount > 0 && dgvAchievementClass.Rows[dgvAchievementClass.CurrentRow.Index].Cells["AchievementClassId"].Value != null)
                {
                    int.TryParse(dgvAchievementClass.Rows[dgvAchievementClass.CurrentRow.Index].Cells["AchievementClassId"].Value.ToString(), out id);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return id;
        }

        private int getClassLevelId()
        {

            int id = -1;
            try
            {
                if (dgvAchievementClassLevel.RowCount > 0 && dgvAchievementClassLevel.CurrentRow.Cells["AchievementClassLevelId"].Value != null)
                {
                    int.TryParse(dgvAchievementClassLevel.CurrentRow.Cells["AchievementClassLevelId"].Value.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return id;
        }
        private void LoadQualifyingLevel()
        {
            List<KeyValuePair<int, string>> QualifyingLevelList = new List<KeyValuePair<int, string>>();
            QualifyingLevelList.Add(new KeyValuePair<int, string>(-1, "Select"));


            //load Class
            AchievementClassesList achievementClassesList = new AchievementClassesList(machineUserContext);
            achievementClassDTOList = new List<AchievementClassDTO>();

            AchievementClassParams achievementClassParams = new AchievementClassParams();
            achievementClassParams.IsActive = false;
            achievementClassParams.SiteId = machineUserContext.GetSiteId();
            achievementClassParams.AchievementProjectId = getProjectId();

            achievementClassDTOList = achievementClassesList.GetAchievementClassList(achievementClassParams);
            AchievementClassLevelsList achievementClassLevelsList = new AchievementClassLevelsList(machineUserContext);
            List<AchievementClassLevelDTO> achievementClassLevelDTOList = new List<AchievementClassLevelDTO>();
            achievementClassLevelDTOList = new List<AchievementClassLevelDTO>();

            AchievementClassLevelParams achievementClassLevelParams = new AchievementClassLevelParams();
            achievementClassLevelParams.IsActive = false;
            achievementClassLevelParams.SiteId = machineUserContext.GetSiteId();

            foreach (AchievementClassDTO achievementClassDTO in achievementClassDTOList)
            {
                achievementClassLevelParams.AchievementClassId = achievementClassDTO.AchievementClassId;
                achievementClassLevelDTOList = achievementClassLevelsList.GetAchievementClassLevelList(achievementClassLevelParams);
                foreach (AchievementClassLevelDTO achievementClassLevelDTO in achievementClassLevelDTOList)
                {
                    QualifyingLevelList.Add(new KeyValuePair<int, string>(achievementClassLevelDTO.AchievementClassLevelId, achievementClassLevelDTO.LevelName));
                }

            }

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = QualifyingLevelList;

            drpQualifyingLevelId.DataSource = QualifyingLevelList;
            drpQualifyingLevelId.ValueMember = "Key";
            drpQualifyingLevelId.DisplayMember = "Value";



        }

        private void LoadProjects()
        {
            AchievementProjectsList achievementProjectsList = new AchievementProjectsList(machineUserContext);

            AchievementProjectParams achievementProjectParams = new AchievementProjectParams();
            achievementProjectParams.IsActive = false;
            List<AchievementProjectDTO> achievementProjectDTOList = achievementProjectsList.GetAchievementProjectsList(achievementProjectParams);


            if (achievementProjectDTOList == null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
                achievementProjectDTOList = new List<AchievementProjectDTO>();
            }
            //achievementProjectDTOList.Insert(0, new AchievementProjectDTO());
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = achievementProjectDTOList;

            drpProjects.DataSource = achievementProjectDTOList;
            drpProjects.ValueMember = "AchievementProjectId";
            drpProjects.DisplayMember = "ProjectName";

        }

        private void LoadAchievementClass(int projectId)
        {

            if (classGridEdited)
            {
                AchievementClassesList achievementClassesList = new AchievementClassesList(machineUserContext);
                achievementClassDTOList = new List<AchievementClassDTO>();

                AchievementClassParams achievementClassParams = new AchievementClassParams();
                achievementClassParams.IsActive = false;
                achievementClassParams.SiteId = machineUserContext.GetSiteId();
                achievementClassParams.AchievementProjectId = projectId;

                achievementClassDTOList = achievementClassesList.GetAchievementClassList(achievementClassParams);
                //achievementClassDTOList.Add(new AchievementClassDTO());
                SortableBindingList<AchievementClassDTO> sortedAchievementClassDTOList = new SortableBindingList<AchievementClassDTO>(achievementClassDTOList);

                if (sortedAchievementClassDTOList == null)
                {
                    sortedAchievementClassDTOList = new SortableBindingList<AchievementClassDTO>();
                }
                if (sortedAchievementClassDTOList.Count == 0)
                {
                    sortedAchievementClassDTOList.Add(new AchievementClassDTO());
                }
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = sortedAchievementClassDTOList;
                dgvAchievementClass.DataSource = bindingSource;
                classGridEdited = false;

                if (classLevelGridEdited)
                {
                    LoadAchievementClassLevel(getClassId());
                }
            }


        }

        private void LoadAchievementClassLevel(int achievementClassId)
        {
            if (classLevelGridEdited)
            {
                List<AchievementClassLevelDTO> achievementClassLevelDTOList = new List<AchievementClassLevelDTO>();
                SortableBindingList<AchievementClassLevelDTO> sortedAchievementClassLevelList = new SortableBindingList<AchievementClassLevelDTO>();
                if (achievementClassId >= 0)
                {
                    AchievementClassLevelsList achievementClassLevelsList = new AchievementClassLevelsList(machineUserContext);
                    achievementClassLevelDTOList = new List<AchievementClassLevelDTO>();

                    AchievementClassLevelParams achievementClassLevelParams = new AchievementClassLevelParams();
                    achievementClassLevelParams.IsActive = false;
                    achievementClassLevelParams.SiteId = machineUserContext.GetSiteId();
                    achievementClassLevelParams.AchievementClassId = achievementClassId;

                    achievementClassLevelDTOList = achievementClassLevelsList.GetAchievementClassLevelList(achievementClassLevelParams);

                    sortedAchievementClassLevelList = new SortableBindingList<AchievementClassLevelDTO>(achievementClassLevelDTOList);
                    if (sortedAchievementClassLevelList == null)
                    {
                        sortedAchievementClassLevelList = new SortableBindingList<AchievementClassLevelDTO>();
                    }
                }


                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = sortedAchievementClassLevelList;
                dgvAchievementClassLevel.DataSource = bindingSource;
                classLevelGridEdited = false;

                if (scoreConvertionEdited)
                {
                    LoadAchievementScoreConversion(-1);
                }
            }
        }

        private void LoadAchievementScoreConversion(int AchievementClassLevelId)
        {
            if (scoreConvertionEdited)
            {
                List<AchievementScoreConversionDTO> achievementScoreConversionDTOList = new List<AchievementScoreConversionDTO>();
                SortableBindingList<AchievementScoreConversionDTO> sortedAchievementScoreConversionDTOList = new SortableBindingList<AchievementScoreConversionDTO>();

                if (AchievementClassLevelId >= 0)
                {
                    AchievementScoreConversionsList achievementScoreConversionsList = new AchievementScoreConversionsList(machineUserContext);
                    achievementScoreConversionDTOList = new List<AchievementScoreConversionDTO>();
                    AchievementScoreConversionParams achievementScoreConversionParams = new AchievementScoreConversionParams()
                    {
                        AchievementClassLevelId = AchievementClassLevelId
                    };
                    achievementScoreConversionDTOList = achievementScoreConversionsList.GetAllAchievementScoreConversions(achievementScoreConversionParams);

                    sortedAchievementScoreConversionDTOList = new SortableBindingList<AchievementScoreConversionDTO>(achievementScoreConversionDTOList);

                    if (sortedAchievementScoreConversionDTOList == null)
                    {
                        sortedAchievementScoreConversionDTOList = new SortableBindingList<AchievementScoreConversionDTO>();
                    }
                }


                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = sortedAchievementScoreConversionDTOList;
                dgvAchievementScoreConversion.DataSource = bindingSource;
                scoreConvertionEdited = false;
            }
        }
        private void dgvAchievementClass_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.Debug("Starts-dgvAchievementClass_RowEnter() Event.");
            int id = -1;
            try
            {
                if (dgvAchievementClass.RowCount > 0 && dgvAchievementClass.CurrentRow.Cells["AchievementClassId"].Value != null)
                {
                    int.TryParse(dgvAchievementClass.CurrentRow.Cells["AchievementClassId"].Value.ToString(), out id);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            try
            {
                classLevelGridEdited = true;
                scoreConvertionEdited = true;
                LoadAchievementClassLevel(id);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.Debug("Ends-dgvAchievementClass_RowEnter() Event.");
        }
        private void dgvAchievementClass_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvAchievementClassLevel_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            log.Debug("Starts-dgvAchievementClassLevel_RowEnter() Event.");
            int id = -1;
            try
            {
                if (dgvAchievementClassLevel.RowCount > 0 && dgvAchievementClassLevel.Rows[e.RowIndex].Cells["AchievementClassLevelId"].Value != null)
                {
                    int.TryParse(dgvAchievementClassLevel.Rows[e.RowIndex].Cells["AchievementClassLevelId"].Value.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            try
            {
                scoreConvertionEdited = true;
                LoadAchievementScoreConversion(id);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            log.Debug("Ends-dgvAchievementClassLevel_RowEnter() Event.");
        }

        private void dgvAchievementScoreConversion_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                log.Debug("Starts-dgvAchievementScoreConversion_RowEnter() Event.");
                DataGridViewCheckBoxCell chk = new DataGridViewCheckBoxCell();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editedForm)
                {

                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                    return;

                }
                SaveAchievementClass();
                SaveAchievementClassLevel();
                SaveAchievementScoreConversion();

                if (classGridEdited)
                    LoadAchievementClass(getProjectId());

                if (classLevelGridEdited)
                    LoadAchievementClassLevel(getClassId());

                if (scoreConvertionEdited)
                    LoadAchievementScoreConversion(getClassLevelId());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefesh_Click() Event.");
            try
            {
                RefreshAchievements();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.Debug("Ends-btnRefesh_Click() Event.");
        }
        private void RefreshAchievements()
        {
            editedForm = false;
            classGridEdited = true;
            classLevelGridEdited = true;
            scoreConvertionEdited = true;
            LoadQualifyingLevel();
            LoadAchievementClass(getProjectId());
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDelete_Click() Event.");

            try
            {
                if (dgvAchievementScoreConversion.RowCount > 1)
                {

                    if ((MessageBox.Show(utilities.MessageUtils.getMessage(958), utilities.MessageUtils.getMessage(1184), MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        DeleteAchievementScoreConversion();
                    }
                }
                else if (dgvAchievementClassLevel.RowCount > 1)
                {
                    if ((MessageBox.Show(utilities.MessageUtils.getMessage(958), utilities.MessageUtils.getMessage(1185), MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        DeleteAchievementClassLevel();
                    }

                }
                else if (dgvAchievementClass.RowCount > 1)
                {
                    if ((MessageBox.Show(utilities.MessageUtils.getMessage(958), utilities.MessageUtils.getMessage(1186), MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        DeleteAchievementClass();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
            log.Debug("Ends-btnDelete_Click() Event.");
        }
        private void dgvAchievementClassLevel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (dgvAchievementClassLevel.Columns[e.ColumnIndex].Name == "btndgSaveImage")
                {
                    int id = -1;
                    if (int.TryParse(dgvAchievementClassLevel.CurrentRow.Cells["AchievementClassLevelId"].Value.ToString(), out id))
                    {
                        if (id == -1)
                            return;
                    }
                    try
                    {
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Title = "Upload Image";
                        fileDialog.Filter = "Image Files (*.gif, *.jpg, *.jpeg, *.wmf, *.png, *.ico, *.bmp)|*.gif; *.jpg; *.jpeg; *.wmf; *.png; *.ico; *.bmp|All Files(*.*)|*.*";
                        if (fileDialog.ShowDialog() == DialogResult.Cancel)
                            return;

                        dgvAchievementClassLevel.CurrentRow.Cells["Picture"].Value = (new System.IO.FileInfo(fileDialog.FileName)).Name;
                        dgvAchievementClassLevel.CurrentRow.Cells["dgTxtImagePath"].Value = (new System.IO.FileInfo(fileDialog.FileName)).FullName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error", ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);

            }
        }

        private void dgvAchievementClass_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            classLevelGridEdited = true;
            LoadAchievementClassLevel(getClassId());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvAchievementClass_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            classGridEdited = true;
            editedForm = true;
            log.Debug("Starts-dgvAchievementClass_CellValueChanged() Event.");

            try
            {
                if (dgvAchievementClass.CurrentRow != null && dgvAchievementClass.CurrentRow.Cells["ClassName"].Value != null)
                {
                    string name = dgvAchievementClass.CurrentRow.Cells["ClassName"].Value.ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        dgvAchievementClass.Rows.Remove(dgvAchievementClass.CurrentRow);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.Debug("Ends-dgvAchievementClass_CellValueChanged() Event.");

        }

        private void dgvAchievementClassLevel_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            classLevelGridEdited = true;
            editedForm = true;
        }

        private void dgvAchievementScoreConversion_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            scoreConvertionEdited = true;
            editedForm = true;
        }

        private void drpProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-drpProjects_SelectedIndexChanged() Event.");
            classGridEdited = true;
            classLevelGridEdited = true;
            scoreConvertionEdited = true;
            dgvAchievementClassLevel.DataSource = null;
            LoadQualifyingLevel();
            LoadAchievementClass(getProjectId());

            log.Debug("Ends-drpProjects_SelectedIndexChanged() Event.");
        }

        private void dgvAchievementClass_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                log.Debug("Starts-dgvAchievementClass_DataError() Event.");
                MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + dgvAchievementClass.Columns[e.ColumnIndex].DataPropertyName +
                   ": " + e.Exception.Message);
                e.Cancel = true;
                log.Debug("Ends-dgvAchievementClass_DataError() Event.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        private void dgvAchievementClassLevel_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                log.Debug("Starts-dgvAchievementClassLevel_DataError() Event.");
                MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + dgvAchievementClassLevel.Columns[e.ColumnIndex].DataPropertyName +
                   ": " + e.Exception.Message);
                e.Cancel = true;
                log.Debug("Ends-dgvAchievementClassLevel_DataError() Event.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void dgvAchievementScoreConversion_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                log.Debug("Starts-dgvAchievementScoreConversion_DataError() Event.");
                MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + dgvAchievementScoreConversion.Columns[e.ColumnIndex].DataPropertyName +
                   ": " + e.Exception.Message);
                e.Cancel = true;
                log.Debug("Ends-dgvAchievementScoreConversion_DataError() Event.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private bool SaveAchievementClass()
        {
            if (classGridEdited)
            {
                int projectId = getProjectId();
                if (projectId == -1)
                {

                    MessageBox.Show(utilities.MessageUtils.getMessage(1179));
                    return false;
                }

                BindingSource achievementClassListBS = (BindingSource)dgvAchievementClass.DataSource;
                var achievementClassDTOOnDisplay = (SortableBindingList<AchievementClassDTO>)achievementClassListBS.DataSource;


                foreach (AchievementClassDTO achievementClassDTO in achievementClassDTOOnDisplay)
                {
                    // var achievementClassDTO = (AchievementClassDTO)selectedRow.DataBoundItem;
                    achievementClassDTO.AchievementProjectId = projectId;
                    achievementClassDTO.IsChanged = true;
                    AchievementClass achievementClass = new AchievementClass(machineUserContext, achievementClassDTO);
                    if (string.IsNullOrEmpty(achievementClass.GetAchievementClassDTO.ClassName))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1180));
                        return false;
                    }

                    achievementClass.Save(null);
                }
            }
            return true;
        }

        private bool SaveAchievementClassLevel()
        {
            int achievementClassId = -1;
            if (classLevelGridEdited)
            {
                try
                {
                    if (dgvAchievementClass.RowCount > 0 && dgvAchievementClass.CurrentRow.Cells["AchievementClassId"].Value != null)
                    {
                        int.TryParse(dgvAchievementClass.CurrentRow.Cells["AchievementClassId"].Value.ToString(), out achievementClassId);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                BindingSource bindingSource = new BindingSource();
                bindingSource = (BindingSource)dgvAchievementClassLevel.DataSource;
                SortableBindingList<AchievementClassLevelDTO> achievementClassLevelList = (SortableBindingList<AchievementClassLevelDTO>)bindingSource.DataSource;
                string directory = utilities.getParafaitDefaults("IMAGE_DIRECTORY");
                foreach (AchievementClassLevelDTO achievementClassLevelDTO in achievementClassLevelList)
                {
                    if (string.IsNullOrEmpty(achievementClassLevelDTO.LevelName))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1181));
                        return false;
                    }
                    if (achievementClassId < 0)
                    {
                        return false;
                    }
                    AchievementClassLevel achievementClassLevel = new AchievementClassLevel(machineUserContext, achievementClassLevelDTO);
                    achievementClassLevelDTO.AchievementClassId = achievementClassId;
                    achievementClassLevel.Save(null);

                    foreach (DataGridViewRow row in dgvAchievementClassLevel.Rows)
                    {
                        if (row.Cells["AchievementClassLevelId"] != null && row.Cells["AchievementClassLevelId"].Value != null && achievementClassLevelDTO.AchievementClassLevelId.ToString() == row.Cells["AchievementClassLevelId"].Value.ToString())
                        {
                            if (row.Cells["dgTxtImagePath"].Value != null && File.Exists(row.Cells["dgTxtImagePath"].Value.ToString()))
                            {
                                string fileName = row.Cells["dgTxtImagePath"].Value.ToString();

                                if (!Directory.Exists(directory))
                                    Directory.CreateDirectory(directory);
                                string fileFullPath = Path.Combine(directory, row.Cells["Picture"].Value.ToString());
                                if (!File.Exists(fileFullPath))
                                    File.Copy(fileName, fileFullPath);
                            }
                        }
                    }
                }
                LoadQualifyingLevel();
            }
            return true;

        }

        private bool SaveAchievementScoreConversion()
        {
            int AchievementClassLevelId = -1;
            if (scoreConvertionEdited)
            {
                try
                {
                    if (dgvAchievementClassLevel.RowCount > 0 && dgvAchievementClassLevel.CurrentRow.Cells["AchievementClassLevelId"].Value != null)
                    {
                        int.TryParse(dgvAchievementClassLevel.CurrentRow.Cells["AchievementClassLevelId"].Value.ToString(), out AchievementClassLevelId);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return false;
                }
                if (AchievementClassLevelId < 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1181));
                    return false;
                }
                BindingSource bindingSource = new BindingSource();
                bindingSource = (BindingSource)dgvAchievementScoreConversion.DataSource;
                SortableBindingList<AchievementScoreConversionDTO> AchievementScoreConversionList = (SortableBindingList<AchievementScoreConversionDTO>)bindingSource.DataSource;
                foreach (AchievementScoreConversionDTO achievementScoreConversionDTO in AchievementScoreConversionList)
                {
                    AchievementScoreConversion achievementScoreConversion = new AchievementScoreConversion(machineUserContext, achievementScoreConversionDTO);
                    achievementScoreConversionDTO.AchievementClassLevelId = AchievementClassLevelId;

                    if ((achievementScoreConversionDTO.FromDate > DateTime.MinValue || achievementScoreConversionDTO.FromDate > DateTime.MinValue))
                    {
                        if (achievementScoreConversionDTO.FromDate > achievementScoreConversionDTO.ToDate)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1183));
                            return false;
                        }
                    }
                    if (achievementScoreConversionDTO.FromDate < achievementScoreConversionDTO.ToDate && achievementScoreConversionDTO.FromDate == DateTime.MinValue)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1183));
                        return false;
                    }
                    achievementScoreConversion.Save(null);
                }
            }
            return true;
        }

        private void GetProduct()
        {
            try
            {
                Products products = new Products();

                ProductsFilterParams productsFilterParams = new ProductsFilterParams()
                {
                    MachineName = utilities.ParafaitEnv.POSMachine,

                    ProductType = "ACHIEVEMENTS"
                };
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    productsFilterParams.SiteId = utilities.ParafaitEnv.SiteId;
                }
                List<ProductsDTO> productList = products.GetProductDTOList(productsFilterParams);
                ProductsDTO productsDTO = new ProductsDTO();
                productsDTO.ProductName = "--Select--";

                productList.Insert(0, productsDTO);
                ProductId.DataSource = productList;
                ProductId.ValueMember = "ProductId";
                ProductId.DisplayMember = "ProductName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void GetGames()
        {
            try
            {
                GameList gameList = new GameList();
                List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                List<GameDTO> gameDTOList = gameList.GetGameList(searchParameters);
                GameDTO gameDTO = new GameDTO()
                {
                    GameName = "--Select--",
                    GameId = -1
                };
                if (gameDTOList == null)
                {
                    gameDTOList = new List<GameDTO>();

                }
                gameDTOList.Insert(0, gameDTO);
                drpGameId.DataSource = gameDTOList;
                drpGameId.ValueMember = "GameId";
                drpGameId.DisplayMember = "GameName";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void GetLoyaltyAttributes()
        {
            try
            {
                List<LoyaltyAttributesDTO> LoyaltyAttributesList = new AchievementScoreConversionsList(machineUserContext).GetLoyaltyAttributes();
                LoyaltyAttributesDTO loyaltyAttributesDTO = new LoyaltyAttributesDTO();
                loyaltyAttributesDTO.CreditPlusType = "";

                LoyaltyAttributesList.Insert(0, loyaltyAttributesDTO);
                BonusEntitlement.DataSource = LoyaltyAttributesList;
                BonusEntitlement.ValueMember = "CreditPlusType";
                BonusEntitlement.DisplayMember = "Attribute";
                ConversionEntitlement.DataSource = LoyaltyAttributesList;
                ConversionEntitlement.ValueMember = "CreditPlusType";
                ConversionEntitlement.DisplayMember = "Attribute";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void DeleteAchievementClass()
        {
            try
            {
                int id = -1;
                AchievementClass achievementClass = null;

                //if (dgvAchievementClass.CurrentRow == null)
                //{
                //    return;
                //}
                if (dgvAchievementClass.CurrentRow != null)
                {
                    int.TryParse(dgvAchievementClass.CurrentRow.Cells["AchievementClassId"].Value.ToString(), out id);
                    if (id > 0)
                    {
                        achievementClass = new AchievementClass(machineUserContext, id);
                        achievementClass.GetAchievementClassDTO.IsActive = false;
                        achievementClass.Save(null);
                    }
                    if (!dgvAchievementClass.CurrentRow.IsNewRow)
                        dgvAchievementClass.Rows.Remove(dgvAchievementClass.CurrentRow);

                }
                else
                {
                    int count = dgvAchievementClass.RowCount;
                    dgvAchievementClass.Rows[count - 1].Selected = true;
                    int.TryParse(dgvAchievementClass.Rows[count - 1].Cells["AchievementClassId"].Value.ToString(), out id);
                    if (id > 0)
                    {
                        achievementClass = new AchievementClass(machineUserContext, id);
                        achievementClass.GetAchievementClassDTO.IsActive = false;
                        achievementClass.Save(null);
                    }
                    if (!dgvAchievementClass.Rows[count - 1].IsNewRow)
                        dgvAchievementClass.Rows.Remove(dgvAchievementClass.Rows[count - 1]);
                }
                classGridEdited = true;
                LoadAchievementClass(getProjectId());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void DeleteAchievementClassLevel()
        {
            try
            {
                int id = -1;
                AchievementClassLevel achievementClassLevel = null;


                if (dgvAchievementClassLevel.CurrentRow != null)
                {
                    int.TryParse(dgvAchievementClassLevel.CurrentRow.Cells["AchievementClassLevelId"].Value.ToString(), out id);
                    if (id > 0)
                    {
                        achievementClassLevel = new AchievementClassLevel(machineUserContext, id);
                        achievementClassLevel.GetAchievementClassLevelDTO.IsActive = false;
                        achievementClassLevel.Save(null);
                    }
                    if (!dgvAchievementClassLevel.CurrentRow.IsNewRow)
                        dgvAchievementClassLevel.Rows.Remove(dgvAchievementClassLevel.CurrentRow);

                }
                else
                {
                    int count = dgvAchievementClassLevel.RowCount;
                    int.TryParse(dgvAchievementClassLevel.Rows[count - 1].Cells["AchievementClassLevelId"].Value.ToString(), out id);
                    if (id > 0)
                    {
                        achievementClassLevel = new AchievementClassLevel(machineUserContext, id);
                        achievementClassLevel.GetAchievementClassLevelDTO.IsActive = false;
                        achievementClassLevel.Save(null);
                    }
                    if (!dgvAchievementClassLevel.Rows[count - 1].IsNewRow)
                        dgvAchievementClassLevel.Rows.Remove(dgvAchievementClassLevel.Rows[count - 1]);
                }
                classLevelGridEdited = true;
                LoadAchievementClassLevel(getClassId());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void DeleteAchievementScoreConversion()
        {
            int id = -1;
            AchievementScoreConversion achievementScoreConversion = null;

            try
            {
                if (dgvAchievementScoreConversion.CurrentRow != null)
                {
                    int.TryParse(dgvAchievementScoreConversion.CurrentRow.Cells["Id"].Value.ToString(), out id);
                    if (id > 0)
                    {
                        achievementScoreConversion = new AchievementScoreConversion(machineUserContext, id);
                        achievementScoreConversion.GetAchievementScoreConversionDTO.IsActive = false;
                        achievementScoreConversion.Save(null);
                    }
                    if (!dgvAchievementScoreConversion.CurrentRow.IsNewRow)
                        dgvAchievementScoreConversion.Rows.Remove(dgvAchievementScoreConversion.CurrentRow);

                }
                else
                {
                    int count = dgvAchievementScoreConversion.RowCount;
                    int.TryParse(dgvAchievementClass.Rows[count - 1].Cells["Id"].Value.ToString(), out id);
                    if (id > 0)
                    {
                        achievementScoreConversion = new AchievementScoreConversion(machineUserContext, id);
                        achievementScoreConversion.GetAchievementScoreConversionDTO.IsActive = false;
                        achievementScoreConversion.Save(null);
                    }
                    if (!dgvAchievementScoreConversion.Rows[count - 1].IsNewRow)
                        dgvAchievementScoreConversion.Rows.Remove(dgvAchievementScoreConversion.Rows[count - 1]);
                }
                scoreConvertionEdited = true;
                LoadAchievementScoreConversion(getClassLevelId());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void dgvAchievementClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            try
            {
                dgvAchievementClass.CurrentRow.Selected = true;
                classLevelGridEdited = true;
                scoreConvertionEdited = true;
                LoadAchievementClassLevel(getClassId());
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
            }
        }

        private void dgvAchievementClass_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                classLevelGridEdited = true;
                scoreConvertionEdited = true;
                LoadAchievementClassLevel(getClassId());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                int achievementProjectId = -1;
                try
                {
                    int.TryParse(drpProjects.SelectedValue.ToString(), out achievementProjectId);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                if (achievementProjectId > -1)
                {
                    BindingSource bindingSource = new BindingSource();

                    List<AchievementProjectDTO> achievementProjectDTOList = (List<AchievementProjectDTO>)drpProjects.DataSource;
                    if (achievementProjectDTOList != null && achievementProjectDTOList.Any())
                    {
                        AchievementProjectDTO achievementProjectDTO = (AchievementProjectDTO)achievementProjectDTOList.First(ws => ws.AchievementProjectId == achievementProjectId);
                        if (achievementProjectDTO != null)
                        {
                            CheckForUnSavedChanges(achievementProjectDTOList);
                            using (PublishUI publishUI = new PublishUI(utilities, achievementProjectDTO.AchievementProjectId, "Achievement", achievementProjectDTO.ProjectName))
                            {
                                publishUI.ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Waiver Set Publish"));
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void CheckForUnSavedChanges(List<AchievementProjectDTO> achievementProjectDTOList)
        {
            log.LogMethodEntry(achievementProjectDTOList);
            if (achievementProjectDTOList.Any(ws => ws.IsChanged == true))
            {
                throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 665));
            }
            log.LogMethodExit();
        }
    }
}
