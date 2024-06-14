namespace Semnox.Parafait.Customer
{
    partial class CustomerTermsandConditionsUI
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
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.gpTermsandConditions = new System.Windows.Forms.GroupBox();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.wbTerms = new System.Windows.Forms.WebBrowser();
            this.gpTermsandConditions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.normal2;
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNo.Location = new System.Drawing.Point(542, 534);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(161, 43);
            this.btnNo.TabIndex = 18;
            this.btnNo.Text = "Cancel";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.normal2;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnYes.Location = new System.Drawing.Point(331, 534);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(161, 43);
            this.btnYes.TabIndex = 17;
            this.btnYes.Text = "Agree";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // gpTermsandConditions
            // 
            this.gpTermsandConditions.Controls.Add(this.vScrollBar);
            this.gpTermsandConditions.Controls.Add(this.wbTerms);
            this.gpTermsandConditions.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpTermsandConditions.Location = new System.Drawing.Point(12, 12);
            this.gpTermsandConditions.Name = "gpTermsandConditions";
            this.gpTermsandConditions.Size = new System.Drawing.Size(978, 494);
            this.gpTermsandConditions.TabIndex = 20;
            this.gpTermsandConditions.TabStop = false;
            this.gpTermsandConditions.Text = "Terms and Conditions";
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.LargeChange = 150;
            this.vScrollBar.Location = new System.Drawing.Point(911, 17);
            this.vScrollBar.Maximum = 2000;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(64, 474);
            this.vScrollBar.SmallChange = 50;
            this.vScrollBar.TabIndex = 20;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // wbTerms
            // 
            this.wbTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbTerms.Location = new System.Drawing.Point(3, 18);
            this.wbTerms.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbTerms.Name = "wbTerms";
            this.wbTerms.ScrollBarsEnabled = false;
            this.wbTerms.Size = new System.Drawing.Size(923, 470);
            this.wbTerms.TabIndex = 21;
            // 
            // CustomerTermsandConditionsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1002, 600);
            this.Controls.Add(this.gpTermsandConditions);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomerTermsandConditionsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomerTermsandConditionsUI";
            this.gpTermsandConditions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.GroupBox gpTermsandConditions;
        private System.Windows.Forms.WebBrowser wbTerms;
        private System.Windows.Forms.VScrollBar vScrollBar;
    }
}