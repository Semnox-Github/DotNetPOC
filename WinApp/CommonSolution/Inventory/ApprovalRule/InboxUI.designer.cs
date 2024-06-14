namespace Semnox.Parafait.Inventory
{
    partial class InboxUI
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.cmbNotification = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblNotification = new System.Windows.Forms.Label();
            this.flpPendingApproval = new System.Windows.Forms.FlowLayoutPanel();
            this.flpHistory = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFilter.SuspendLayout();
            this.flpHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(53, 272);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(85, 33);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(171, 272);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 33);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlFilter
            // 
            this.pnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.cmbNotification);
            this.pnlFilter.Controls.Add(this.btnSearch);
            this.pnlFilter.Controls.Add(this.lblNotification);
            this.pnlFilter.Location = new System.Drawing.Point(3, 3);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(265, 46);
            this.pnlFilter.TabIndex = 5;
            // 
            // cmbNotification
            // 
            this.cmbNotification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNotification.FormattingEnabled = true;
            this.cmbNotification.Items.AddRange(new object[] {
            "NONE",
            "APPROVED",
            "REJECTED",
            "CANCELLED"});
            this.cmbNotification.Location = new System.Drawing.Point(93, 12);
            this.cmbNotification.Name = "cmbNotification";
            this.cmbNotification.Size = new System.Drawing.Size(97, 21);
            this.cmbNotification.TabIndex = 3;
            this.cmbNotification.Text = "NONE";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(196, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(53, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblNotification
            // 
            this.lblNotification.Location = new System.Drawing.Point(4, 10);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(86, 23);
            this.lblNotification.TabIndex = 2;
            this.lblNotification.Text = "Notification :";
            this.lblNotification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flpPendingApproval
            // 
            this.flpPendingApproval.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpPendingApproval.AutoScroll = true;
            this.flpPendingApproval.Location = new System.Drawing.Point(8, 3);
            this.flpPendingApproval.Name = "flpPendingApproval";
            this.flpPendingApproval.Size = new System.Drawing.Size(498, 252);
            this.flpPendingApproval.TabIndex = 3;
            // 
            // flpHistory
            // 
            this.flpHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpHistory.AutoScroll = true;
            this.flpHistory.Controls.Add(this.pnlFilter);
            this.flpHistory.Location = new System.Drawing.Point(512, 3);
            this.flpHistory.Name = "flpHistory";
            this.flpHistory.Size = new System.Drawing.Size(287, 310);
            this.flpHistory.TabIndex = 6;
            // 
            // InboxUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(805, 317);
            this.Controls.Add(this.flpHistory);
            this.Controls.Add(this.flpPendingApproval);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "InboxUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inbox";
            this.Load += new System.EventHandler(this.InboxUI_Load);
            this.pnlFilter.ResumeLayout(false);
            this.flpHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbNotification;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.FlowLayoutPanel flpPendingApproval;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.FlowLayoutPanel flpHistory;
    }
}

