namespace Semnox.Parafait.Customer
{
    partial class CustomerFeedbackSurveyDetailsUI
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
            this.cmbCriteria = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSurvey = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSurvey = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvSurveyDetails = new System.Windows.Forms.DataGridView();
            this.custFbSurveyDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custFbSurveyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.custFbQuestionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.nextQuestionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.criteriaIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.criteriaValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custFbResponseIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ExpectedCustFbResponseValueId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpectedCustFbResponseValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isResponseMandatoryDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isRecurDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customerFeedbackSurveyDetailsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbDataColumn = new System.Windows.Forms.ComboBox();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSurveyDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerFeedbackSurveyDetailsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.cmbCriteria);
            this.gpFilter.Controls.Add(this.label3);
            this.gpFilter.Controls.Add(this.cmbSurvey);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.lblSurvey);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(3, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(769, 55);
            this.gpFilter.TabIndex = 31;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // cmbCriteria
            // 
            this.cmbCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCriteria.FormattingEnabled = true;
            this.cmbCriteria.Location = new System.Drawing.Point(502, 21);
            this.cmbCriteria.Name = "cmbCriteria";
            this.cmbCriteria.Size = new System.Drawing.Size(121, 23);
            this.cmbCriteria.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(400, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Criteria";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSurvey
            // 
            this.cmbSurvey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSurvey.FormattingEnabled = true;
            this.cmbSurvey.Location = new System.Drawing.Point(266, 21);
            this.cmbSurvey.Name = "cmbSurvey";
            this.cmbSurvey.Size = new System.Drawing.Size(121, 23);
            this.cmbSurvey.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(657, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblSurvey
            // 
            this.lblSurvey.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSurvey.Location = new System.Drawing.Point(161, 22);
            this.lblSurvey.Name = "lblSurvey";
            this.lblSurvey.Size = new System.Drawing.Size(96, 20);
            this.lblSurvey.TabIndex = 1;
            this.lblSurvey.Text = "Survey";
            this.lblSurvey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(10, 23);
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
            this.btnDelete.Location = new System.Drawing.Point(269, 367);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 30;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(397, 367);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(141, 367);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 28;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 367);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 27;
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
            this.custFbSurveyDetailIdDataGridViewTextBoxColumn,
            this.custFbSurveyIdDataGridViewTextBoxColumn,
            this.custFbQuestionIdDataGridViewTextBoxColumn,
            this.nextQuestionIdDataGridViewTextBoxColumn,
            this.criteriaIdDataGridViewTextBoxColumn,
            this.criteriaValueDataGridViewTextBoxColumn,
            this.custFbResponseIdDataGridViewTextBoxColumn,
            this.ExpectedCustFbResponseValueId,
            this.ExpectedCustFbResponseValue,
            this.isResponseMandatoryDataGridViewCheckBoxColumn,
            this.isRecurDataGridViewCheckBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvSurveyDetails.DataSource = this.customerFeedbackSurveyDetailsDTOBindingSource;
            this.dgvSurveyDetails.Location = new System.Drawing.Point(3, 61);
            this.dgvSurveyDetails.Name = "dgvSurveyDetails";
            this.dgvSurveyDetails.Size = new System.Drawing.Size(769, 286);
            this.dgvSurveyDetails.TabIndex = 26;
            this.dgvSurveyDetails.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSurveyDetails_CellEnter);
            // 
            // custFbSurveyDetailIdDataGridViewTextBoxColumn
            // 
            this.custFbSurveyDetailIdDataGridViewTextBoxColumn.DataPropertyName = "CustFbSurveyDetailId";
            this.custFbSurveyDetailIdDataGridViewTextBoxColumn.HeaderText = "Survey Detail Id";
            this.custFbSurveyDetailIdDataGridViewTextBoxColumn.Name = "custFbSurveyDetailIdDataGridViewTextBoxColumn";
            this.custFbSurveyDetailIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // custFbSurveyIdDataGridViewTextBoxColumn
            // 
            this.custFbSurveyIdDataGridViewTextBoxColumn.DataPropertyName = "CustFbSurveyId";
            this.custFbSurveyIdDataGridViewTextBoxColumn.HeaderText = "Survey";
            this.custFbSurveyIdDataGridViewTextBoxColumn.Name = "custFbSurveyIdDataGridViewTextBoxColumn";
            this.custFbSurveyIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.custFbSurveyIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // custFbQuestionIdDataGridViewTextBoxColumn
            // 
            this.custFbQuestionIdDataGridViewTextBoxColumn.DataPropertyName = "CustFbQuestionId";
            this.custFbQuestionIdDataGridViewTextBoxColumn.HeaderText = "Question";
            this.custFbQuestionIdDataGridViewTextBoxColumn.Name = "custFbQuestionIdDataGridViewTextBoxColumn";
            this.custFbQuestionIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.custFbQuestionIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // nextQuestionIdDataGridViewTextBoxColumn
            // 
            this.nextQuestionIdDataGridViewTextBoxColumn.DataPropertyName = "NextQuestionId";
            this.nextQuestionIdDataGridViewTextBoxColumn.HeaderText = "Next Question";
            this.nextQuestionIdDataGridViewTextBoxColumn.Name = "nextQuestionIdDataGridViewTextBoxColumn";
            this.nextQuestionIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nextQuestionIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // criteriaIdDataGridViewTextBoxColumn
            // 
            this.criteriaIdDataGridViewTextBoxColumn.DataPropertyName = "CriteriaId";
            this.criteriaIdDataGridViewTextBoxColumn.HeaderText = "Criteria";
            this.criteriaIdDataGridViewTextBoxColumn.Name = "criteriaIdDataGridViewTextBoxColumn";
            this.criteriaIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.criteriaIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // criteriaValueDataGridViewTextBoxColumn
            // 
            this.criteriaValueDataGridViewTextBoxColumn.DataPropertyName = "CriteriaValue";
            this.criteriaValueDataGridViewTextBoxColumn.HeaderText = "Criteria Value";
            this.criteriaValueDataGridViewTextBoxColumn.Name = "criteriaValueDataGridViewTextBoxColumn";
            // 
            // custFbResponseIdDataGridViewTextBoxColumn
            // 
            this.custFbResponseIdDataGridViewTextBoxColumn.DataPropertyName = "CustFbResponseId";
            this.custFbResponseIdDataGridViewTextBoxColumn.HeaderText = "Response";
            this.custFbResponseIdDataGridViewTextBoxColumn.Name = "custFbResponseIdDataGridViewTextBoxColumn";
            this.custFbResponseIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.custFbResponseIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ExpectedCustFbResponseValueId
            // 
            this.ExpectedCustFbResponseValueId.DataPropertyName = "ExpectedCustFbResponseValueId";
            this.ExpectedCustFbResponseValueId.HeaderText = "ExpectedCustFbResponseValueId";
            this.ExpectedCustFbResponseValueId.Name = "ExpectedCustFbResponseValueId";
            this.ExpectedCustFbResponseValueId.Visible = false;
            // 
            // ExpectedCustFbResponseValue
            // 
            this.ExpectedCustFbResponseValue.DataPropertyName = "ExpectedCustFbResponseValue";
            this.ExpectedCustFbResponseValue.HeaderText = "Expected Response Value";
            this.ExpectedCustFbResponseValue.Name = "ExpectedCustFbResponseValue";
            this.ExpectedCustFbResponseValue.ReadOnly = true;
            // 
            // isResponseMandatoryDataGridViewCheckBoxColumn
            // 
            this.isResponseMandatoryDataGridViewCheckBoxColumn.DataPropertyName = "IsResponseMandatory";
            this.isResponseMandatoryDataGridViewCheckBoxColumn.HeaderText = "IsResponseMandatory?";
            this.isResponseMandatoryDataGridViewCheckBoxColumn.Name = "isResponseMandatoryDataGridViewCheckBoxColumn";
            // 
            // isRecurDataGridViewCheckBoxColumn
            // 
            this.isRecurDataGridViewCheckBoxColumn.DataPropertyName = "IsRecur";
            this.isRecurDataGridViewCheckBoxColumn.HeaderText = "IsRecur?";
            this.isRecurDataGridViewCheckBoxColumn.Name = "isRecurDataGridViewCheckBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // customerFeedbackSurveyDetailsDTOBindingSource
            // 
            this.customerFeedbackSurveyDetailsDTOBindingSource.DataSource = typeof(CustomerFeedbackSurveyDetailsDTO);
            // 
            // cmbDataColumn
            // 
            this.cmbDataColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataColumn.FormattingEnabled = true;
            this.cmbDataColumn.Location = new System.Drawing.Point(327, 190);
            this.cmbDataColumn.Name = "cmbDataColumn";
            this.cmbDataColumn.Size = new System.Drawing.Size(121, 21);
            this.cmbDataColumn.TabIndex = 32;
            this.cmbDataColumn.Visible = false;
            this.cmbDataColumn.SelectedValueChanged += new System.EventHandler(this.cmbDataColumn_SelectedValueChanged);
            // 
            // CustomerFeedbackSurveyDetailsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 400);
            this.Controls.Add(this.cmbDataColumn);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvSurveyDetails);
            this.Name = "CustomerFeedbackSurveyDetailsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Feedback Survey Details";
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSurveyDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerFeedbackSurveyDetailsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblSurvey;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvSurveyDetails;
        private System.Windows.Forms.BindingSource customerFeedbackSurveyDetailsDTOBindingSource;
        private System.Windows.Forms.ComboBox cmbCriteria;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSurvey;
        private System.Windows.Forms.ComboBox cmbDataColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn custFbSurveyDetailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn custFbSurveyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn custFbQuestionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn nextQuestionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn criteriaIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn criteriaValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn custFbResponseIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpectedCustFbResponseValueId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpectedCustFbResponseValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isResponseMandatoryDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isRecurDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}