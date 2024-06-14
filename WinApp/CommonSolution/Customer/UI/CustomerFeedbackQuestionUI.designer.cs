namespace Semnox.Parafait.Customer
{
    partial class CustomerFeedbackQuestionUI
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
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvSurveyDetails = new System.Windows.Forms.DataGridView();
            this.custFbQuestionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.questionNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.questionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custFbResponseIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Translation = new System.Windows.Forms.DataGridViewButtonColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customerFeedbackQuestionsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSurveyDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerFeedbackQuestionsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(12, 10);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(743, 47);
            this.gpFilter.TabIndex = 19;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(435, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(293, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(157, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(130, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Question No :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(269, 280);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(397, 280);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(141, 280);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 280);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvSurveyDetails
            // 
            this.dgvSurveyDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSurveyDetails.AutoGenerateColumns = false;
            this.dgvSurveyDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSurveyDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.custFbQuestionIdDataGridViewTextBoxColumn,
            this.questionNoDataGridViewTextBoxColumn,
            this.questionDataGridViewTextBoxColumn,
            this.custFbResponseIdDataGridViewTextBoxColumn,
            this.Translation,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvSurveyDetails.DataSource = this.customerFeedbackQuestionsDTOBindingSource;
            this.dgvSurveyDetails.Location = new System.Drawing.Point(12, 63);
            this.dgvSurveyDetails.Name = "dgvSurveyDetails";
            this.dgvSurveyDetails.Size = new System.Drawing.Size(743, 201);
            this.dgvSurveyDetails.TabIndex = 14;
            this.dgvSurveyDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSurveyDetails_CellContentClick);
            // 
            // custFbQuestionIdDataGridViewTextBoxColumn
            // 
            this.custFbQuestionIdDataGridViewTextBoxColumn.DataPropertyName = "CustFbQuestionId";
            this.custFbQuestionIdDataGridViewTextBoxColumn.HeaderText = "Question Id";
            this.custFbQuestionIdDataGridViewTextBoxColumn.Name = "custFbQuestionIdDataGridViewTextBoxColumn";
            this.custFbQuestionIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // questionNoDataGridViewTextBoxColumn
            // 
            this.questionNoDataGridViewTextBoxColumn.DataPropertyName = "QuestionNo";
            this.questionNoDataGridViewTextBoxColumn.HeaderText = "Question No";
            this.questionNoDataGridViewTextBoxColumn.Name = "questionNoDataGridViewTextBoxColumn";
            // 
            // questionDataGridViewTextBoxColumn
            // 
            this.questionDataGridViewTextBoxColumn.DataPropertyName = "Question";
            this.questionDataGridViewTextBoxColumn.HeaderText = "Question";
            this.questionDataGridViewTextBoxColumn.Name = "questionDataGridViewTextBoxColumn";
            // 
            // custFbResponseIdDataGridViewTextBoxColumn
            // 
            this.custFbResponseIdDataGridViewTextBoxColumn.DataPropertyName = "CustFbResponseId";
            this.custFbResponseIdDataGridViewTextBoxColumn.HeaderText = "Response";
            this.custFbResponseIdDataGridViewTextBoxColumn.Name = "custFbResponseIdDataGridViewTextBoxColumn";
            this.custFbResponseIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.custFbResponseIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Translation
            // 
            this.Translation.HeaderText = "";
            this.Translation.Name = "Translation";
            this.Translation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Translation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Translation.Text = "T";
            this.Translation.UseColumnTextForButtonValue = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // customerFeedbackQuestionsDTOBindingSource
            // 
            this.customerFeedbackQuestionsDTOBindingSource.DataSource = typeof(CustomerFeedbackQuestionsDTO);
            // 
            // CustomerFeedbackQuestionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 313);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvSurveyDetails);
            this.Name = "CustomerFeedbackQuestionUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Feedback Questions";
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSurveyDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerFeedbackQuestionsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvSurveyDetails;
        private System.Windows.Forms.BindingSource customerFeedbackQuestionsDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn custFbQuestionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn questionNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn questionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn custFbResponseIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn Translation;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}