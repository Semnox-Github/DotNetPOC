namespace Semnox.Parafait.Inventory
{
    partial class frmInventoryNotes
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
            this.dgvInventoryNotes = new System.Windows.Forms.DataGridView();
            this.NoteTypeDataGridViewCombobox = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.creationDateDataGridViewTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationByDataGridViewTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpInventoryNotes = new System.Windows.Forms.GroupBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.grpRemarks = new System.Windows.Forms.GroupBox();
            this.lblNoteType = new System.Windows.Forms.Label();
            this.cmbNoteType = new System.Windows.Forms.ComboBox();
            this.inventoryNoteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parafaitObjectIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inventoryNotesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryNotes)).BeginInit();
            this.grpInventoryNotes.SuspendLayout();
            this.grpRemarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryNotesDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInventoryNotes
            // 
            this.dgvInventoryNotes.AllowUserToAddRows = false;
            this.dgvInventoryNotes.AllowUserToDeleteRows = false;
            this.dgvInventoryNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInventoryNotes.AutoGenerateColumns = false;
            this.dgvInventoryNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventoryNotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.inventoryNoteIdDataGridViewTextBoxColumn,
            this.NoteTypeDataGridViewCombobox,
            this.parafaitObjectIdDataGridViewTextBoxColumn,
            this.sourceNameDataGridViewTextBoxColumn,
            this.notesDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBox,
            this.creationByDataGridViewTextBox});
            this.dgvInventoryNotes.DataSource = this.inventoryNotesDTOBindingSource;
            this.dgvInventoryNotes.Location = new System.Drawing.Point(19, 19);
            this.dgvInventoryNotes.Name = "dgvInventoryNotes";
            this.dgvInventoryNotes.ReadOnly = true;
            this.dgvInventoryNotes.Size = new System.Drawing.Size(708, 151);
            this.dgvInventoryNotes.TabIndex = 0;
            this.dgvInventoryNotes.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvInventoryNotes_DataError);
            // 
            // NoteTypeDataGridViewCombobox
            // 
            this.NoteTypeDataGridViewCombobox.DataPropertyName = "NoteTypeId";
            this.NoteTypeDataGridViewCombobox.HeaderText = "Note Type";
            this.NoteTypeDataGridViewCombobox.Name = "NoteTypeDataGridViewCombobox";
            this.NoteTypeDataGridViewCombobox.ReadOnly = true;
            this.NoteTypeDataGridViewCombobox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NoteTypeDataGridViewCombobox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.NoteTypeDataGridViewCombobox.Width = 82;
            // 
            // creationDateDataGridViewTextBox
            // 
            this.creationDateDataGridViewTextBox.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBox.HeaderText = "Created Date";
            this.creationDateDataGridViewTextBox.Name = "creationDateDataGridViewTextBox";
            this.creationDateDataGridViewTextBox.ReadOnly = true;
            // 
            // creationByDataGridViewTextBox
            // 
            this.creationByDataGridViewTextBox.DataPropertyName = "LastUpdatedBy";
            this.creationByDataGridViewTextBox.HeaderText = "Created By";
            this.creationByDataGridViewTextBox.Name = "creationByDataGridViewTextBox";
            this.creationByDataGridViewTextBox.ReadOnly = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(156, 368);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Image = global::Semnox.Parafait.Inventory.Properties.Resources.Refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(290, 368);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(98, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(425, 368);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpInventoryNotes
            // 
            this.grpInventoryNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInventoryNotes.BackColor = System.Drawing.Color.White;
            this.grpInventoryNotes.Controls.Add(this.dgvInventoryNotes);
            this.grpInventoryNotes.Location = new System.Drawing.Point(12, 173);
            this.grpInventoryNotes.Name = "grpInventoryNotes";
            this.grpInventoryNotes.Size = new System.Drawing.Size(741, 176);
            this.grpInventoryNotes.TabIndex = 7;
            this.grpInventoryNotes.TabStop = false;
            this.grpInventoryNotes.Text = "Inventory Notes";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.Location = new System.Drawing.Point(114, 46);
            this.txtRemarks.MaxLength = 20000;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(592, 91);
            this.txtRemarks.TabIndex = 1;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(34, 48);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(73, 13);
            this.lblRemarks.TabIndex = 5;
            this.lblRemarks.Text = "Enter Notes :*";
            // 
            // grpRemarks
            // 
            this.grpRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRemarks.Controls.Add(this.lblNoteType);
            this.grpRemarks.Controls.Add(this.cmbNoteType);
            this.grpRemarks.Controls.Add(this.lblRemarks);
            this.grpRemarks.Controls.Add(this.txtRemarks);
            this.grpRemarks.Location = new System.Drawing.Point(12, 12);
            this.grpRemarks.Name = "grpRemarks";
            this.grpRemarks.Size = new System.Drawing.Size(741, 155);
            this.grpRemarks.TabIndex = 6;
            this.grpRemarks.TabStop = false;
            // 
            // lblNoteType
            // 
            this.lblNoteType.AutoSize = true;
            this.lblNoteType.Location = new System.Drawing.Point(12, 17);
            this.lblNoteType.Name = "lblNoteType";
            this.lblNoteType.Size = new System.Drawing.Size(97, 13);
            this.lblNoteType.TabIndex = 8;
            this.lblNoteType.Text = "Select note type :* ";
            // 
            // cmbNoteType
            // 
            this.cmbNoteType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNoteType.FormattingEnabled = true;
            this.cmbNoteType.Location = new System.Drawing.Point(114, 14);
            this.cmbNoteType.Name = "cmbNoteType";
            this.cmbNoteType.Size = new System.Drawing.Size(111, 21);
            this.cmbNoteType.TabIndex = 6;
            // 
            // inventoryNoteIdDataGridViewTextBoxColumn
            // 
            this.inventoryNoteIdDataGridViewTextBoxColumn.DataPropertyName = "InventoryNoteId";
            this.inventoryNoteIdDataGridViewTextBoxColumn.HeaderText = "Inventory Note Id";
            this.inventoryNoteIdDataGridViewTextBoxColumn.Name = "inventoryNoteIdDataGridViewTextBoxColumn";
            this.inventoryNoteIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.inventoryNoteIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // parafaitObjectIdDataGridViewTextBoxColumn
            // 
            this.parafaitObjectIdDataGridViewTextBoxColumn.DataPropertyName = "ParafaitObjectId";
            this.parafaitObjectIdDataGridViewTextBoxColumn.HeaderText = "ParafaitObjectId";
            this.parafaitObjectIdDataGridViewTextBoxColumn.Name = "parafaitObjectIdDataGridViewTextBoxColumn";
            this.parafaitObjectIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.parafaitObjectIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // sourceNameDataGridViewTextBoxColumn
            // 
            this.sourceNameDataGridViewTextBoxColumn.DataPropertyName = "ParafaitObjectName";
            this.sourceNameDataGridViewTextBoxColumn.HeaderText = "Source Name";
            this.sourceNameDataGridViewTextBoxColumn.Name = "sourceNameDataGridViewTextBoxColumn";
            this.sourceNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // notesDataGridViewTextBoxColumn
            // 
            this.notesDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            this.notesDataGridViewTextBoxColumn.HeaderText = "Notes";
            this.notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            this.notesDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // inventoryNotesDTOBindingSource
            // 
            this.inventoryNotesDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryNotesDTO);
            // 
            // frmInventoryNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(765, 403);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpRemarks);
            this.Controls.Add(this.grpInventoryNotes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmInventoryNotes";
            this.Text = "Inventory Notes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryNotes)).EndInit();
            this.grpInventoryNotes.ResumeLayout(false);
            this.grpRemarks.ResumeLayout(false);
            this.grpRemarks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryNotesDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInventoryNotes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource inventoryNotesDTOBindingSource;
        private System.Windows.Forms.GroupBox grpInventoryNotes;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.GroupBox grpRemarks;
        private System.Windows.Forms.Label lblNoteType;
        private System.Windows.Forms.ComboBox cmbNoteType;
        //private System.Windows.Forms.DataGridViewTextBoxColumn noteTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn inventoryNoteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn NoteTypeDataGridViewCombobox;
        private System.Windows.Forms.DataGridViewTextBoxColumn parafaitObjectIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationByDataGridViewTextBox;
    }
}