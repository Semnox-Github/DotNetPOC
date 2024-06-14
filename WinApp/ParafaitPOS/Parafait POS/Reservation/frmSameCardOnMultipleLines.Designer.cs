namespace Parafait_POS.Reservation
{
    partial class frmSameCardOnMultipleLines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSameCardOnMultipleLines));
            this.pnlCardMapDetails = new System.Windows.Forms.Panel();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.cardMapVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lblMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlCardMapDetails
            // 
            this.pnlCardMapDetails.AutoScroll = true;
            this.pnlCardMapDetails.Location = new System.Drawing.Point(4, 39);
            this.pnlCardMapDetails.Name = "pnlCardMapDetails";
            this.pnlCardMapDetails.Size = new System.Drawing.Size(344, 422);
            this.pnlCardMapDetails.TabIndex = 126;
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(53, 467);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(116, 34);
            this.btnYes.TabIndex = 129;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(199, 467);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(116, 34);
            this.btnNo.TabIndex = 129;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // cardMapVScrollBar
            // 
            this.cardMapVScrollBar.AutoHide = false;
            this.cardMapVScrollBar.DataGridView = null;
            this.cardMapVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("cardMapVScrollBar.DownButtonBackgroundImage")));
            this.cardMapVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("cardMapVScrollBar.DownButtonDisabledBackgroundImage")));
            this.cardMapVScrollBar.Location = new System.Drawing.Point(330, 39);
            this.cardMapVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.cardMapVScrollBar.Name = "cardMapVScrollBar";
            this.cardMapVScrollBar.ScrollableControl = this.pnlCardMapDetails;
            this.cardMapVScrollBar.ScrollViewer = null;
            this.cardMapVScrollBar.Size = new System.Drawing.Size(40, 422);
            this.cardMapVScrollBar.TabIndex = 130;
            this.cardMapVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("cardMapVScrollBar.UpButtonBackgroundImage")));
            this.cardMapVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("cardMapVScrollBar.UpButtonDisabledBackgroundImage")));
            this.cardMapVScrollBar.UpButtonClick += new System.EventHandler(this.ScrollBar_ButtonClick);
            this.cardMapVScrollBar.DownButtonClick += new System.EventHandler(this.ScrollBar_ButtonClick);
            // 
            // lblMsg
            // 
            this.lblMsg.Font = new System.Drawing.Font("Arial", 9F);
            this.lblMsg.Location = new System.Drawing.Point(4, 1);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(327, 35);
            this.lblMsg.TabIndex = 131;
            this.lblMsg.Text = "Do you want to proceed with this mapping?";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSameCardOnMultipleLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(373, 506);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.cardMapVScrollBar);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.pnlCardMapDetails);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSameCardOnMultipleLines";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Same Card Mapped to Muiltple Product Lines";
            this.Load += new System.EventHandler(this.frmSameCardOnMultipleLines_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlCardMapDetails;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView cardMapVScrollBar;
        private System.Windows.Forms.Label lblMsg;
    }
}