namespace Semnox.Parafait.Achievements
{
    partial class AchievementProjectsUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvAchievementProject = new System.Windows.Forms.DataGridView();
            this.AchievementProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projectNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.achievementProjectDTOBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.achievementProjectDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnAchievement = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementProjectDTOBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementProjectDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(222, 346);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(325, 346);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(119, 346);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(16, 346);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvAchievementProject
            // 
            this.dgvAchievementProject.AllowUserToDeleteRows = false;
            this.dgvAchievementProject.AllowUserToOrderColumns = true;
            this.dgvAchievementProject.AutoGenerateColumns = false;
            this.dgvAchievementProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAchievementProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AchievementProjectId,
            this.projectNameDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvAchievementProject.DataSource = this.achievementProjectDTOBindingSource1;
            this.dgvAchievementProject.Location = new System.Drawing.Point(12, 12);
            this.dgvAchievementProject.Name = "dgvAchievementProject";
            this.dgvAchievementProject.Size = new System.Drawing.Size(601, 302);
            this.dgvAchievementProject.TabIndex = 26;
            // 
            // AchievementProjectId
            // 
            this.AchievementProjectId.DataPropertyName = "AchievementProjectId";
            this.AchievementProjectId.HeaderText = "Project Id";
            this.AchievementProjectId.Name = "AchievementProjectId";
            this.AchievementProjectId.ReadOnly = true;
            // 
            // projectNameDataGridViewTextBoxColumn
            // 
            this.projectNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.projectNameDataGridViewTextBoxColumn.DataPropertyName = "ProjectName";
            this.projectNameDataGridViewTextBoxColumn.HeaderText = "Project Name";
            this.projectNameDataGridViewTextBoxColumn.Name = "projectNameDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // achievementProjectDTOBindingSource1
            // 
            this.achievementProjectDTOBindingSource1.DataSource = typeof(Semnox.Parafait.Achievements.AchievementProjectDTO);
            // 
            // achievementProjectDTOBindingSource
            // 
            this.achievementProjectDTOBindingSource.DataSource = typeof(Semnox.Parafait.Achievements.AchievementProjectDTO);
            // 
            // btnAchievement
            // 
            this.btnAchievement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAchievement.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAchievement.Location = new System.Drawing.Point(422, 346);
            this.btnAchievement.Name = "btnAchievement";
            this.btnAchievement.Size = new System.Drawing.Size(152, 23);
            this.btnAchievement.TabIndex = 27;
            this.btnAchievement.Text = "Achievement Class";
            this.btnAchievement.UseVisualStyleBackColor = true;
            this.btnAchievement.Click += new System.EventHandler(this.btnAchievement_Click);
            // 
            // AchievementProjectsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 381);
            this.Controls.Add(this.btnAchievement);
            this.Controls.Add(this.dgvAchievementProject);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Name = "AchievementProjectsUI";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Achievement Projects";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementProjectDTOBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementProjectDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource achievementProjectDTOBindingSource;
        private System.Windows.Forms.DataGridView dgvAchievementProject;
        private System.Windows.Forms.BindingSource achievementProjectDTOBindingSource1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnAchievement;
        private System.Windows.Forms.DataGridViewTextBoxColumn AchievementProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}