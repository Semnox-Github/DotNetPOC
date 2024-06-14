namespace Parafait_POS.Reservation
{
    partial class frmBookingCheckList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingCheckList));
            this.txtBookingName = new System.Windows.Forms.TextBox();
            this.lblBookingName = new System.Windows.Forms.Label();
            this.dgvBookingCheckList = new System.Windows.Forms.DataGridView();
            this.bookingCheckListIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventHostUserId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.hostNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checklistTaskGroupId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.checkListTaskGroupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingCheckListDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.vlScrollBarCheckList = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.horizontalScrollBarView1 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingCheckList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookingCheckListDTOBindingSource)).BeginInit();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBookingName
            // 
            this.txtBookingName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBookingName.Location = new System.Drawing.Point(117, 7);
            this.txtBookingName.MaxLength = 50;
            this.txtBookingName.Name = "txtBookingName";
            this.txtBookingName.ReadOnly = true;
            this.txtBookingName.Size = new System.Drawing.Size(263, 30);
            this.txtBookingName.TabIndex = 71;
            // 
            // lblBookingName
            // 
            this.lblBookingName.Location = new System.Drawing.Point(5, 5);
            this.lblBookingName.Name = "lblBookingName";
            this.lblBookingName.Size = new System.Drawing.Size(108, 35);
            this.lblBookingName.TabIndex = 109;
            this.lblBookingName.Text = "Booking Name:";
            this.lblBookingName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvBookingCheckList
            // 
            this.dgvBookingCheckList.AllowUserToDeleteRows = false;
            this.dgvBookingCheckList.AutoGenerateColumns = false;
            this.dgvBookingCheckList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookingCheckList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bookingCheckListIdDataGridViewTextBoxColumn,
            this.bookingIdDataGridViewTextBoxColumn,
            this.eventHostUserId,
            this.hostNameDataGridViewTextBoxColumn,
            this.checklistTaskGroupId,
            this.checkListTaskGroupNameDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn});
            this.dgvBookingCheckList.DataSource = this.bookingCheckListDTOBindingSource;
            this.dgvBookingCheckList.Location = new System.Drawing.Point(23, 51);
            this.dgvBookingCheckList.Name = "dgvBookingCheckList";
            this.dgvBookingCheckList.RowHeadersVisible = false;
            this.dgvBookingCheckList.RowTemplate.Height = 30;
            this.dgvBookingCheckList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvBookingCheckList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookingCheckList.Size = new System.Drawing.Size(1154, 323);
            this.dgvBookingCheckList.TabIndex = 114;
            this.dgvBookingCheckList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingCheckList_CellClick);
            this.dgvBookingCheckList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBookingCheckList_DataError);
            this.dgvBookingCheckList.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvBookingCheckList_DefaultValuesNeeded);
            // 
            // bookingCheckListIdDataGridViewTextBoxColumn
            // 
            this.bookingCheckListIdDataGridViewTextBoxColumn.DataPropertyName = "BookingCheckListId";
            this.bookingCheckListIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.bookingCheckListIdDataGridViewTextBoxColumn.Name = "bookingCheckListIdDataGridViewTextBoxColumn";
            this.bookingCheckListIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bookingIdDataGridViewTextBoxColumn
            // 
            this.bookingIdDataGridViewTextBoxColumn.DataPropertyName = "BookingId";
            this.bookingIdDataGridViewTextBoxColumn.HeaderText = "Booking Id";
            this.bookingIdDataGridViewTextBoxColumn.Name = "bookingIdDataGridViewTextBoxColumn";
            this.bookingIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.bookingIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // eventHostUserId
            // 
            this.eventHostUserId.DataPropertyName = "EventHostUserId";
            this.eventHostUserId.HeaderText = "Host";
            this.eventHostUserId.MinimumWidth = 200;
            this.eventHostUserId.Name = "eventHostUserId";
            this.eventHostUserId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.eventHostUserId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.eventHostUserId.Width = 200;
            // 
            // hostNameDataGridViewTextBoxColumn
            // 
            this.hostNameDataGridViewTextBoxColumn.DataPropertyName = "HostName";
            this.hostNameDataGridViewTextBoxColumn.HeaderText = "Host Name";
            this.hostNameDataGridViewTextBoxColumn.Name = "hostNameDataGridViewTextBoxColumn";
            this.hostNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // checklistTaskGroupId
            // 
            this.checklistTaskGroupId.DataPropertyName = "ChecklistTaskGroupId";
            this.checklistTaskGroupId.HeaderText = "Check List";
            this.checklistTaskGroupId.MinimumWidth = 250;
            this.checklistTaskGroupId.Name = "checklistTaskGroupId";
            this.checklistTaskGroupId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.checklistTaskGroupId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.checklistTaskGroupId.Width = 250;
            // 
            // checkListTaskGroupNameDataGridViewTextBoxColumn
            // 
            this.checkListTaskGroupNameDataGridViewTextBoxColumn.DataPropertyName = "CheckListTaskGroupName";
            this.checkListTaskGroupNameDataGridViewTextBoxColumn.HeaderText = "Check List Name";
            this.checkListTaskGroupNameDataGridViewTextBoxColumn.Name = "checkListTaskGroupNameDataGridViewTextBoxColumn";
            this.checkListTaskGroupNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.MinimumWidth = 70;
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "true";
            this.isActiveDataGridViewCheckBoxColumn.Width = 70;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.MinimumWidth = 130;
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Width = 130;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.MinimumWidth = 130;
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Width = 130;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Update By";
            this.lastUpdatedByDataGridViewTextBoxColumn.MinimumWidth = 130;
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Width = 130;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn.MinimumWidth = 130;
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdateDateDataGridViewTextBoxColumn.Width = 130;
            // 
            // bookingCheckListDTOBindingSource
            // 
            this.bookingCheckListDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.BookingCheckListDTO);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(792, 465);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(135, 39);
            this.btnCancel.TabIndex = 122;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(276, 462);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 42);
            this.btnSave.TabIndex = 121;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // vlScrollBarCheckList
            // 
            this.vlScrollBarCheckList.AutoHide = false;
            this.vlScrollBarCheckList.DataGridView = this.dgvBookingCheckList;
            this.vlScrollBarCheckList.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vlScrollBarCheckList.DownButtonBackgroundImage")));
            this.vlScrollBarCheckList.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vlScrollBarCheckList.DownButtonDisabledBackgroundImage")));
            this.vlScrollBarCheckList.Location = new System.Drawing.Point(1179, 51);
            this.vlScrollBarCheckList.Margin = new System.Windows.Forms.Padding(0);
            this.vlScrollBarCheckList.Name = "vlScrollBarCheckList";
            this.vlScrollBarCheckList.ScrollableControl = null;
            this.vlScrollBarCheckList.ScrollViewer = null;
            this.vlScrollBarCheckList.Size = new System.Drawing.Size(47, 323);
            this.vlScrollBarCheckList.TabIndex = 123;
            this.vlScrollBarCheckList.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vlScrollBarCheckList.UpButtonBackgroundImage")));
            this.vlScrollBarCheckList.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vlScrollBarCheckList.UpButtonDisabledBackgroundImage")));
            this.vlScrollBarCheckList.UpButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.vlScrollBarCheckList.DownButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(620, 463);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(121, 42);
            this.btnDelete.TabIndex = 124;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // horizontalScrollBarView1
            // 
            this.horizontalScrollBarView1.AutoHide = false;
            this.horizontalScrollBarView1.DataGridView = this.dgvBookingCheckList;
            this.horizontalScrollBarView1.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonBackgroundImage")));
            this.horizontalScrollBarView1.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.Location = new System.Drawing.Point(23, 376);
            this.horizontalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView1.Name = "horizontalScrollBarView1";
            this.horizontalScrollBarView1.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonBackgroundImage")));
            this.horizontalScrollBarView1.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.ScrollableControl = null;
            this.horizontalScrollBarView1.ScrollViewer = null;
            this.horizontalScrollBarView1.Size = new System.Drawing.Size(1155, 46);
            this.horizontalScrollBarView1.TabIndex = 125;
            this.horizontalScrollBarView1.LeftButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.horizontalScrollBarView1.RightButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // pnlBase
            // 
            this.pnlBase.AutoScroll = true;
            this.pnlBase.Controls.Add(this.btnRefresh);
            this.pnlBase.Controls.Add(this.lblBookingName);
            this.pnlBase.Controls.Add(this.txtBookingName);
            this.pnlBase.Controls.Add(this.dgvBookingCheckList);
            this.pnlBase.Controls.Add(this.horizontalScrollBarView1);
            this.pnlBase.Controls.Add(this.vlScrollBarCheckList);
            this.pnlBase.Controls.Add(this.btnSave);
            this.pnlBase.Controls.Add(this.btnDelete);
            this.pnlBase.Controls.Add(this.btnCancel);
            this.pnlBase.Location = new System.Drawing.Point(2, 2);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(1256, 560);
            this.pnlBase.TabIndex = 126;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(448, 462);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(121, 42);
            this.btnRefresh.TabIndex = 126;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmBookingCheckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 531);
            this.Controls.Add(this.pnlBase);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBookingCheckList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Booking Check List";
            this.Load += new System.EventHandler(this.frmBookingCheckList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingCheckList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookingCheckListDTOBindingSource)).EndInit();
            this.pnlBase.ResumeLayout(false);
            this.pnlBase.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBookingName;
        private System.Windows.Forms.Label lblBookingName;
        private System.Windows.Forms.DataGridView dgvBookingCheckList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vlScrollBarCheckList;
        private System.Windows.Forms.Button btnDelete; 
        private System.Windows.Forms.BindingSource bookingCheckListDTOBindingSource;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView1;
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingCheckListIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn eventHostUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn checklistTaskGroupId;
        private System.Windows.Forms.DataGridViewTextBoxColumn checkListTaskGroupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
    }
}