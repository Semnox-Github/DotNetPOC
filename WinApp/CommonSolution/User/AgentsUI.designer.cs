namespace Semnox.Parafait.User
{
    partial class AgentsUI
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
            this.tabAgentUI = new System.Windows.Forms.TabControl();
            this.tabAgents = new System.Windows.Forms.TabPage();
            this.btnAgentNew = new System.Windows.Forms.Button();
            this.btnAgentEdit = new System.Windows.Forms.Button();
            this.btnAgentUserClose = new System.Windows.Forms.Button();
            this.btnAgentUserDelete = new System.Windows.Forms.Button();
            this.btnAgentUserRefresh = new System.Windows.Forms.Button();
            this.lblPartner = new System.Windows.Forms.Label();
            this.dgAgentUser = new System.Windows.Forms.DataGridView();
            this.cmbSelectPartner = new System.Windows.Forms.ComboBox();
            this.tabAgentGroups = new System.Windows.Forms.TabPage();
            this.btnAgentGroupClose = new System.Windows.Forms.Button();
            this.btnAgentGroupRefresh = new System.Windows.Forms.Button();
            this.btnAgentGroupSave = new System.Windows.Forms.Button();
            this.btnAgentgroupDelete = new System.Windows.Forms.Button();
            this.cmbPartnerAgentGroup = new System.Windows.Forms.ComboBox();
            this.dgAgentGroup = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExclude = new System.Windows.Forms.Button();
            this.btnInclude = new System.Windows.Forms.Button();
            this.lstAssignedAgents = new System.Windows.Forms.ListBox();
            this.lstAvailableAgents = new System.Windows.Forms.ListBox();
            this.lbltab2Partner = new System.Windows.Forms.Label();
            this.agentIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobileNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commissionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedUserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bsAgentUser = new System.Windows.Forms.BindingSource(this.components);
            this.agentGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.partnerIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedUserDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agentGroupsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabAgentUI.SuspendLayout();
            this.tabAgents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAgentUser)).BeginInit();
            this.tabAgentGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAgentGroup)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAgentUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agentGroupsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabAgentUI
            // 
            this.tabAgentUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabAgentUI.Controls.Add(this.tabAgents);
            this.tabAgentUI.Controls.Add(this.tabAgentGroups);
            this.tabAgentUI.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAgentUI.Location = new System.Drawing.Point(12, 9);
            this.tabAgentUI.Margin = new System.Windows.Forms.Padding(0);
            this.tabAgentUI.Name = "tabAgentUI";
            this.tabAgentUI.SelectedIndex = 0;
            this.tabAgentUI.Size = new System.Drawing.Size(855, 478);
            this.tabAgentUI.TabIndex = 31;
            // 
            // tabAgents
            // 
            this.tabAgents.BackColor = System.Drawing.SystemColors.Control;
            this.tabAgents.Controls.Add(this.btnAgentNew);
            this.tabAgents.Controls.Add(this.btnAgentEdit);
            this.tabAgents.Controls.Add(this.btnAgentUserClose);
            this.tabAgents.Controls.Add(this.btnAgentUserDelete);
            this.tabAgents.Controls.Add(this.btnAgentUserRefresh);
            this.tabAgents.Controls.Add(this.lblPartner);
            this.tabAgents.Controls.Add(this.dgAgentUser);
            this.tabAgents.Controls.Add(this.cmbSelectPartner);
            this.tabAgents.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAgents.Location = new System.Drawing.Point(4, 24);
            this.tabAgents.Name = "tabAgents";
            this.tabAgents.Padding = new System.Windows.Forms.Padding(3);
            this.tabAgents.Size = new System.Drawing.Size(847, 450);
            this.tabAgents.TabIndex = 0;
            this.tabAgents.Text = "Agents";
            // 
            // btnAgentNew
            // 
            this.btnAgentNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentNew.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentNew.Location = new System.Drawing.Point(17, 413);
            this.btnAgentNew.Name = "btnAgentNew";
            this.btnAgentNew.Size = new System.Drawing.Size(108, 23);
            this.btnAgentNew.TabIndex = 54;
            this.btnAgentNew.Text = "New";
            this.btnAgentNew.UseVisualStyleBackColor = true;
            this.btnAgentNew.Click += new System.EventHandler(this.btnAgentNew_Click);
            // 
            // btnAgentEdit
            // 
            this.btnAgentEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentEdit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentEdit.Location = new System.Drawing.Point(146, 413);
            this.btnAgentEdit.Name = "btnAgentEdit";
            this.btnAgentEdit.Size = new System.Drawing.Size(108, 23);
            this.btnAgentEdit.TabIndex = 51;
            this.btnAgentEdit.Text = "Edit";
            this.btnAgentEdit.UseVisualStyleBackColor = true;
            this.btnAgentEdit.Click += new System.EventHandler(this.btnAgentEdit_Click);
            // 
            // btnAgentUserClose
            // 
            this.btnAgentUserClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentUserClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentUserClose.Location = new System.Drawing.Point(404, 413);
            this.btnAgentUserClose.Name = "btnAgentUserClose";
            this.btnAgentUserClose.Size = new System.Drawing.Size(108, 23);
            this.btnAgentUserClose.TabIndex = 50;
            this.btnAgentUserClose.Text = "Close";
            this.btnAgentUserClose.UseVisualStyleBackColor = true;
            this.btnAgentUserClose.Click += new System.EventHandler(this.btnAgentUserClose_Click);
            // 
            // btnAgentUserDelete
            // 
            this.btnAgentUserDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentUserDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentUserDelete.Location = new System.Drawing.Point(533, 413);
            this.btnAgentUserDelete.Name = "btnAgentUserDelete";
            this.btnAgentUserDelete.Size = new System.Drawing.Size(108, 23);
            this.btnAgentUserDelete.TabIndex = 49;
            this.btnAgentUserDelete.Text = "Delete";
            this.btnAgentUserDelete.UseVisualStyleBackColor = true;
            this.btnAgentUserDelete.Visible = false;
            this.btnAgentUserDelete.Click += new System.EventHandler(this.btnAgentUserDelete_Click);
            // 
            // btnAgentUserRefresh
            // 
            this.btnAgentUserRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentUserRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentUserRefresh.Location = new System.Drawing.Point(275, 413);
            this.btnAgentUserRefresh.Name = "btnAgentUserRefresh";
            this.btnAgentUserRefresh.Size = new System.Drawing.Size(108, 23);
            this.btnAgentUserRefresh.TabIndex = 47;
            this.btnAgentUserRefresh.Text = "Refresh";
            this.btnAgentUserRefresh.UseVisualStyleBackColor = true;
            this.btnAgentUserRefresh.Click += new System.EventHandler(this.btnAgentUserRefresh_Click);
            // 
            // lblPartner
            // 
            this.lblPartner.AutoSize = true;
            this.lblPartner.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblPartner.Location = new System.Drawing.Point(14, 11);
            this.lblPartner.Name = "lblPartner";
            this.lblPartner.Size = new System.Drawing.Size(95, 15);
            this.lblPartner.TabIndex = 46;
            this.lblPartner.Text = "Select Partner :";
            // 
            // dgAgentUser
            // 
            this.dgAgentUser.AllowUserToAddRows = false;
            this.dgAgentUser.AllowUserToDeleteRows = false;
            this.dgAgentUser.AllowUserToOrderColumns = true;
            this.dgAgentUser.AllowUserToResizeRows = false;
            this.dgAgentUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgAgentUser.AutoGenerateColumns = false;
            this.dgAgentUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgAgentUser.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dgAgentUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAgentUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.agentIdDataGridViewTextBoxColumn,
            this.loginIdDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.userNameDataGridViewTextBoxColumn,
            this.partnerIdDataGridViewTextBoxColumn,
            this.userIdDataGridViewTextBoxColumn,
            this.mobileNoDataGridViewTextBoxColumn,
            this.commissionDataGridViewTextBoxColumn,
            this.activeDataGridViewCheckBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedUserDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn,
            this.siteidDataGridViewTextBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn});
            this.dgAgentUser.DataSource = this.bsAgentUser;
            this.dgAgentUser.GridColor = System.Drawing.SystemColors.Control;
            this.dgAgentUser.Location = new System.Drawing.Point(17, 41);
            this.dgAgentUser.MultiSelect = false;
            this.dgAgentUser.Name = "dgAgentUser";
            this.dgAgentUser.Size = new System.Drawing.Size(798, 360);
            this.dgAgentUser.TabIndex = 44;
            // 
            // cmbSelectPartner
            // 
            this.cmbSelectPartner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectPartner.FormattingEnabled = true;
            this.cmbSelectPartner.Location = new System.Drawing.Point(127, 8);
            this.cmbSelectPartner.Name = "cmbSelectPartner";
            this.cmbSelectPartner.Size = new System.Drawing.Size(213, 23);
            this.cmbSelectPartner.TabIndex = 45;
            this.cmbSelectPartner.SelectedIndexChanged += new System.EventHandler(this.cmbSelectPartner_SelectedIndexChanged);
            // 
            // tabAgentGroups
            // 
            this.tabAgentGroups.BackColor = System.Drawing.SystemColors.Control;
            this.tabAgentGroups.Controls.Add(this.btnAgentGroupClose);
            this.tabAgentGroups.Controls.Add(this.btnAgentGroupRefresh);
            this.tabAgentGroups.Controls.Add(this.btnAgentGroupSave);
            this.tabAgentGroups.Controls.Add(this.btnAgentgroupDelete);
            this.tabAgentGroups.Controls.Add(this.cmbPartnerAgentGroup);
            this.tabAgentGroups.Controls.Add(this.dgAgentGroup);
            this.tabAgentGroups.Controls.Add(this.groupBox1);
            this.tabAgentGroups.Controls.Add(this.lbltab2Partner);
            this.tabAgentGroups.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAgentGroups.Location = new System.Drawing.Point(4, 24);
            this.tabAgentGroups.Name = "tabAgentGroups";
            this.tabAgentGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabAgentGroups.Size = new System.Drawing.Size(847, 450);
            this.tabAgentGroups.TabIndex = 1;
            this.tabAgentGroups.Text = "Agent Group";
            // 
            // btnAgentGroupClose
            // 
            this.btnAgentGroupClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentGroupClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentGroupClose.Location = new System.Drawing.Point(418, 418);
            this.btnAgentGroupClose.Name = "btnAgentGroupClose";
            this.btnAgentGroupClose.Size = new System.Drawing.Size(102, 23);
            this.btnAgentGroupClose.TabIndex = 58;
            this.btnAgentGroupClose.Text = "Close";
            this.btnAgentGroupClose.UseVisualStyleBackColor = true;
            this.btnAgentGroupClose.Click += new System.EventHandler(this.btnAgentGroupClose_Click);
            // 
            // btnAgentGroupRefresh
            // 
            this.btnAgentGroupRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentGroupRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentGroupRefresh.Location = new System.Drawing.Point(285, 418);
            this.btnAgentGroupRefresh.Name = "btnAgentGroupRefresh";
            this.btnAgentGroupRefresh.Size = new System.Drawing.Size(102, 23);
            this.btnAgentGroupRefresh.TabIndex = 57;
            this.btnAgentGroupRefresh.Text = "Refresh";
            this.btnAgentGroupRefresh.UseVisualStyleBackColor = true;
            this.btnAgentGroupRefresh.Click += new System.EventHandler(this.btnAgentGroupRefresh_Click);
            // 
            // btnAgentGroupSave
            // 
            this.btnAgentGroupSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentGroupSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentGroupSave.Location = new System.Drawing.Point(19, 418);
            this.btnAgentGroupSave.Name = "btnAgentGroupSave";
            this.btnAgentGroupSave.Size = new System.Drawing.Size(102, 23);
            this.btnAgentGroupSave.TabIndex = 56;
            this.btnAgentGroupSave.Text = "Save";
            this.btnAgentGroupSave.UseVisualStyleBackColor = true;
            this.btnAgentGroupSave.Click += new System.EventHandler(this.btnAgentGroupSave_Click);
            // 
            // btnAgentgroupDelete
            // 
            this.btnAgentgroupDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentgroupDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentgroupDelete.Location = new System.Drawing.Point(152, 418);
            this.btnAgentgroupDelete.Name = "btnAgentgroupDelete";
            this.btnAgentgroupDelete.Size = new System.Drawing.Size(102, 23);
            this.btnAgentgroupDelete.TabIndex = 55;
            this.btnAgentgroupDelete.Text = "Delete";
            this.btnAgentgroupDelete.UseVisualStyleBackColor = true;
            this.btnAgentgroupDelete.Click += new System.EventHandler(this.btnAgentgroupDelete_Click_1);
            // 
            // cmbPartnerAgentGroup
            // 
            this.cmbPartnerAgentGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartnerAgentGroup.FormattingEnabled = true;
            this.cmbPartnerAgentGroup.Location = new System.Drawing.Point(126, 9);
            this.cmbPartnerAgentGroup.Name = "cmbPartnerAgentGroup";
            this.cmbPartnerAgentGroup.Size = new System.Drawing.Size(213, 23);
            this.cmbPartnerAgentGroup.TabIndex = 46;
            this.cmbPartnerAgentGroup.SelectedIndexChanged += new System.EventHandler(this.cmbPartnerAgentGroup_SelectedIndexChanged);
            // 
            // dgAgentGroup
            // 
            this.dgAgentGroup.AllowUserToOrderColumns = true;
            this.dgAgentGroup.AutoGenerateColumns = false;
            this.dgAgentGroup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgAgentGroup.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dgAgentGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAgentGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.agentGroupIdDataGridViewTextBoxColumn,
            this.groupNameDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.siteidDataGridViewTextBoxColumn1,
            this.guidDataGridViewTextBoxColumn1,
            this.synchStatusDataGridViewCheckBoxColumn1,
            this.partnerIdDataGridViewTextBoxColumn1,
            this.createdByDataGridViewTextBoxColumn1,
            this.creationDateDataGridViewTextBoxColumn1,
            this.lastUpdatedUserDataGridViewTextBoxColumn1,
            this.lastUpdatedDateDataGridViewTextBoxColumn1});
            this.dgAgentGroup.DataSource = this.agentGroupsDTOBindingSource;
            this.dgAgentGroup.GridColor = System.Drawing.SystemColors.Control;
            this.dgAgentGroup.Location = new System.Drawing.Point(16, 41);
            this.dgAgentGroup.MultiSelect = false;
            this.dgAgentGroup.Name = "dgAgentGroup";
            this.dgAgentGroup.Size = new System.Drawing.Size(802, 165);
            this.dgAgentGroup.TabIndex = 45;
            this.dgAgentGroup.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgAgentGroup_RowEnter);
            this.dgAgentGroup.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgAgentGroup_DataError);
            this.dgAgentGroup.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgAgentGroup_RowEnter);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnExclude);
            this.groupBox1.Controls.Add(this.btnInclude);
            this.groupBox1.Controls.Add(this.lstAssignedAgents);
            this.groupBox1.Controls.Add(this.lstAvailableAgents);
            this.groupBox1.Location = new System.Drawing.Point(16, 212);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(722, 200);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Agents";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(414, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Assigned Agents";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Available Agents";
            // 
            // btnExclude
            // 
            this.btnExclude.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExclude.Location = new System.Drawing.Point(289, 118);
            this.btnExclude.Name = "btnExclude";
            this.btnExclude.Size = new System.Drawing.Size(52, 34);
            this.btnExclude.TabIndex = 10;
            this.btnExclude.Text = "<<";
            this.btnExclude.UseVisualStyleBackColor = true;
            this.btnExclude.Click += new System.EventHandler(this.btnExclude_Click);
            // 
            // btnInclude
            // 
            this.btnInclude.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInclude.Location = new System.Drawing.Point(289, 57);
            this.btnInclude.Name = "btnInclude";
            this.btnInclude.Size = new System.Drawing.Size(52, 34);
            this.btnInclude.TabIndex = 9;
            this.btnInclude.Text = ">>";
            this.btnInclude.UseVisualStyleBackColor = true;
            this.btnInclude.Click += new System.EventHandler(this.btnInclude_Click);
            // 
            // lstAssignedAgents
            // 
            this.lstAssignedAgents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstAssignedAgents.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAssignedAgents.FormattingEnabled = true;
            this.lstAssignedAgents.ItemHeight = 14;
            this.lstAssignedAgents.Location = new System.Drawing.Point(417, 36);
            this.lstAssignedAgents.Name = "lstAssignedAgents";
            this.lstAssignedAgents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAssignedAgents.Size = new System.Drawing.Size(243, 144);
            this.lstAssignedAgents.Sorted = true;
            this.lstAssignedAgents.TabIndex = 8;
            // 
            // lstAvailableAgents
            // 
            this.lstAvailableAgents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstAvailableAgents.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAvailableAgents.FormattingEnabled = true;
            this.lstAvailableAgents.ItemHeight = 14;
            this.lstAvailableAgents.Location = new System.Drawing.Point(6, 36);
            this.lstAvailableAgents.MultiColumn = true;
            this.lstAvailableAgents.Name = "lstAvailableAgents";
            this.lstAvailableAgents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailableAgents.Size = new System.Drawing.Size(243, 144);
            this.lstAvailableAgents.Sorted = true;
            this.lstAvailableAgents.TabIndex = 7;
            // 
            // lbltab2Partner
            // 
            this.lbltab2Partner.AutoSize = true;
            this.lbltab2Partner.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lbltab2Partner.Location = new System.Drawing.Point(13, 12);
            this.lbltab2Partner.Name = "lbltab2Partner";
            this.lbltab2Partner.Size = new System.Drawing.Size(95, 15);
            this.lbltab2Partner.TabIndex = 40;
            this.lbltab2Partner.Text = "Select Partner :";
            // 
            // agentIdDataGridViewTextBoxColumn
            // 
            this.agentIdDataGridViewTextBoxColumn.DataPropertyName = "AgentId";
            this.agentIdDataGridViewTextBoxColumn.HeaderText = "Agent Id";
            this.agentIdDataGridViewTextBoxColumn.Name = "agentIdDataGridViewTextBoxColumn";
            this.agentIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // loginIdDataGridViewTextBoxColumn
            // 
            this.loginIdDataGridViewTextBoxColumn.DataPropertyName = "LoginId";
            this.loginIdDataGridViewTextBoxColumn.HeaderText = "Login Id";
            this.loginIdDataGridViewTextBoxColumn.Name = "loginIdDataGridViewTextBoxColumn";
            this.loginIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "Email";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            this.emailDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "User Name";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // partnerIdDataGridViewTextBoxColumn
            // 
            this.partnerIdDataGridViewTextBoxColumn.DataPropertyName = "PartnerId";
            this.partnerIdDataGridViewTextBoxColumn.HeaderText = "Partner Id";
            this.partnerIdDataGridViewTextBoxColumn.Name = "partnerIdDataGridViewTextBoxColumn";
            this.partnerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.partnerIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // userIdDataGridViewTextBoxColumn
            // 
            this.userIdDataGridViewTextBoxColumn.DataPropertyName = "User_Id";
            this.userIdDataGridViewTextBoxColumn.HeaderText = "User Id";
            this.userIdDataGridViewTextBoxColumn.Name = "userIdDataGridViewTextBoxColumn";
            this.userIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.userIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // mobileNoDataGridViewTextBoxColumn
            // 
            this.mobileNoDataGridViewTextBoxColumn.DataPropertyName = "MobileNo";
            this.mobileNoDataGridViewTextBoxColumn.HeaderText = "Mobile No";
            this.mobileNoDataGridViewTextBoxColumn.Name = "mobileNoDataGridViewTextBoxColumn";
            this.mobileNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // commissionDataGridViewTextBoxColumn
            // 
            this.commissionDataGridViewTextBoxColumn.DataPropertyName = "Commission";
            this.commissionDataGridViewTextBoxColumn.HeaderText = "Commission";
            this.commissionDataGridViewTextBoxColumn.Name = "commissionDataGridViewTextBoxColumn";
            this.commissionDataGridViewTextBoxColumn.ReadOnly = true;
            this.commissionDataGridViewTextBoxColumn.Visible = false;
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "Active";
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            this.activeDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedUserDataGridViewTextBoxColumn
            // 
            this.lastUpdatedUserDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedUser";
            this.lastUpdatedUserDataGridViewTextBoxColumn.HeaderText = "Last Updated User";
            this.lastUpdatedUserDataGridViewTextBoxColumn.Name = "lastUpdatedUserDataGridViewTextBoxColumn";
            this.lastUpdatedUserDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // siteidDataGridViewTextBoxColumn
            // 
            this.siteidDataGridViewTextBoxColumn.DataPropertyName = "Site_id";
            this.siteidDataGridViewTextBoxColumn.HeaderText = "Site id ";
            this.siteidDataGridViewTextBoxColumn.Name = "siteidDataGridViewTextBoxColumn";
            this.siteidDataGridViewTextBoxColumn.ReadOnly = true;
            this.siteidDataGridViewTextBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid ";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "Synch Status";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.ReadOnly = true;
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // bsAgentUser
            // 
            this.bsAgentUser.DataSource = typeof(AgentUserDTO);
            // 
            // agentGroupIdDataGridViewTextBoxColumn
            // 
            this.agentGroupIdDataGridViewTextBoxColumn.DataPropertyName = "AgentGroupId";
            this.agentGroupIdDataGridViewTextBoxColumn.HeaderText = "Agent Group Id";
            this.agentGroupIdDataGridViewTextBoxColumn.Name = "agentGroupIdDataGridViewTextBoxColumn";
            this.agentGroupIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // groupNameDataGridViewTextBoxColumn
            // 
            this.groupNameDataGridViewTextBoxColumn.DataPropertyName = "GroupName";
            this.groupNameDataGridViewTextBoxColumn.HeaderText = "Group Name";
            this.groupNameDataGridViewTextBoxColumn.Name = "groupNameDataGridViewTextBoxColumn";
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            // 
            // siteidDataGridViewTextBoxColumn1
            // 
            this.siteidDataGridViewTextBoxColumn1.DataPropertyName = "Site_id";
            this.siteidDataGridViewTextBoxColumn1.HeaderText = "Site id ";
            this.siteidDataGridViewTextBoxColumn1.Name = "siteidDataGridViewTextBoxColumn1";
            this.siteidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn1
            // 
            this.guidDataGridViewTextBoxColumn1.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn1.HeaderText = "Guid ";
            this.guidDataGridViewTextBoxColumn1.Name = "guidDataGridViewTextBoxColumn1";
            this.guidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn1
            // 
            this.synchStatusDataGridViewCheckBoxColumn1.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.HeaderText = "Synch Status";
            this.synchStatusDataGridViewCheckBoxColumn1.Name = "synchStatusDataGridViewCheckBoxColumn1";
            this.synchStatusDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // partnerIdDataGridViewTextBoxColumn1
            // 
            this.partnerIdDataGridViewTextBoxColumn1.DataPropertyName = "PartnerId";
            this.partnerIdDataGridViewTextBoxColumn1.HeaderText = "PartnerId";
            this.partnerIdDataGridViewTextBoxColumn1.Name = "partnerIdDataGridViewTextBoxColumn1";
            this.partnerIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn1
            // 
            this.createdByDataGridViewTextBoxColumn1.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn1.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn1.Name = "createdByDataGridViewTextBoxColumn1";
            this.createdByDataGridViewTextBoxColumn1.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn1
            // 
            this.creationDateDataGridViewTextBoxColumn1.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn1.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn1.Name = "creationDateDataGridViewTextBoxColumn1";
            this.creationDateDataGridViewTextBoxColumn1.Visible = false;
            // 
            // lastUpdatedUserDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedUserDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedUser";
            this.lastUpdatedUserDataGridViewTextBoxColumn1.HeaderText = "Last Updated By";
            this.lastUpdatedUserDataGridViewTextBoxColumn1.Name = "lastUpdatedUserDataGridViewTextBoxColumn1";
            this.lastUpdatedUserDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.Name = "lastUpdatedDateDataGridViewTextBoxColumn1";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // agentGroupsDTOBindingSource
            // 
            this.agentGroupsDTOBindingSource.DataSource = typeof(AgentGroupsDTO);
            // 
            // AgentsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(876, 495);
            this.Controls.Add(this.tabAgentUI);
            this.Name = "AgentsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agents";
            this.tabAgentUI.ResumeLayout(false);
            this.tabAgents.ResumeLayout(false);
            this.tabAgents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAgentUser)).EndInit();
            this.tabAgentGroups.ResumeLayout(false);
            this.tabAgentGroups.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAgentGroup)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAgentUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agentGroupsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsAgentUser;
        private System.Windows.Forms.TabControl tabAgentUI;
        private System.Windows.Forms.TabPage tabAgents;
        private System.Windows.Forms.TabPage tabAgentGroups;
        private System.Windows.Forms.Label lbltab2Partner;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExclude;
        private System.Windows.Forms.Button btnInclude;
        private System.Windows.Forms.ListBox lstAssignedAgents;
        private System.Windows.Forms.DataGridView dgAgentGroup;
        private System.Windows.Forms.ListBox lstAvailableAgents;
        private System.Windows.Forms.BindingSource agentGroupsDTOBindingSource;
        private System.Windows.Forms.Button btnAgentEdit;
        private System.Windows.Forms.Button btnAgentUserClose;
        private System.Windows.Forms.Button btnAgentUserDelete;
        private System.Windows.Forms.Button btnAgentUserRefresh;
        private System.Windows.Forms.Label lblPartner;
        private System.Windows.Forms.DataGridView dgAgentUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn agentIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobileNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commissionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedUserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.ComboBox cmbSelectPartner;
        private System.Windows.Forms.ComboBox cmbPartnerAgentGroup;
        private System.Windows.Forms.Button btnAgentNew;
        private System.Windows.Forms.Button btnAgentGroupSave;
        private System.Windows.Forms.Button btnAgentgroupDelete;
        private System.Windows.Forms.Button btnAgentGroupClose;
        private System.Windows.Forms.Button btnAgentGroupRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn agentGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnerIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedUserDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn1;
    }
}

