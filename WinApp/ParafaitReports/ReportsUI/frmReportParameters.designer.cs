namespace Semnox.Parafait.Report.Reports
{
    partial class frmReportParameters
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
            this.dgvReportParameters = new System.Windows.Forms.DataGridView();
            this.reportParametersDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.ParameterId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcReportId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parameterNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sqlParameterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataSourceTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataSourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operatorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.activeFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DisplayOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mandatory = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dcLastUpdatedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcLastUpdatedUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportParameters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportParametersDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReportParameters
            // 
            this.dgvReportParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReportParameters.AutoGenerateColumns = false;
            this.dgvReportParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParameterId,
            this.dcReportId,
            this.parameterNameDataGridViewTextBoxColumn,
            this.sqlParameterDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn1,
            this.dataTypeDataGridViewTextBoxColumn,
            this.dataSourceTypeDataGridViewTextBoxColumn,
            this.dataSourceDataGridViewTextBoxColumn,
            this.operatorDataGridViewTextBoxColumn,
            this.activeFlagDataGridViewCheckBoxColumn,
            this.DisplayOrder,
            this.Mandatory,
            this.dcLastUpdatedDate,
            this.dcLastUpdatedUser,
            this.dataGridViewTextBoxColumn3});
            this.dgvReportParameters.DataSource = this.reportParametersDTOBindingSource;
            this.dgvReportParameters.Location = new System.Drawing.Point(13, 13);
            this.dgvReportParameters.Name = "dgvReportParameters";
            this.dgvReportParameters.Size = new System.Drawing.Size(983, 421);
            this.dgvReportParameters.TabIndex = 0;
            this.dgvReportParameters.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvReportParameters_DataError);
            this.dgvReportParameters.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvReportParameters_DefaultValuesNeeded);
            // 
            // reportParametersDTOBindingSource
            // 
            this.reportParametersDTOBindingSource.DataSource = typeof(Semnox.Parafait.Reports.ReportParametersDTO);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(412, 442);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 23);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPreview.Location = new System.Drawing.Point(279, 442);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(90, 23);
            this.btnPreview.TabIndex = 14;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(545, 442);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(13, 442);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(146, 442);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ParameterId
            // 
            this.ParameterId.DataPropertyName = "ParameterId";
            this.ParameterId.HeaderText = "Parameter Id";
            this.ParameterId.Name = "ParameterId";
            this.ParameterId.ReadOnly = true;
            // 
            // dcReportId
            // 
            this.dcReportId.DataPropertyName = "ReportId";
            this.dcReportId.HeaderText = "Report Id";
            this.dcReportId.Name = "dcReportId";
            this.dcReportId.Visible = false;
            // 
            // parameterNameDataGridViewTextBoxColumn
            // 
            this.parameterNameDataGridViewTextBoxColumn.DataPropertyName = "ParameterName";
            this.parameterNameDataGridViewTextBoxColumn.HeaderText = "ParameterName";
            this.parameterNameDataGridViewTextBoxColumn.Name = "parameterNameDataGridViewTextBoxColumn";
            // 
            // sqlParameterDataGridViewTextBoxColumn
            // 
            this.sqlParameterDataGridViewTextBoxColumn.DataPropertyName = "SqlParameter";
            this.sqlParameterDataGridViewTextBoxColumn.HeaderText = "SQL Parameter";
            this.sqlParameterDataGridViewTextBoxColumn.Name = "sqlParameterDataGridViewTextBoxColumn";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn1.HeaderText = "Description";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataTypeDataGridViewTextBoxColumn
            // 
            this.dataTypeDataGridViewTextBoxColumn.DataPropertyName = "DataType";
            this.dataTypeDataGridViewTextBoxColumn.HeaderText = "Data Type";
            this.dataTypeDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "TEXT",
            "NUMBER",
            "DATETIME"});
            this.dataTypeDataGridViewTextBoxColumn.Name = "dataTypeDataGridViewTextBoxColumn";
            this.dataTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataSourceTypeDataGridViewTextBoxColumn
            // 
            this.dataSourceTypeDataGridViewTextBoxColumn.DataPropertyName = "DataSourceType";
            this.dataSourceTypeDataGridViewTextBoxColumn.HeaderText = "Data Source Type";
            this.dataSourceTypeDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "CONSTANT",
            "STATIC_LIST",
            "QUERY"});
            this.dataSourceTypeDataGridViewTextBoxColumn.Name = "dataSourceTypeDataGridViewTextBoxColumn";
            this.dataSourceTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataSourceTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataSourceDataGridViewTextBoxColumn
            // 
            this.dataSourceDataGridViewTextBoxColumn.DataPropertyName = "DataSource";
            this.dataSourceDataGridViewTextBoxColumn.HeaderText = "DataSource";
            this.dataSourceDataGridViewTextBoxColumn.Name = "dataSourceDataGridViewTextBoxColumn";
            // 
            // operatorDataGridViewTextBoxColumn
            // 
            this.operatorDataGridViewTextBoxColumn.DataPropertyName = "Operator";
            this.operatorDataGridViewTextBoxColumn.HeaderText = "Operater";
            this.operatorDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "Default",
            "INLIST"});
            this.operatorDataGridViewTextBoxColumn.Name = "operatorDataGridViewTextBoxColumn";
            this.operatorDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.operatorDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // activeFlagDataGridViewCheckBoxColumn
            // 
            this.activeFlagDataGridViewCheckBoxColumn.DataPropertyName = "ActiveFlag";
            this.activeFlagDataGridViewCheckBoxColumn.FalseValue = "false";
            this.activeFlagDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeFlagDataGridViewCheckBoxColumn.Name = "activeFlagDataGridViewCheckBoxColumn";
            this.activeFlagDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // DisplayOrder
            // 
            this.DisplayOrder.DataPropertyName = "DisplayOrder";
            this.DisplayOrder.HeaderText = "Display Order";
            this.DisplayOrder.Name = "DisplayOrder";
            // 
            // Mandatory
            // 
            this.Mandatory.DataPropertyName = "Mandatory";
            this.Mandatory.FalseValue = "false";
            this.Mandatory.HeaderText = "Mandatory?";
            this.Mandatory.Name = "Mandatory";
            this.Mandatory.TrueValue = "true";
            // 
            // dcLastUpdatedDate
            // 
            this.dcLastUpdatedDate.DataPropertyName = "LastUpdatedDate";
            this.dcLastUpdatedDate.HeaderText = "LastUpdatedDate";
            this.dcLastUpdatedDate.Name = "dcLastUpdatedDate";
            // 
            // dcLastUpdatedUser
            // 
            this.dcLastUpdatedUser.DataPropertyName = "LastUpdatedUser";
            this.dcLastUpdatedUser.HeaderText = "LastUpdatedUser";
            this.dcLastUpdatedUser.Name = "dcLastUpdatedUser";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "MasterEntityId";
            this.dataGridViewTextBoxColumn3.HeaderText = "Master Entity Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // frmReportParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 475);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvReportParameters);
            this.Name = "frmReportParameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReportParameters_FormClosing);
            this.Load += new System.EventHandler(this.frmReportParameters_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportParameters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportParametersDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.DataGridView dgvReportParameters;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;

       
        private System.Windows.Forms.BindingSource reportParametersBindingSource;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcParameterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSQLParameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dcDataType;
        private System.Windows.Forms.DataGridViewComboBoxColumn dcDataSourceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcDataSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcLastUpdatedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcLastUpdatedUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource reportParametersDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcReportId;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sqlParameterDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataSourceTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn operatorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayOrder;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Mandatory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}