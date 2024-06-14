namespace Semnox.Parafait.JobUtils
{
    partial class ConcurrentProgramArgumentsUI
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
            this.ConcurrentProgramArgumentsGridView = new System.Windows.Forms.DataGridView();
            this.argumentIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.argumentTypeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.argumentValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.concurrentProgramArgumentsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.GrpProgramArguments = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ChkBoxActive = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.GrpFilter = new System.Windows.Forms.GroupBox();
            this.lblProgramName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ConcurrentProgramArgumentsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramArgumentsDTOBindingSource)).BeginInit();
            this.GrpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConcurrentProgramArgumentsGridView
            // 
            this.ConcurrentProgramArgumentsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConcurrentProgramArgumentsGridView.AutoGenerateColumns = false;
            this.ConcurrentProgramArgumentsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConcurrentProgramArgumentsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.argumentIdDataGridViewTextBoxColumn,
            this.argumentTypeDataGridViewComboBoxColumn,
            this.argumentValueDataGridViewTextBoxColumn,
            this.IsActive});
            this.ConcurrentProgramArgumentsGridView.DataSource = this.concurrentProgramArgumentsDTOBindingSource;
            this.ConcurrentProgramArgumentsGridView.Location = new System.Drawing.Point(26, 75);
            this.ConcurrentProgramArgumentsGridView.Name = "ConcurrentProgramArgumentsGridView";
            this.ConcurrentProgramArgumentsGridView.Size = new System.Drawing.Size(464, 187);
            this.ConcurrentProgramArgumentsGridView.TabIndex = 21;
            // 
            // argumentIdDataGridViewTextBoxColumn
            // 
            this.argumentIdDataGridViewTextBoxColumn.DataPropertyName = "ArgumentId";
            this.argumentIdDataGridViewTextBoxColumn.HeaderText = "Argument Id";
            this.argumentIdDataGridViewTextBoxColumn.Name = "argumentIdDataGridViewTextBoxColumn";
            this.argumentIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // argumentTypeDataGridViewComboBoxColumn
            // 
            this.argumentTypeDataGridViewComboBoxColumn.DataPropertyName = "ArgumentType";
            this.argumentTypeDataGridViewComboBoxColumn.HeaderText = "Argument Type";
            this.argumentTypeDataGridViewComboBoxColumn.Name = "argumentTypeDataGridViewComboBoxColumn";
            this.argumentTypeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.argumentTypeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // argumentValueDataGridViewTextBoxColumn
            // 
            this.argumentValueDataGridViewTextBoxColumn.DataPropertyName = "ArgumentValue";
            this.argumentValueDataGridViewTextBoxColumn.HeaderText = "Argument Value";
            this.argumentValueDataGridViewTextBoxColumn.Name = "argumentValueDataGridViewTextBoxColumn";
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "IsActive";
            this.IsActive.Name = "IsActive";
            // 
            // concurrentProgramArgumentsDTOBindingSource
            // 
            this.concurrentProgramArgumentsDTOBindingSource.DataSource = typeof(ConcurrentProgramArgumentsDTO);
            // 
            // GrpProgramArguments
            // 
            this.GrpProgramArguments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrpProgramArguments.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.GrpProgramArguments.Location = new System.Drawing.Point(13, 53);
            this.GrpProgramArguments.Name = "GrpProgramArguments";
            this.GrpProgramArguments.Size = new System.Drawing.Size(499, 225);
            this.GrpProgramArguments.TabIndex = 22;
            this.GrpProgramArguments.TabStop = false;
            this.GrpProgramArguments.Text = "Program Arguments";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(25, 285);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(266, 285);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ChkBoxActive
            // 
            this.ChkBoxActive.AutoSize = true;
            this.ChkBoxActive.Checked = true;
            this.ChkBoxActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkBoxActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.ChkBoxActive.Location = new System.Drawing.Point(11, 15);
            this.ChkBoxActive.Name = "ChkBoxActive";
            this.ChkBoxActive.Size = new System.Drawing.Size(147, 19);
            this.ChkBoxActive.TabIndex = 25;
            this.ChkBoxActive.Text = "Show Active Records";
            this.ChkBoxActive.UseVisualStyleBackColor = true;
            this.ChkBoxActive.CheckedChanged += new System.EventHandler(this.ChkBoxActive_CheckedChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(145, 285);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 26;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // GrpFilter
            // 
            this.GrpFilter.Controls.Add(this.lblProgramName);
            this.GrpFilter.Controls.Add(this.ChkBoxActive);
            this.GrpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.GrpFilter.Location = new System.Drawing.Point(13, 8);
            this.GrpFilter.Name = "GrpFilter";
            this.GrpFilter.Size = new System.Drawing.Size(499, 41);
            this.GrpFilter.TabIndex = 27;
            this.GrpFilter.TabStop = false;
            this.GrpFilter.Text = "Filter";
            // 
            // lblProgramName
            // 
            this.lblProgramName.AutoSize = true;
            this.lblProgramName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblProgramName.Location = new System.Drawing.Point(159, 16);
            this.lblProgramName.Name = "lblProgramName";
            this.lblProgramName.Size = new System.Drawing.Size(99, 15);
            this.lblProgramName.TabIndex = 27;
            this.lblProgramName.Text = "Program Name :";
            // 
            // ConcurrentProgramArgumentsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 320);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ConcurrentProgramArgumentsGridView);
            this.Controls.Add(this.GrpProgramArguments);
            this.Controls.Add(this.GrpFilter);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConcurrentProgramArgumentsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Program Arguments";
            ((System.ComponentModel.ISupportInitialize)(this.ConcurrentProgramArgumentsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramArgumentsDTOBindingSource)).EndInit();
            this.GrpFilter.ResumeLayout(false);
            this.GrpFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ConcurrentProgramArgumentsGridView;
        private System.Windows.Forms.BindingSource concurrentProgramArgumentsDTOBindingSource;
        private System.Windows.Forms.GroupBox GrpProgramArguments;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox ChkBoxActive;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn argumentIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn argumentTypeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn argumentValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.GroupBox GrpFilter;
        private System.Windows.Forms.Label lblProgramName;
    }
}