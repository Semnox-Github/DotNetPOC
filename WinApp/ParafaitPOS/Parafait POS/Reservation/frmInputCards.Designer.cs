namespace Parafait_POS.Reservation
{
    partial class frmInputCards
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.flpTrxCards = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCardSample = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCardTappedSample = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.flpAllocatedCardsSample = new System.Windows.Forms.FlowLayoutPanel();
            this.lblProductToAllotSample = new System.Windows.Forms.Label();
            this.btnAllocatedCardSample = new System.Windows.Forms.Button();
            this.flpProductsToAllocate = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.flpTrxCards.SuspendLayout();
            this.flpAllocatedCardsSample.SuspendLayout();
            this.flpProductsToAllocate.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(593, 487);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(97, 35);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.BackColor = System.Drawing.Color.Transparent;
            this.buttonOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonOK.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonOK.FlatAppearance.BorderSize = 0;
            this.buttonOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(374, 487);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(97, 35);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.BackColor = System.Drawing.Color.Moccasin;
            this.textBoxMessageLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMessageLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Black;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 529);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.ReadOnly = true;
            this.textBoxMessageLine.Size = new System.Drawing.Size(961, 26);
            this.textBoxMessageLine.TabIndex = 7;
            this.textBoxMessageLine.Text = "Please tap cards / tags or allocate from transaction";
            // 
            // flpTrxCards
            // 
            this.flpTrxCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpTrxCards.AutoScroll = true;
            this.flpTrxCards.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpTrxCards.Controls.Add(this.btnCardSample);
            this.flpTrxCards.Controls.Add(this.button1);
            this.flpTrxCards.Controls.Add(this.btnCardTappedSample);
            this.flpTrxCards.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTrxCards.Location = new System.Drawing.Point(1, 23);
            this.flpTrxCards.Name = "flpTrxCards";
            this.flpTrxCards.Size = new System.Drawing.Size(243, 499);
            this.flpTrxCards.TabIndex = 1;
            this.flpTrxCards.WrapContents = false;
            this.flpTrxCards.Click += new System.EventHandler(this.flpTrxCards_Click);
            // 
            // btnCardSample
            // 
            this.btnCardSample.BackgroundImage = global::Parafait_POS.Properties.Resources.greenGradient;
            this.btnCardSample.FlatAppearance.BorderSize = 0;
            this.btnCardSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCardSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCardSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardSample.Location = new System.Drawing.Point(1, 1);
            this.btnCardSample.Margin = new System.Windows.Forms.Padding(1);
            this.btnCardSample.Name = "btnCardSample";
            this.btnCardSample.Size = new System.Drawing.Size(215, 47);
            this.btnCardSample.TabIndex = 0;
            this.btnCardSample.Text = "button1\r\ntest\r\ntest";
            this.btnCardSample.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnCardSample.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Parafait_POS.Properties.Resources.BlankBlack;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1, 50);
            this.button1.Margin = new System.Windows.Forms.Padding(1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(215, 47);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnCardTappedSample
            // 
            this.btnCardTappedSample.BackgroundImage = global::Parafait_POS.Properties.Resources.blueGradient;
            this.btnCardTappedSample.FlatAppearance.BorderSize = 0;
            this.btnCardTappedSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCardTappedSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCardTappedSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardTappedSample.Location = new System.Drawing.Point(1, 99);
            this.btnCardTappedSample.Margin = new System.Windows.Forms.Padding(1);
            this.btnCardTappedSample.Name = "btnCardTappedSample";
            this.btnCardTappedSample.Size = new System.Drawing.Size(215, 47);
            this.btnCardTappedSample.TabIndex = 2;
            this.btnCardTappedSample.Text = "button3";
            this.btnCardTappedSample.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnCardTappedSample.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Transaction/Tapped Tags";
            // 
            // flpAllocatedCardsSample
            // 
            this.flpAllocatedCardsSample.AutoScroll = true;
            this.flpAllocatedCardsSample.BackColor = System.Drawing.Color.White;
            this.flpAllocatedCardsSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpAllocatedCardsSample.Controls.Add(this.lblProductToAllotSample);
            this.flpAllocatedCardsSample.Controls.Add(this.btnAllocatedCardSample);
            this.flpAllocatedCardsSample.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpAllocatedCardsSample.Location = new System.Drawing.Point(3, 3);
            this.flpAllocatedCardsSample.Name = "flpAllocatedCardsSample";
            this.flpAllocatedCardsSample.Size = new System.Drawing.Size(243, 436);
            this.flpAllocatedCardsSample.TabIndex = 12;
            this.flpAllocatedCardsSample.WrapContents = false;
            // 
            // lblProductToAllotSample
            // 
            this.lblProductToAllotSample.AutoEllipsis = true;
            this.lblProductToAllotSample.BackColor = System.Drawing.Color.Gainsboro;
            this.lblProductToAllotSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductToAllotSample.ForeColor = System.Drawing.Color.Black;
            this.lblProductToAllotSample.Location = new System.Drawing.Point(0, 0);
            this.lblProductToAllotSample.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.lblProductToAllotSample.Name = "lblProductToAllotSample";
            this.lblProductToAllotSample.Size = new System.Drawing.Size(216, 48);
            this.lblProductToAllotSample.TabIndex = 1;
            this.lblProductToAllotSample.Text = "label2";
            // 
            // btnAllocatedCardSample
            // 
            this.btnAllocatedCardSample.BackgroundImage = global::Parafait_POS.Properties.Resources.greenGradient;
            this.btnAllocatedCardSample.FlatAppearance.BorderSize = 0;
            this.btnAllocatedCardSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAllocatedCardSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAllocatedCardSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAllocatedCardSample.Location = new System.Drawing.Point(1, 51);
            this.btnAllocatedCardSample.Margin = new System.Windows.Forms.Padding(1);
            this.btnAllocatedCardSample.Name = "btnAllocatedCardSample";
            this.btnAllocatedCardSample.Size = new System.Drawing.Size(215, 47);
            this.btnAllocatedCardSample.TabIndex = 0;
            this.btnAllocatedCardSample.Text = "button1";
            this.btnAllocatedCardSample.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnAllocatedCardSample.UseVisualStyleBackColor = true;
            // 
            // flpProductsToAllocate
            // 
            this.flpProductsToAllocate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpProductsToAllocate.AutoScroll = true;
            this.flpProductsToAllocate.Controls.Add(this.flpAllocatedCardsSample);
            this.flpProductsToAllocate.Location = new System.Drawing.Point(247, 20);
            this.flpProductsToAllocate.Name = "flpProductsToAllocate";
            this.flpProductsToAllocate.Size = new System.Drawing.Size(712, 462);
            this.flpProductsToAllocate.TabIndex = 13;
            this.flpProductsToAllocate.WrapContents = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(251, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Products";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(171, 4);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 15;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // frmInputCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 555);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flpProductsToAllocate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flpTrxCards);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxMessageLine);
            this.Name = "frmInputCards";
            this.Text = "Input Cards / Tags";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInputCards_FormClosed);
            this.Load += new System.EventHandler(this.frmInputCards_Load);
            this.flpTrxCards.ResumeLayout(false);
            this.flpAllocatedCardsSample.ResumeLayout(false);
            this.flpProductsToAllocate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxMessageLine;
        private System.Windows.Forms.FlowLayoutPanel flpTrxCards;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCardSample;
        private System.Windows.Forms.FlowLayoutPanel flpAllocatedCardsSample;
        private System.Windows.Forms.Label lblProductToAllotSample;
        private System.Windows.Forms.Button btnAllocatedCardSample;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flpProductsToAllocate;
        private System.Windows.Forms.Button btnCardTappedSample;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkSelectAll;
    }
}