namespace Semnox.Core.GenericUtilities
{
    partial class ApplicationRemarksUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationRemarksUI));
            this.pnlViewRemarks = new System.Windows.Forms.Panel();
            this.dgvViewRemarks = new System.Windows.Forms.DataGridView();
            this.applicationRemarksDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlEnterRemarks = new System.Windows.Forms.Panel();
            this.btnKeyPad = new System.Windows.Forms.Button();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCloseRemarkEntry = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlViewRemarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvViewRemarks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationRemarksDTOBindingSource)).BeginInit();
            this.pnlEnterRemarks.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlViewRemarks
            // 
            this.pnlViewRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlViewRemarks.BackColor = System.Drawing.SystemColors.Info;
            this.pnlViewRemarks.Controls.Add(this.dgvViewRemarks);
            this.pnlViewRemarks.Controls.Add(this.btnClose);
            this.pnlViewRemarks.Location = new System.Drawing.Point(0, 191);
            this.pnlViewRemarks.Name = "pnlViewRemarks";
            this.pnlViewRemarks.Size = new System.Drawing.Size(543, 203);
            this.pnlViewRemarks.TabIndex = 0;
            // 
            // dgvViewRemarks
            // 
            this.dgvViewRemarks.AllowUserToAddRows = false;
            this.dgvViewRemarks.AllowUserToDeleteRows = false;
            this.dgvViewRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvViewRemarks.AutoGenerateColumns = false;
            this.dgvViewRemarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvViewRemarks.DataSource = this.applicationRemarksDTOBindingSource;
            this.dgvViewRemarks.Location = new System.Drawing.Point(7, 6);
            this.dgvViewRemarks.Name = "dgvViewRemarks";
            this.dgvViewRemarks.ReadOnly = true;
            this.dgvViewRemarks.Size = new System.Drawing.Size(533, 190);
            this.dgvViewRemarks.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(193, 143);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 50);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlEnterRemarks
            // 
            this.pnlEnterRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEnterRemarks.BackColor = System.Drawing.SystemColors.Info;
            this.pnlEnterRemarks.Controls.Add(this.btnKeyPad);
            this.pnlEnterRemarks.Controls.Add(this.txtRemarks);
            this.pnlEnterRemarks.Controls.Add(this.label1);
            this.pnlEnterRemarks.Controls.Add(this.btnCloseRemarkEntry);
            this.pnlEnterRemarks.Controls.Add(this.btnSave);
            this.pnlEnterRemarks.Location = new System.Drawing.Point(0, 0);
            this.pnlEnterRemarks.Name = "pnlEnterRemarks";
            this.pnlEnterRemarks.Size = new System.Drawing.Size(543, 190);
            this.pnlEnterRemarks.TabIndex = 1;
            // 
            // btnKeyPad
            // 
            this.btnKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.BorderSize = 0;
            this.btnKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyPad.Image = global::Semnox.Core.GenericUtilities.Properties.Resources.keyboard;
            this.btnKeyPad.Location = new System.Drawing.Point(385, 141);
            this.btnKeyPad.Name = "btnKeyPad";
            this.btnKeyPad.Size = new System.Drawing.Size(45, 34);
            this.btnKeyPad.TabIndex = 99;
            this.btnKeyPad.UseVisualStyleBackColor = false;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(160, 11);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(342, 104);
            this.txtRemarks.TabIndex = 3;
            this.txtRemarks.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRemarks_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter Remarks:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCloseRemarkEntry
            // 
            this.btnCloseRemarkEntry.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseRemarkEntry.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCloseRemarkEntry.BackgroundImage")));
            this.btnCloseRemarkEntry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCloseRemarkEntry.FlatAppearance.BorderSize = 0;
            this.btnCloseRemarkEntry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseRemarkEntry.ForeColor = System.Drawing.Color.White;
            this.btnCloseRemarkEntry.Location = new System.Drawing.Point(271, 125);
            this.btnCloseRemarkEntry.Name = "btnCloseRemarkEntry";
            this.btnCloseRemarkEntry.Size = new System.Drawing.Size(108, 50);
            this.btnCloseRemarkEntry.TabIndex = 1;
            this.btnCloseRemarkEntry.Text = "Close";
            this.btnCloseRemarkEntry.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCloseRemarkEntry.UseVisualStyleBackColor = false;
            this.btnCloseRemarkEntry.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(146, 125);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(108, 50);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ApplicationRemarksUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 397);
            this.Controls.Add(this.pnlViewRemarks);
            this.Controls.Add(this.pnlEnterRemarks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ApplicationRemarksUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remarks";
            this.pnlViewRemarks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvViewRemarks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationRemarksDTOBindingSource)).EndInit();
            this.pnlEnterRemarks.ResumeLayout(false);
            this.pnlEnterRemarks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlViewRemarks;
        private System.Windows.Forms.DataGridView dgvViewRemarks;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource applicationRemarksDTOBindingSource;
        private System.Windows.Forms.Panel pnlEnterRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCloseRemarkEntry;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnKeyPad;
    }
}

