namespace Semnox.Parafait.Customer
{
    partial class TagSerialMappingListUI
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
            this.lblFileName = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.dgvTagSerialMappingDTOList = new System.Windows.Forms.DataGridView();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblXLRecordCount = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnFileFormat = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.serialNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagSerialMappingDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagSerialMappingDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagSerialMappingDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFileName.Location = new System.Drawing.Point(99, 32);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(405, 22);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "FileName!";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChooseFile.FlatAppearance.BorderSize = 0;
            this.btnChooseFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChooseFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChooseFile.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnChooseFile.ForeColor = System.Drawing.Color.Black;
            this.btnChooseFile.Location = new System.Drawing.Point(517, 30);
            this.btnChooseFile.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(117, 25);
            this.btnChooseFile.TabIndex = 1;
            this.btnChooseFile.Text = "Choose File";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // dgvTagSerialMappingDTOList
            // 
            this.dgvTagSerialMappingDTOList.AllowUserToAddRows = false;
            this.dgvTagSerialMappingDTOList.AllowUserToDeleteRows = false;
            this.dgvTagSerialMappingDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvTagSerialMappingDTOList.AutoGenerateColumns = false;
            this.dgvTagSerialMappingDTOList.BackgroundColor = System.Drawing.Color.White;
            this.dgvTagSerialMappingDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTagSerialMappingDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.serialNumberDataGridViewTextBoxColumn,
            this.tagNumberDataGridViewTextBoxColumn,
            this.Status,
            this.Message});
            this.dgvTagSerialMappingDTOList.DataSource = this.tagSerialMappingDTOListBS;
            this.dgvTagSerialMappingDTOList.Location = new System.Drawing.Point(23, 83);
            this.dgvTagSerialMappingDTOList.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dgvTagSerialMappingDTOList.Name = "dgvTagSerialMappingDTOList";
            this.dgvTagSerialMappingDTOList.ReadOnly = true;
            this.dgvTagSerialMappingDTOList.Size = new System.Drawing.Size(611, 326);
            this.dgvTagSerialMappingDTOList.TabIndex = 2;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Message
            // 
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            // 
            // lblXLRecordCount
            // 
            this.lblXLRecordCount.AutoSize = true;
            this.lblXLRecordCount.Location = new System.Drawing.Point(21, 66);
            this.lblXLRecordCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblXLRecordCount.Name = "lblXLRecordCount";
            this.lblXLRecordCount.Size = new System.Drawing.Size(33, 15);
            this.lblXLRecordCount.TabIndex = 3;
            this.lblXLRecordCount.Text = "Data";
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnUpload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnUpload.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpload.ForeColor = System.Drawing.Color.Black;
            this.btnUpload.Location = new System.Drawing.Point(23, 421);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(117, 25);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(188, 421);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(117, 25);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(517, 421);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 25);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnFileFormat
            // 
            this.btnFileFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFileFormat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFileFormat.FlatAppearance.BorderSize = 0;
            this.btnFileFormat.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFileFormat.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFileFormat.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnFileFormat.ForeColor = System.Drawing.Color.Black;
            this.btnFileFormat.Location = new System.Drawing.Point(352, 421);
            this.btnFileFormat.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnFileFormat.Name = "btnFileFormat";
            this.btnFileFormat.Size = new System.Drawing.Size(117, 25);
            this.btnFileFormat.TabIndex = 7;
            this.btnFileFormat.Text = "File Format";
            this.btnFileFormat.UseVisualStyleBackColor = true;
            this.btnFileFormat.Click += new System.EventHandler(this.btnFileFormat_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Upload File:";
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Location = new System.Drawing.Point(0, 457);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(659, 16);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "Message";
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.White;
            this.progressBar.Location = new System.Drawing.Point(244, 66);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(365, 15);
            this.progressBar.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(180, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "Progress:";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.cancel;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(615, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(18, 19);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // serialNumberDataGridViewTextBoxColumn
            // 
            this.serialNumberDataGridViewTextBoxColumn.DataPropertyName = "SerialNumber";
            this.serialNumberDataGridViewTextBoxColumn.HeaderText = "Serial Number";
            this.serialNumberDataGridViewTextBoxColumn.Name = "serialNumberDataGridViewTextBoxColumn";
            this.serialNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tagNumberDataGridViewTextBoxColumn
            // 
            this.tagNumberDataGridViewTextBoxColumn.DataPropertyName = "TagNumber";
            this.tagNumberDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.tagNumberDataGridViewTextBoxColumn.Name = "tagNumberDataGridViewTextBoxColumn";
            this.tagNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tagSerialMappingDTOListBS
            // 
            this.tagSerialMappingDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.TagSerialMappingDTO);
            // 
            // TagSerialMappingListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 473);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFileFormat);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.lblXLRecordCount);
            this.Controls.Add(this.dgvTagSerialMappingDTOList);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.lblFileName);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TagSerialMappingListUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bulk Upload Cards";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBulkUploadCards_FormClosing);
            this.Load += new System.EventHandler(this.frmBulkUploadCards_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagSerialMappingDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagSerialMappingDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.DataGridView dgvTagSerialMappingDTOList;
        private System.Windows.Forms.Label lblXLRecordCount;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnFileFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource tagSerialMappingDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.Button btnCancel;
    }
}