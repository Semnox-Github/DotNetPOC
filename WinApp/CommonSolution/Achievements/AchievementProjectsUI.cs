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

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// User interface of Achievement Projects UI
    /// </summary>
    public partial class AchievementProjectsUI : Form
    {

        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Constructor used during object creation.
        /// </summary>
        public AchievementProjectsUI(Utilities _Utilities)
        {
            log.Debug("Starts-AchievementProjectUI(Utilities) parameterized constructor.");
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvAchievementProject);

            machineUserContext.SetSiteId(utilities.ParafaitEnv.IsCorporate == true ? utilities.ParafaitEnv.SiteId : -1);
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);

            utilities.setLanguage(this);
            try
            {
                LoadAchievementProjects();
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }

            log.Debug("Ends-AchievementProjectUI(Utilities) parameterized constructor.");
        }


        private void LoadAchievementProjects()
        {
            AchievementProjectsList achievementProjectsList = new AchievementProjectsList(machineUserContext);

            AchievementProjectParams achievementProjectParams = new AchievementProjectParams();
            achievementProjectParams.IsActive = false;
            achievementProjectParams.SiteId = machineUserContext.GetSiteId();
            List<AchievementProjectDTO> achievementProjectDTOOnDisplay = achievementProjectsList.GetAchievementProjectsList(achievementProjectParams);

            SortableBindingList<AchievementProjectDTO> achievementProjectDTOList = new SortableBindingList<AchievementProjectDTO>(achievementProjectDTOOnDisplay);

            if (achievementProjectDTOList == null)
            {
                achievementProjectDTOList = new SortableBindingList<AchievementProjectDTO>();
            }

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = achievementProjectDTOList;
            dgvAchievementProject.DataSource = bindingSource;
        }




        private bool isProjectNameAlreadyExists()
        {
            log.Debug("Starts-isProjectNameAlreadyExists() event.");
            bool status = false;
            try
            {


                BindingSource achievementProjectListBS = new BindingSource();

                achievementProjectListBS = (BindingSource)dgvAchievementProject.DataSource;
                var achievementProjectListOnDisplay = (SortableBindingList<AchievementProjectDTO>)achievementProjectListBS.DataSource;

                if (achievementProjectListOnDisplay.Count > 0)
                {
                    AchievementProjectDTO currentObject = (AchievementProjectDTO)dgvAchievementProject.CurrentRow.DataBoundItem;
                    foreach (AchievementProjectDTO achievementProjectDTO in achievementProjectListOnDisplay)
                    {
                        if (!(string.IsNullOrEmpty(currentObject.ProjectName)) && !(string.IsNullOrEmpty(achievementProjectDTO.ProjectName)))
                        {
                            if ((currentObject.ProjectName.CompareTo(achievementProjectDTO.ProjectName) == 0) && (achievementProjectDTO.AchievementProjectId != currentObject.AchievementProjectId))
                            {
                                status = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.Debug("Ends-isProjectNameAlreadyExists() event.");
            return status;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() event.");
            try
            {
                if (isProjectNameAlreadyExists())
                {
                    MessageBox.Show("Project Name Already Exist");
                    log.Debug("Ends-btnSave_Click() event by returning name already exists message popup.");
                    return;
                }
                else
                {
                    AchievementProject achievementProjects;
                    BindingSource achievementProjectListBS = (BindingSource)dgvAchievementProject.DataSource;
                    var achievementProjectListOnDisplay = (SortableBindingList<AchievementProjectDTO>)achievementProjectListBS.DataSource;
                    foreach (AchievementProjectDTO achievementProjectDTO in achievementProjectListOnDisplay)
                    {
                        if (string.IsNullOrEmpty(achievementProjectDTO.ProjectName.Trim()))
                        {
                            MessageBox.Show("Enter Project Name");
                            return;
                        }

                        achievementProjects = new AchievementProject(machineUserContext, achievementProjectDTO);
                        achievementProjects.Save();
                    }
                    LoadAchievementProjects();

                    log.Debug("Ends-btnSave_Click() event.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("Starts-btnDelete_Click() event.");
                if (this.dgvAchievementProject.SelectedRows.Count <= 0 && this.dgvAchievementProject.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvAchievementProject.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvAchievementProject.SelectedCells)
                    {
                        dgvAchievementProject.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow achievementProjectRow in this.dgvAchievementProject.SelectedRows)
                {
                    if (achievementProjectRow.Cells[0].Value == null)
                    {
                        return;
                    }
                    else if (Convert.ToInt32(achievementProjectRow.Cells[0].Value.ToString()) <= 0)
                    {
                        dgvAchievementProject.Rows.RemoveAt(achievementProjectRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm to delete.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource achievementProjectDTOListDTOBS = (BindingSource)dgvAchievementProject.DataSource;
                            var achievementProjectDTOList = (SortableBindingList<AchievementProjectDTO>)achievementProjectDTOListDTOBS.DataSource;
                            AchievementProjectDTO achievementProjectDTO = achievementProjectDTOList[achievementProjectRow.Index];
                            achievementProjectDTO.IsActive = false;
                            AchievementProject achievementProjects = new AchievementProject(machineUserContext, achievementProjectDTO);
                            achievementProjects.Save();
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                LoadAchievementProjects();
                log.Debug("Ends-btnDelete_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() event.");
            this.Close();
            log.Debug("Ends-btnClose_Click() event.");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefresh_Click() event.");
            try
            {
                LoadAchievementProjects();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.Debug("Ends-btnRefresh_Click() event.");
        }

        private void btnAchievement_Click(object sender, EventArgs e)
        {
            try
            {
                int projectId = -1;
                if (dgvAchievementProject.CurrentRow != null && dgvAchievementProject.CurrentRow.Cells["AchievementProjectId"].Value != null)
                {
                    int.TryParse(dgvAchievementProject.CurrentRow.Cells["AchievementProjectId"].Value.ToString(), out projectId);

                    AchievementClassUI achievementClassUI = Application.OpenForms.OfType<AchievementClassUI>().FirstOrDefault();
                    if (achievementClassUI != null)
                    {
                        achievementClassUI.Close();
                    }
               
                    //achievementClassUI = new AchievementClassUI(utilities, projectId);
                    //achievementClassUI.Show();
                    achievementClassUI = new AchievementClassUI(utilities, projectId);
                    achievementClassUI.MdiParent = this.ParentForm;
                    if (!achievementClassUI.Visible)
                        achievementClassUI.Show();

                
                  
                }
                else
                {
                    MessageBox.Show("No data");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }
    }
}
