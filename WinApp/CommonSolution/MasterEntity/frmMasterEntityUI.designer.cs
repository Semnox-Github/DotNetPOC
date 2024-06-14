namespace Semnox.Parafait.GenericUtilities
{
    partial class frmMasterEntityUI
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
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14});
            this.splitContainerRoles = new System.Windows.Forms.SplitContainer();
            this.tvOrganization = new System.Windows.Forms.TreeView();
            this.lblEntity = new System.Windows.Forms.Label();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.btnSiteSearchParameter = new System.Windows.Forms.Button();
            this.btnMasterSearchParameter = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvMasterData = new System.Windows.Forms.DataGridView();
            this.grpMasterDetails = new System.Windows.Forms.GroupBox();
            this.grpSiteDetails = new System.Windows.Forms.GroupBox();
            this.dgvSiteData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRoles)).BeginInit();
            this.splitContainerRoles.Panel1.SuspendLayout();
            this.splitContainerRoles.Panel2.SuspendLayout();
            this.splitContainerRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterData)).BeginInit();
            this.grpSiteDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiteData)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerRoles
            // 
            this.splitContainerRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerRoles.BackColor = System.Drawing.Color.SkyBlue;
            this.splitContainerRoles.Location = new System.Drawing.Point(4, 5);
            this.splitContainerRoles.Name = "splitContainerRoles";
            // 
            // splitContainerRoles.Panel1
            // 
            this.splitContainerRoles.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainerRoles.Panel1.Controls.Add(this.tvOrganization);
            // 
            // splitContainerRoles.Panel2
            // 
            this.splitContainerRoles.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainerRoles.Panel2.Controls.Add(this.lblEntity);
            this.splitContainerRoles.Panel2.Controls.Add(this.cmbTables);
            this.splitContainerRoles.Panel2.Controls.Add(this.btnSiteSearchParameter);
            this.splitContainerRoles.Panel2.Controls.Add(this.btnMasterSearchParameter);
            this.splitContainerRoles.Panel2.Controls.Add(this.btnClose);
            this.splitContainerRoles.Panel2.Controls.Add(this.btnSave);
            this.splitContainerRoles.Panel2.Controls.Add(this.dgvMasterData);
            this.splitContainerRoles.Panel2.Controls.Add(this.grpMasterDetails);
            this.splitContainerRoles.Panel2.Controls.Add(this.grpSiteDetails);
            this.splitContainerRoles.Size = new System.Drawing.Size(1157, 448);
            this.splitContainerRoles.SplitterDistance = 206;
            this.splitContainerRoles.TabIndex = 9;
            // 
            // tvOrganization
            // 
            this.tvOrganization.BackColor = System.Drawing.Color.PaleTurquoise;
            this.tvOrganization.CheckBoxes = true;
            this.tvOrganization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrganization.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvOrganization.Location = new System.Drawing.Point(0, 0);
            this.tvOrganization.Name = "tvOrganization";
            treeNode13.Name = "Node1";
            treeNode13.Text = "Node1";
            treeNode14.Name = "Node2";
            treeNode14.Text = "Node2";
            treeNode15.Name = "Node0";
            treeNode15.Text = "Node0";
            this.tvOrganization.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode15});
            this.tvOrganization.Size = new System.Drawing.Size(206, 448);
            this.tvOrganization.TabIndex = 1;
            this.tvOrganization.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvOrganization_AfterCheck);
            // 
            // lblEntity
            // 
            this.lblEntity.AutoSize = true;
            this.lblEntity.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntity.Location = new System.Drawing.Point(13, 20);
            this.lblEntity.Name = "lblEntity";
            this.lblEntity.Size = new System.Drawing.Size(51, 16);
            this.lblEntity.TabIndex = 11;
            this.lblEntity.Text = "Entity :";
            // 
            // cmbTables
            // 
            this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTables.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new System.Drawing.Point(66, 18);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(125, 23);
            this.cmbTables.TabIndex = 9;
            this.cmbTables.SelectedValueChanged += new System.EventHandler(this.cmbTables_SelectedValueChanged);
            // 
            // btnSiteSearchParameter
            // 
            this.btnSiteSearchParameter.Location = new System.Drawing.Point(797, 18);
            this.btnSiteSearchParameter.Name = "btnSiteSearchParameter";
            this.btnSiteSearchParameter.Size = new System.Drawing.Size(136, 23);
            this.btnSiteSearchParameter.TabIndex = 8;
            this.btnSiteSearchParameter.Text = "Advanced Search ";
            this.btnSiteSearchParameter.UseVisualStyleBackColor = true;
            this.btnSiteSearchParameter.Click += new System.EventHandler(this.btnSiteSearchParameter_Click);
            // 
            // btnMasterSearchParameter
            // 
            this.btnMasterSearchParameter.Location = new System.Drawing.Point(197, 18);
            this.btnMasterSearchParameter.Name = "btnMasterSearchParameter";
            this.btnMasterSearchParameter.Size = new System.Drawing.Size(140, 23);
            this.btnMasterSearchParameter.TabIndex = 7;
            this.btnMasterSearchParameter.Text = "Advanced Search ";
            this.btnMasterSearchParameter.UseVisualStyleBackColor = true;
            this.btnMasterSearchParameter.Click += new System.EventHandler(this.btnMasterSearchParameter_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(144, 410);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(26, 410);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvMasterData
            // 
            this.dgvMasterData.AllowUserToAddRows = false;
            this.dgvMasterData.AllowUserToDeleteRows = false;
            this.dgvMasterData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvMasterData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMasterData.Location = new System.Drawing.Point(15, 66);
            this.dgvMasterData.Name = "dgvMasterData";
            this.dgvMasterData.ReadOnly = true;
            this.dgvMasterData.Size = new System.Drawing.Size(450, 326);
            this.dgvMasterData.TabIndex = 0;
            // 
            // grpMasterDetails
            // 
            this.grpMasterDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpMasterDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMasterDetails.Location = new System.Drawing.Point(7, 48);
            this.grpMasterDetails.Name = "grpMasterDetails";
            this.grpMasterDetails.Size = new System.Drawing.Size(464, 353);
            this.grpMasterDetails.TabIndex = 12;
            this.grpMasterDetails.TabStop = false;
            this.grpMasterDetails.Text = "Master Site Details";
            // 
            // grpSiteDetails
            // 
            this.grpSiteDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSiteDetails.Controls.Add(this.dgvSiteData);
            this.grpSiteDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSiteDetails.Location = new System.Drawing.Point(477, 48);
            this.grpSiteDetails.Name = "grpSiteDetails";
            this.grpSiteDetails.Size = new System.Drawing.Size(464, 353);
            this.grpSiteDetails.TabIndex = 13;
            this.grpSiteDetails.TabStop = false;
            this.grpSiteDetails.Text = "Site Details";
            // 
            // dgvSiteData
            // 
            this.dgvSiteData.AllowUserToAddRows = false;
            this.dgvSiteData.AllowUserToDeleteRows = false;
            this.dgvSiteData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSiteData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSiteData.Location = new System.Drawing.Point(6, 18);
            this.dgvSiteData.Name = "dgvSiteData";
            this.dgvSiteData.Size = new System.Drawing.Size(450, 326);
            this.dgvSiteData.TabIndex = 10;
            this.dgvSiteData.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvSiteData_CellValidating);
            // 
            // frmMasterEntityUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 461);
            this.Controls.Add(this.splitContainerRoles);
            this.Name = "frmMasterEntityUI";
            this.Text = "Mapping Site and HQ Entity";
            this.Load += new System.EventHandler(this.frmMasterEntityUI_Load);
            this.splitContainerRoles.Panel1.ResumeLayout(false);
            this.splitContainerRoles.Panel2.ResumeLayout(false);
            this.splitContainerRoles.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRoles)).EndInit();
            this.splitContainerRoles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterData)).EndInit();
            this.grpSiteDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiteData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerRoles;
        private System.Windows.Forms.DataGridView dgvSiteData;
        private System.Windows.Forms.ComboBox cmbTables;
        private System.Windows.Forms.Button btnSiteSearchParameter;
        private System.Windows.Forms.Button btnMasterSearchParameter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvMasterData;
        private System.Windows.Forms.Label lblEntity;
        private System.Windows.Forms.GroupBox grpMasterDetails;
        private System.Windows.Forms.GroupBox grpSiteDetails;
        private System.Windows.Forms.TreeView tvOrganization;
    }
}