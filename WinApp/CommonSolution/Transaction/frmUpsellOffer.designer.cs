namespace Semnox.Parafait.Transaction
{
    partial class frmUpsellOffer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpsellOffer));
            this.flpUpsellOffersList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSample = new System.Windows.Forms.Button();
            this.lblProductname = new System.Windows.Forms.Label();
            this.btnNoThanks = new System.Windows.Forms.Button();
            this.btnYesPlease = new System.Windows.Forms.Button();
            this.txtOfferMessage = new System.Windows.Forms.TextBox();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.flpUpsellOffersList.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpUpsellOffersList
            // 
            this.flpUpsellOffersList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flpUpsellOffersList.AutoScroll = true;
            this.flpUpsellOffersList.BackColor = System.Drawing.Color.Azure;
            this.flpUpsellOffersList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpUpsellOffersList.Controls.Add(this.btnSample);
            this.flpUpsellOffersList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.flpUpsellOffersList.ForeColor = System.Drawing.SystemColors.MenuText;
            this.flpUpsellOffersList.Location = new System.Drawing.Point(1, 107);
            this.flpUpsellOffersList.Name = "flpUpsellOffersList";
            this.flpUpsellOffersList.Size = new System.Drawing.Size(764, 280);
            this.flpUpsellOffersList.TabIndex = 0;
            // 
            // btnSample
            // 
            this.btnSample.BackColor = System.Drawing.Color.Transparent;
            this.btnSample.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSample.BackgroundImage")));
            this.btnSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSample.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSample.FlatAppearance.BorderSize = 0;
            this.btnSample.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSample.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.btnSample.ForeColor = System.Drawing.Color.White;
            this.btnSample.Location = new System.Drawing.Point(3, 3);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(80, 60);
            this.btnSample.TabIndex = 0;
            this.btnSample.Text = "sample";
            this.btnSample.UseVisualStyleBackColor = false;
            this.btnSample.Visible = false;
            // 
            // lblProductname
            // 
            this.lblProductname.AutoSize = true;
            this.lblProductname.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductname.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblProductname.Location = new System.Drawing.Point(14, 15);
            this.lblProductname.Name = "lblProductname";
            this.lblProductname.Size = new System.Drawing.Size(176, 24);
            this.lblProductname.TabIndex = 15;
            this.lblProductname.Text = "You have selected :";
            // 
            // btnNoThanks
            // 
            this.btnNoThanks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNoThanks.BackColor = System.Drawing.Color.Transparent;
            this.btnNoThanks.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.normal2;
            this.btnNoThanks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNoThanks.FlatAppearance.BorderSize = 0;
            this.btnNoThanks.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNoThanks.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNoThanks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoThanks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.btnNoThanks.ForeColor = System.Drawing.SystemColors.Control;
            this.btnNoThanks.Location = new System.Drawing.Point(463, 413);
            this.btnNoThanks.Margin = new System.Windows.Forms.Padding(5);
            this.btnNoThanks.Name = "btnNoThanks";
            this.btnNoThanks.Size = new System.Drawing.Size(161, 48);
            this.btnNoThanks.TabIndex = 14;
            this.btnNoThanks.Text = "No Thanks";
            this.btnNoThanks.UseVisualStyleBackColor = false;
            this.btnNoThanks.Click += new System.EventHandler(this.btnNoThanks_Click);
            // 
            // btnYesPlease
            // 
            this.btnYesPlease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnYesPlease.BackColor = System.Drawing.Color.Transparent;
            this.btnYesPlease.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.normal2;
            this.btnYesPlease.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnYesPlease.Enabled = false;
            this.btnYesPlease.FlatAppearance.BorderSize = 0;
            this.btnYesPlease.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYesPlease.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYesPlease.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYesPlease.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.btnYesPlease.ForeColor = System.Drawing.SystemColors.Control;
            this.btnYesPlease.Location = new System.Drawing.Point(127, 413);
            this.btnYesPlease.Margin = new System.Windows.Forms.Padding(5);
            this.btnYesPlease.Name = "btnYesPlease";
            this.btnYesPlease.Size = new System.Drawing.Size(161, 48);
            this.btnYesPlease.TabIndex = 13;
            this.btnYesPlease.Text = "Yes Please";
            this.btnYesPlease.UseVisualStyleBackColor = false;
            this.btnYesPlease.Click += new System.EventHandler(this.btnYesPlease_Click);
            // 
            // txtOfferMessage
            // 
            this.txtOfferMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOfferMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOfferMessage.ForeColor = System.Drawing.Color.Sienna;
            this.txtOfferMessage.Location = new System.Drawing.Point(18, 43);
            this.txtOfferMessage.Multiline = true;
            this.txtOfferMessage.Name = "txtOfferMessage";
            this.txtOfferMessage.Size = new System.Drawing.Size(747, 64);
            this.txtOfferMessage.TabIndex = 19;
            this.txtOfferMessage.Text = "Special Offer :";
            this.txtOfferMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOfferMessage_KeyPress);
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMessageLine.CausesValidation = false;
            this.textBoxMessageLine.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Sienna;
            this.textBoxMessageLine.Location = new System.Drawing.Point(1, 472);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(763, 19);
            this.textBoxMessageLine.TabIndex = 20;
            // 
            // frmUpsellOffer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(766, 493);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.txtOfferMessage);
            this.Controls.Add(this.lblProductname);
            this.Controls.Add(this.btnNoThanks);
            this.Controls.Add(this.btnYesPlease);
            this.Controls.Add(this.flpUpsellOffersList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmUpsellOffer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upsell Offer";
            this.flpUpsellOffersList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpUpsellOffersList;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Button btnYesPlease;
        private System.Windows.Forms.Button btnNoThanks;
        private System.Windows.Forms.Label lblProductname;
        private System.Windows.Forms.TextBox txtOfferMessage;
        private System.Windows.Forms.TextBox textBoxMessageLine;
    }
}

