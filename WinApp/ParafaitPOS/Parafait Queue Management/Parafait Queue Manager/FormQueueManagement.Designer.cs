namespace ParafaitQueueManagement
{
    partial class FormQueueMgmt
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvActivePlayersList = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.dgridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolstripLoginID = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolstripVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.tooltipadjust = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSiteName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtCenterClose = new System.Windows.Forms.TextBox();
            this.dgActivePlayersContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnInitialize = new System.Windows.Forms.Button();
            this.btnReinstate = new System.Windows.Forms.Button();
            this.btnSetupTeam = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();

            ((System.ComponentModel.ISupportInitialize)(this.dgvActivePlayersList)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(69, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Active Players";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(513, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Game Queue";
            // 
            // dgvActivePlayersList
            // 
            this.dgvActivePlayersList.BackgroundColor = System.Drawing.Color.White;
            this.dgvActivePlayersList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvActivePlayersList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvActivePlayersList.Location = new System.Drawing.Point(12, 103);
            this.dgvActivePlayersList.Name = "dgvActivePlayersList";
            this.dgvActivePlayersList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvActivePlayersList.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvActivePlayersList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvActivePlayersList.Size = new System.Drawing.Size(231, 318);
            this.dgvActivePlayersList.TabIndex = 9;
            this.dgvActivePlayersList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvActivePlayersList_CellValueChanged);
            this.dgvActivePlayersList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvActivePlayersList_CurrentCellDirtyStateChanged);
            this.dgvActivePlayersList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvActivePlayersList_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(80, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "label3";
            this.label3.Visible = false;
            // 
            // dgridContextMenu
            // 
            this.dgridContextMenu.Name = "dgridContextMenu";
            this.dgridContextMenu.Size = new System.Drawing.Size(61, 4);
            this.dgridContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.dgridContextMenu_ItemClicked);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 434);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripLoginID,
            this.toolstripVersion,
            this.tooltipadjust,
            this.toolStripRole,
            this.toolStripTime,
            this.toolStripSiteName,
            this.toolStripServer});
            this.statusStrip.Location = new System.Drawing.Point(0, 585);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1298, 22);
            this.statusStrip.TabIndex = 15;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolstripLoginID
            // 
            this.toolstripLoginID.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolstripLoginID.Name = "toolstripLoginID";
            this.toolstripLoginID.Size = new System.Drawing.Size(113, 17);
            this.toolstripLoginID.Text = "toolStripStatusLabel1";
            // 
            // toolstripVersion
            // 
            this.toolstripVersion.Name = "toolstripVersion";
            this.toolstripVersion.Size = new System.Drawing.Size(109, 17);
            this.toolstripVersion.Text = "toolStripStatusLabel1";
            // 
            // tooltipadjust
            // 
            this.tooltipadjust.Name = "tooltipadjust";
            this.tooltipadjust.Size = new System.Drawing.Size(726, 17);
            this.tooltipadjust.Spring = true;
            // 
            // toolStripRole
            // 
            this.toolStripRole.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripRole.Name = "toolStripRole";
            this.toolStripRole.Size = new System.Drawing.Size(113, 17);
            this.toolStripRole.Text = "toolStripStatusLabel1";
            // 
            // toolStripTime
            // 
            this.toolStripTime.Name = "toolStripTime";
            this.toolStripTime.Size = new System.Drawing.Size(109, 17);
            this.toolStripTime.Text = "toolStripStatusLabel1";
            // 
            // toolStripSiteName
            // 
            this.toolStripSiteName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripSiteName.Name = "toolStripSiteName";
            this.toolStripSiteName.Size = new System.Drawing.Size(113, 17);
            this.toolStripSiteName.Text = "toolStripStatusLabel1";
            this.toolStripSiteName.Visible = false;
            // 
            // toolStripServer
            // 
            this.toolStripServer.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripServer.Name = "toolStripServer";
            this.toolStripServer.Size = new System.Drawing.Size(113, 17);
            this.toolStripServer.Text = "toolStripStatusLabel1";
            // 
            // txtCenterClose
            // 
            this.txtCenterClose.Location = new System.Drawing.Point(12, 464);
            this.txtCenterClose.Name = "txtCenterClose";
            this.txtCenterClose.ReadOnly = true;
            this.txtCenterClose.Size = new System.Drawing.Size(231, 21);
            this.txtCenterClose.TabIndex = 17;
            // 
            // dgActivePlayersContextMenu
            // 
            this.dgActivePlayersContextMenu.Name = "dgActivePlayersContextMenu";
            this.dgActivePlayersContextMenu.Size = new System.Drawing.Size(61, 4);
            this.dgActivePlayersContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.dgActivePlayersContextMenu_ItemClicked);
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(12, 528);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(75, 23);
            this.btnInitialize.TabIndex = 18;
            this.btnInitialize.Text = "Initialize";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // btnReinstate
            // 
            this.btnReinstate.Location = new System.Drawing.Point(127, 528);
            this.btnReinstate.Name = "btnReinstate";
            this.btnReinstate.Size = new System.Drawing.Size(116, 23);
            this.btnReinstate.TabIndex = 19;
            this.btnReinstate.Text = "Reinstate Customer";
            this.btnReinstate.UseVisualStyleBackColor = true;
            this.btnReinstate.Click += new System.EventHandler(this.btnReinstate_Click);
            // 
            // btnSetupTeam
            // 
            this.btnSetupTeam.Location = new System.Drawing.Point(298, 528);
            this.btnSetupTeam.Name = "btnSetupTeam";
            this.btnSetupTeam.Size = new System.Drawing.Size(98, 23);
            this.btnSetupTeam.TabIndex = 20;
            this.btnSetupTeam.Text = "Setup Team";
            this.btnSetupTeam.UseVisualStyleBackColor = true;
            this.btnSetupTeam.Click += new System.EventHandler(this.btnSetupTeam_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(459, 527);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 585);
            this.splitter1.TabIndex = 23;
            this.splitter1.TabStop = false;
            // 
            // FormQueueMgmt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1298, 607);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSetupTeam);
            this.Controls.Add(this.btnReinstate);
            this.Controls.Add(this.btnInitialize);
            this.Controls.Add(this.txtCenterClose);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvActivePlayersList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "FormQueueMgmt";
            this.Text = "Parafait Queue Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQueueMgmt_FormClosing);
            this.Load += new System.EventHandler(this.FormQueueMgmt_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormQueueMgmt_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivePlayersList)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvActivePlayersList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip dgridContextMenu;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolstripLoginID;
        private System.Windows.Forms.ToolStripStatusLabel toolstripVersion;
        private System.Windows.Forms.ToolStripStatusLabel tooltipadjust;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRole;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSiteName;
        private System.Windows.Forms.TextBox txtCenterClose;
        private System.Windows.Forms.ContextMenuStrip dgActivePlayersContextMenu;
        private System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.Button btnReinstate;
        private System.Windows.Forms.Button btnSetupTeam;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripServer;
    }
}

