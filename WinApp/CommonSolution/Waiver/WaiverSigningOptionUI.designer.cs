using System.Drawing;

namespace Semnox.Parafait.Waiver
{
    partial class WaiverSigningOptionUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvWaiverSigningOption = new System.Windows.Forms.DataGridView();
            this.chkWaiverSignedEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lookupValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LookupValueId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lookupValuesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverSigningOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupValuesDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvWaiverSigningOption
            // 
            this.dgvWaiverSigningOption.AllowUserToAddRows = false;
            this.dgvWaiverSigningOption.AllowUserToDeleteRows = false;
            this.dgvWaiverSigningOption.AllowUserToOrderColumns = true;
            this.dgvWaiverSigningOption.AllowUserToResizeColumns = false;
            this.dgvWaiverSigningOption.AllowUserToResizeRows = false;
            this.dgvWaiverSigningOption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWaiverSigningOption.AutoGenerateColumns = false;
            this.dgvWaiverSigningOption.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWaiverSigningOption.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvWaiverSigningOption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWaiverSigningOption.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.lookupValueDataGridViewTextBoxColumn,
            this.LookupValueId,
            this.descriptionDataGridViewTextBoxColumn,
            this.chkWaiverSignedEnable});
            this.dgvWaiverSigningOption.DataSource = this.lookupValuesDTOBindingSource;
            this.dgvWaiverSigningOption.Location = new System.Drawing.Point(22, 29);
            this.dgvWaiverSigningOption.MultiSelect = false;
            this.dgvWaiverSigningOption.Name = "dgvWaiverSigningOption";
            this.dgvWaiverSigningOption.Size = new System.Drawing.Size(482, 165);
            this.dgvWaiverSigningOption.TabIndex = 0;
            this.dgvWaiverSigningOption.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvWaiverSigningOption_DataBindingComplete);
            // 
            // chkWaiverSignedEnable
            // 
            this.chkWaiverSignedEnable.HeaderText = "Enable";
            this.chkWaiverSignedEnable.Name = "chkWaiverSignedEnable";
            this.chkWaiverSignedEnable.Width = 83;
            // 
            // lookupValueDataGridViewTextBoxColumn
            // 
            this.lookupValueDataGridViewTextBoxColumn.DataPropertyName = "LookupValue";
            this.lookupValueDataGridViewTextBoxColumn.HeaderText = "Lookup Value";
            this.lookupValueDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.lookupValueDataGridViewTextBoxColumn.Name = "lookupValueDataGridViewTextBoxColumn";
            this.lookupValueDataGridViewTextBoxColumn.ReadOnly = true;
            this.lookupValueDataGridViewTextBoxColumn.Width = 200;
            // 
            // LookupValueId
            // 
            this.LookupValueId.DataPropertyName = "LookupValueId";
            this.LookupValueId.HeaderText = "LookupValueId";
            this.LookupValueId.Name = "LookupValueId";
            this.LookupValueId.ReadOnly = true;
            this.LookupValueId.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Width = 250;
            // 
            // lookupValuesDTOBindingSource
            // 
            this.lookupValuesDTOBindingSource.DataSource = typeof(Semnox.Core.Utilities.LookupValuesDTO);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(79, 207);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(201, 207);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(323, 207);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // WaiverSigningOptionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 264);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvWaiverSigningOption);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaiverSigningOptionUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Waiver Signing Option";
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverSigningOption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupValuesDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvWaiverSigningOption;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource lookupValuesDTOBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkWaiverSignedEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn lookupValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LookupValueId;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
    }
}