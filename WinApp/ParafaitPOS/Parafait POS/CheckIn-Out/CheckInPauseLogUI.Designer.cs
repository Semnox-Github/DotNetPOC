namespace Parafait_POS
{
    partial class CheckInPauseLogUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckInPauseLogUI));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpboxCheckInPauseLog = new System.Windows.Forms.GroupBox();
            this.lblTimeInMins = new System.Windows.Forms.Label();
            this.lblTotalPauseTime = new System.Windows.Forms.Label();
            this.verticalScrollBarView2 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvCheckInPauseLog = new System.Windows.Forms.DataGridView();
            this.checkInPauseLogIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkInDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pauseStartTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pauseEndTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalPauseTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pOSMachineDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pausedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unPausedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.checkInPauseLogDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.horizontalScrollBarView2 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpboxCheckInPauseLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckInPauseLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkInPauseLogDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxCheckInPauseLog
            // 
            this.grpboxCheckInPauseLog.Controls.Add(this.lblTimeInMins);
            this.grpboxCheckInPauseLog.Controls.Add(this.lblTotalPauseTime);
            this.grpboxCheckInPauseLog.Controls.Add(this.verticalScrollBarView2);
            this.grpboxCheckInPauseLog.Controls.Add(this.horizontalScrollBarView2);
            this.grpboxCheckInPauseLog.Controls.Add(this.btnClose);
            this.grpboxCheckInPauseLog.Controls.Add(this.dgvCheckInPauseLog);
            this.grpboxCheckInPauseLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpboxCheckInPauseLog.Location = new System.Drawing.Point(12, 12);
            this.grpboxCheckInPauseLog.Name = "grpboxCheckInPauseLog";
            this.grpboxCheckInPauseLog.Size = new System.Drawing.Size(612, 470);
            this.grpboxCheckInPauseLog.TabIndex = 0;
            this.grpboxCheckInPauseLog.TabStop = false;
            this.grpboxCheckInPauseLog.Text = "Check-In Pause Log";
            // 
            // lblTimeInMins
            // 
            this.lblTimeInMins.AutoSize = true;
            this.lblTimeInMins.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeInMins.Location = new System.Drawing.Point(143, 26);
            this.lblTimeInMins.Name = "lblTimeInMins";
            this.lblTimeInMins.Size = new System.Drawing.Size(54, 18);
            this.lblTimeInMins.TabIndex = 27;
            this.lblTimeInMins.Text = "0 mins";
            // 
            // lblTotalPauseTime
            // 
            this.lblTotalPauseTime.AutoSize = true;
            this.lblTotalPauseTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPauseTime.Location = new System.Drawing.Point(10, 26);
            this.lblTotalPauseTime.Name = "lblTotalPauseTime";
            this.lblTotalPauseTime.Size = new System.Drawing.Size(127, 18);
            this.lblTotalPauseTime.TabIndex = 26;
            this.lblTotalPauseTime.Text = "Total Pause Time";
            // 
            // verticalScrollBarView2
            // 
            this.verticalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView2.AutoHide = false;
            this.verticalScrollBarView2.DataGridView = this.dgvCheckInPauseLog;
            this.verticalScrollBarView2.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonBackgroundImage")));
            this.verticalScrollBarView2.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView2.Location = new System.Drawing.Point(559, 19);
            this.verticalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView2.Name = "verticalScrollBarView2";
            this.verticalScrollBarView2.ScrollableControl = null;
            this.verticalScrollBarView2.ScrollViewer = null;
            this.verticalScrollBarView2.Size = new System.Drawing.Size(49, 353);
            this.verticalScrollBarView2.TabIndex = 25;
            this.verticalScrollBarView2.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonBackgroundImage")));
            this.verticalScrollBarView2.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonDisabledBackgroundImage")));
            // 
            // dgvCheckInPauseLog
            // 
            this.dgvCheckInPauseLog.AllowUserToAddRows = false;
            this.dgvCheckInPauseLog.AllowUserToDeleteRows = false;
            this.dgvCheckInPauseLog.AutoGenerateColumns = false;
            this.dgvCheckInPauseLog.ColumnHeadersHeight = 30;
            this.dgvCheckInPauseLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkInPauseLogIdDataGridViewTextBoxColumn,
            this.checkInDetailIdDataGridViewTextBoxColumn,
            this.pauseStartTimeDataGridViewTextBoxColumn,
            this.pauseEndTimeDataGridViewTextBoxColumn,
            this.totalPauseTimeDataGridViewTextBoxColumn,
            this.pOSMachineDataGridViewTextBoxColumn,
            this.pausedByDataGridViewTextBoxColumn,
            this.unPausedByDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.siteIdDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn});
            this.dgvCheckInPauseLog.DataSource = this.checkInPauseLogDTOBindingSource;
            this.dgvCheckInPauseLog.Location = new System.Drawing.Point(6, 57);
            this.dgvCheckInPauseLog.Name = "dgvCheckInPauseLog";
            this.dgvCheckInPauseLog.ReadOnly = true;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCheckInPauseLog.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCheckInPauseLog.RowTemplate.Height = 30;
            this.dgvCheckInPauseLog.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCheckInPauseLog.Size = new System.Drawing.Size(550, 315);
            this.dgvCheckInPauseLog.TabIndex = 22;
            // 
            // checkInPauseLogIdDataGridViewTextBoxColumn
            // 
            this.checkInPauseLogIdDataGridViewTextBoxColumn.DataPropertyName = "CheckInPauseLogId";
            this.checkInPauseLogIdDataGridViewTextBoxColumn.HeaderText = "CheckInPauseLogId";
            this.checkInPauseLogIdDataGridViewTextBoxColumn.Name = "checkInPauseLogIdDataGridViewTextBoxColumn";
            this.checkInPauseLogIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.checkInPauseLogIdDataGridViewTextBoxColumn.Visible = false;
            this.checkInPauseLogIdDataGridViewTextBoxColumn.Width = 129;
            // 
            // checkInDetailIdDataGridViewTextBoxColumn
            // 
            this.checkInDetailIdDataGridViewTextBoxColumn.DataPropertyName = "CheckInDetailId";
            this.checkInDetailIdDataGridViewTextBoxColumn.HeaderText = "CheckInDetailId";
            this.checkInDetailIdDataGridViewTextBoxColumn.Name = "checkInDetailIdDataGridViewTextBoxColumn";
            this.checkInDetailIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.checkInDetailIdDataGridViewTextBoxColumn.Visible = false;
            this.checkInDetailIdDataGridViewTextBoxColumn.Width = 108;
            // 
            // pauseStartTimeDataGridViewTextBoxColumn
            // 
            this.pauseStartTimeDataGridViewTextBoxColumn.DataPropertyName = "PauseStartTime";
            this.pauseStartTimeDataGridViewTextBoxColumn.HeaderText = "Pause Start Time";
            this.pauseStartTimeDataGridViewTextBoxColumn.Name = "pauseStartTimeDataGridViewTextBoxColumn";
            this.pauseStartTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.pauseStartTimeDataGridViewTextBoxColumn.Width = 113;
            // 
            // pauseEndTimeDataGridViewTextBoxColumn
            // 
            this.pauseEndTimeDataGridViewTextBoxColumn.DataPropertyName = "PauseEndTime";
            this.pauseEndTimeDataGridViewTextBoxColumn.HeaderText = "Pause End Time";
            this.pauseEndTimeDataGridViewTextBoxColumn.Name = "pauseEndTimeDataGridViewTextBoxColumn";
            this.pauseEndTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.pauseEndTimeDataGridViewTextBoxColumn.Width = 110;
            // 
            // totalPauseTimeDataGridViewTextBoxColumn
            // 
            this.totalPauseTimeDataGridViewTextBoxColumn.DataPropertyName = "TotalPauseTime";
            this.totalPauseTimeDataGridViewTextBoxColumn.HeaderText = "Total Pause Time";
            this.totalPauseTimeDataGridViewTextBoxColumn.Name = "totalPauseTimeDataGridViewTextBoxColumn";
            this.totalPauseTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalPauseTimeDataGridViewTextBoxColumn.Width = 115;
            // 
            // pOSMachineDataGridViewTextBoxColumn
            // 
            this.pOSMachineDataGridViewTextBoxColumn.DataPropertyName = "POSMachine";
            this.pOSMachineDataGridViewTextBoxColumn.HeaderText = "POSMachine";
            this.pOSMachineDataGridViewTextBoxColumn.Name = "pOSMachineDataGridViewTextBoxColumn";
            this.pOSMachineDataGridViewTextBoxColumn.ReadOnly = true;
            this.pOSMachineDataGridViewTextBoxColumn.Visible = false;
            this.pOSMachineDataGridViewTextBoxColumn.Width = 95;
            // 
            // pausedByDataGridViewTextBoxColumn
            // 
            this.pausedByDataGridViewTextBoxColumn.DataPropertyName = "PausedBy";
            this.pausedByDataGridViewTextBoxColumn.HeaderText = "Paused By";
            this.pausedByDataGridViewTextBoxColumn.Name = "pausedByDataGridViewTextBoxColumn";
            this.pausedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.pausedByDataGridViewTextBoxColumn.Width = 83;
            // 
            // unPausedByDataGridViewTextBoxColumn
            // 
            this.unPausedByDataGridViewTextBoxColumn.DataPropertyName = "UnPausedBy";
            this.unPausedByDataGridViewTextBoxColumn.HeaderText = "UnPaused By";
            this.unPausedByDataGridViewTextBoxColumn.Name = "unPausedByDataGridViewTextBoxColumn";
            this.unPausedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.unPausedByDataGridViewTextBoxColumn.Width = 97;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            this.isActiveDataGridViewCheckBoxColumn.Width = 49;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            this.createdByDataGridViewTextBoxColumn.Width = 81;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            this.creationDateDataGridViewTextBoxColumn.Width = 94;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            this.lastUpdatedByDataGridViewTextBoxColumn.Width = 105;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdateDateDataGridViewTextBoxColumn.Visible = false;
            this.lastUpdateDateDataGridViewTextBoxColumn.Width = 110;
            // 
            // siteIdDataGridViewTextBoxColumn
            // 
            this.siteIdDataGridViewTextBoxColumn.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.Name = "siteIdDataGridViewTextBoxColumn";
            this.siteIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.siteIdDataGridViewTextBoxColumn.Visible = false;
            this.siteIdDataGridViewTextBoxColumn.Width = 59;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            this.masterEntityIdDataGridViewTextBoxColumn.Width = 99;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.ReadOnly = true;
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            this.synchStatusDataGridViewCheckBoxColumn.Width = 73;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            this.guidDataGridViewTextBoxColumn.Width = 54;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isChangedDataGridViewCheckBoxColumn.Visible = false;
            this.isChangedDataGridViewCheckBoxColumn.Width = 64;
            // 
            // checkInPauseLogDTOBindingSource
            // 
            this.checkInPauseLogDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.CheckInPauseLogDTO);
            // 
            // horizontalScrollBarView2
            // 
            this.horizontalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView2.AutoHide = false;
            this.horizontalScrollBarView2.DataGridView = this.dgvCheckInPauseLog;
            this.horizontalScrollBarView2.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.LeftButtonBackgroundImage")));
            this.horizontalScrollBarView2.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView2.Location = new System.Drawing.Point(6, 375);
            this.horizontalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView2.Name = "horizontalScrollBarView2";
            this.horizontalScrollBarView2.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.RightButtonBackgroundImage")));
            this.horizontalScrollBarView2.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView2.ScrollableControl = null;
            this.horizontalScrollBarView2.ScrollViewer = null;
            this.horizontalScrollBarView2.Size = new System.Drawing.Size(550, 40);
            this.horizontalScrollBarView2.TabIndex = 24;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Beige;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(258, 418);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 45);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // CheckInPauseLogUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 494);
            this.Controls.Add(this.grpboxCheckInPauseLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CheckInPauseLogUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Check-In Pause Log";
            this.Load += new System.EventHandler(this.CheckInPauseLogUI_Load);
            this.grpboxCheckInPauseLog.ResumeLayout(false);
            this.grpboxCheckInPauseLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckInPauseLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkInPauseLogDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpboxCheckInPauseLog;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView2;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvCheckInPauseLog;
        private System.Windows.Forms.Label lblTotalPauseTime;
        private System.Windows.Forms.Label lblTimeInMins;
        private System.Windows.Forms.DataGridViewTextBoxColumn checkInPauseLogIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn checkInDetailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pauseStartTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pauseEndTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalPauseTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pOSMachineDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pausedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unPausedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource checkInPauseLogDTOBindingSource;
    }
}