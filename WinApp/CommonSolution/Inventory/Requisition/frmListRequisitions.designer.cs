namespace Semnox.Parafait.Inventory
{
    partial class frmListRequisitions
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
            this.dgvRequisitions = new System.Windows.Forms.DataGridView();
            this.requisitionLinesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSelectRequisitions = new System.Windows.Forms.Button();
            this.SelectRequisition = new System.Windows.Forms.DataGridViewButtonColumn();
            this.requisitionid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requisitionnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestingDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequisitions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requisitionLinesDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRequisitions
            // 
            this.dgvRequisitions.AllowUserToAddRows = false;
            this.dgvRequisitions.AllowUserToDeleteRows = false;
            this.dgvRequisitions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRequisitions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectRequisition,
            this.requisitionid,
            this.requisitionnumber,
            this.RequestingDept,
            this.FromDepartment,
            this.ToDepartment,
            this.Remarks,
            this.CreatedBy,
            this.CreationDate});
            this.dgvRequisitions.Location = new System.Drawing.Point(19, 15);
            this.dgvRequisitions.Name = "dgvRequisitions";
            this.dgvRequisitions.ReadOnly = true;
            this.dgvRequisitions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRequisitions.Size = new System.Drawing.Size(842, 401);
            this.dgvRequisitions.TabIndex = 0;
            this.dgvRequisitions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequisitions_CellContentClick);
            // 
            // requisitionLinesDTOBindingSource
            // 
            this.requisitionLinesDTOBindingSource.DataSource = typeof(RequisitionLinesDTO);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(304, 440);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSelectRequisitions
            // 
            this.btnSelectRequisitions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectRequisitions.Location = new System.Drawing.Point(188, 440);
            this.btnSelectRequisitions.Name = "btnSelectRequisitions";
            this.btnSelectRequisitions.Size = new System.Drawing.Size(75, 23);
            this.btnSelectRequisitions.TabIndex = 4;
            this.btnSelectRequisitions.Text = "OK";
            this.btnSelectRequisitions.UseVisualStyleBackColor = true;
            this.btnSelectRequisitions.Click += new System.EventHandler(this.btnSelectRequisitions_Click);
            // 
            // SelectRequisition
            // 
            this.SelectRequisition.HeaderText = "Select";
            this.SelectRequisition.Name = "SelectRequisition";
            this.SelectRequisition.ReadOnly = true;
            this.SelectRequisition.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SelectRequisition.Text = "...";
            this.SelectRequisition.UseColumnTextForButtonValue = true;
            // 
            // requisitionid
            // 
            this.requisitionid.DataPropertyName = "requisitionid";
            this.requisitionid.HeaderText = "requisitionid";
            this.requisitionid.Name = "requisitionid";
            this.requisitionid.ReadOnly = true;
            this.requisitionid.Visible = false;
            // 
            // requisitionnumber
            // 
            this.requisitionnumber.DataPropertyName = "requisitionnumber";
            this.requisitionnumber.HeaderText = "Requisition Number";
            this.requisitionnumber.Name = "requisitionnumber";
            this.requisitionnumber.ReadOnly = true;
            // 
            // RequestingDept
            // 
            this.RequestingDept.DataPropertyName = "RequestingDept";
            this.RequestingDept.HeaderText = "Requesting Dept";
            this.RequestingDept.Name = "RequestingDept";
            this.RequestingDept.ReadOnly = true;
            // 
            // FromDepartment
            // 
            this.FromDepartment.DataPropertyName = "FromDepartment";
            this.FromDepartment.HeaderText = "From Department";
            this.FromDepartment.Name = "FromDepartment";
            this.FromDepartment.ReadOnly = true;
            // 
            // ToDepartment
            // 
            this.ToDepartment.DataPropertyName = "ToDepartment";
            this.ToDepartment.HeaderText = "To Department";
            this.ToDepartment.Name = "ToDepartment";
            this.ToDepartment.ReadOnly = true;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            // 
            // CreationDate
            // 
            this.CreationDate.DataPropertyName = "CreationDate";
            this.CreationDate.HeaderText = "Creation Date";
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            // 
            // frmListRequisitions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 479);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSelectRequisitions);
            this.Controls.Add(this.dgvRequisitions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmListRequisitions";
            this.Text = "Select Requisitions";
            this.Load += new System.EventHandler(this.frmListRequisitions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequisitions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requisitionLinesDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRequisitions;
        private System.Windows.Forms.BindingSource requisitionLinesDTOBindingSource;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelectRequisitions;
        private System.Windows.Forms.DataGridViewButtonColumn SelectRequisition;
        private System.Windows.Forms.DataGridViewTextBoxColumn requisitionid;
        private System.Windows.Forms.DataGridViewTextBoxColumn requisitionnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestingDept;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
    }
}