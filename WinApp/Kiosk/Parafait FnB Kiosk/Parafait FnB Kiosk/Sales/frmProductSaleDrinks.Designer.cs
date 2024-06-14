namespace Parafait_FnB_Kiosk
{
    partial class frmProductSaleDrinks
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
            this.flpParameters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblScreenTitle2 = new System.Windows.Forms.Label();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.flpDrinkParams = new System.Windows.Forms.FlowLayoutPanel();
            this.panelParameterSample = new System.Windows.Forms.Panel();
            this.flpDisplayTextsSample = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDisplayText1Sample = new System.Windows.Forms.Label();
            this.lblDisplayText2Sample = new System.Windows.Forms.Label();
            this.cmbParameter = new System.Windows.Forms.ComboBox();
            this.lblComboDispSample = new System.Windows.Forms.Label();
            this.lblScreenFooter1 = new System.Windows.Forms.Label();
            this.lblScreenFooter2 = new System.Windows.Forms.Label();
            this.panelBG.SuspendLayout();
            this.flpParameters.SuspendLayout();
            this.flpDrinkParams.SuspendLayout();
            this.panelParameterSample.SuspendLayout();
            this.flpDisplayTextsSample.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Drinks_Pop_up_Screen_Purple_Bg;
            this.panelBG.Controls.Add(this.flpParameters);
            this.panelBG.Size = new System.Drawing.Size(955, 1652);
            this.panelBG.Controls.SetChildIndex(this.panelAddItem, 0);
            this.panelBG.Controls.SetChildIndex(this.btnClose, 0);
            this.panelBG.Controls.SetChildIndex(this.flpParameters, 0);
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // panelAddItem
            // 
            this.panelAddItem.Location = new System.Drawing.Point(0, 1347);
            // 
            // flpParameters
            // 
            this.flpParameters.Controls.Add(this.lblScreenTitle2);
            this.flpParameters.Controls.Add(this.pnlImage);
            this.flpParameters.Controls.Add(this.flpDrinkParams);
            this.flpParameters.Controls.Add(this.lblScreenFooter1);
            this.flpParameters.Controls.Add(this.lblScreenFooter2);
            this.flpParameters.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpParameters.Location = new System.Drawing.Point(0, 133);
            this.flpParameters.Margin = new System.Windows.Forms.Padding(0);
            this.flpParameters.Name = "flpParameters";
            this.flpParameters.Size = new System.Drawing.Size(955, 1201);
            this.flpParameters.TabIndex = 5;
            // 
            // lblScreenTitle2
            // 
            this.lblScreenTitle2.Font = new System.Drawing.Font("VAG Rounded", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle2.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle2.Location = new System.Drawing.Point(3, 0);
            this.lblScreenTitle2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblScreenTitle2.Name = "lblScreenTitle2";
            this.lblScreenTitle2.Size = new System.Drawing.Size(951, 28);
            this.lblScreenTitle2.TabIndex = 7;
            this.lblScreenTitle2.Text = "Title 2";
            this.lblScreenTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlImage
            // 
            this.pnlImage.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.pnlImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlImage.Location = new System.Drawing.Point(0, 48);
            this.pnlImage.Margin = new System.Windows.Forms.Padding(0, 10, 3, 10);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(955, 450);
            this.pnlImage.TabIndex = 10;
            // 
            // flpDrinkParams
            // 
            this.flpDrinkParams.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Drinks_Pop_up_Screen_White_Bg;
            this.flpDrinkParams.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpDrinkParams.Controls.Add(this.panelParameterSample);
            this.flpDrinkParams.Location = new System.Drawing.Point(32, 518);
            this.flpDrinkParams.Margin = new System.Windows.Forms.Padding(32, 10, 4, 5);
            this.flpDrinkParams.Name = "flpDrinkParams";
            this.flpDrinkParams.Size = new System.Drawing.Size(884, 403);
            this.flpDrinkParams.TabIndex = 0;
            // 
            // panelParameterSample
            // 
            this.panelParameterSample.BackColor = System.Drawing.Color.Transparent;
            this.panelParameterSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelParameterSample.Controls.Add(this.flpDisplayTextsSample);
            this.panelParameterSample.Controls.Add(this.lblComboDispSample);
            this.panelParameterSample.Controls.Add(this.cmbParameter);
            this.panelParameterSample.Location = new System.Drawing.Point(15, 0);
            this.panelParameterSample.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.panelParameterSample.Name = "panelParameterSample";
            this.panelParameterSample.Size = new System.Drawing.Size(869, 66);
            this.panelParameterSample.TabIndex = 4;
            // 
            // flpDisplayTextsSample
            // 
            this.flpDisplayTextsSample.Controls.Add(this.lblDisplayText1Sample);
            this.flpDisplayTextsSample.Controls.Add(this.lblDisplayText2Sample);
            this.flpDisplayTextsSample.Location = new System.Drawing.Point(177, 14);
            this.flpDisplayTextsSample.Name = "flpDisplayTextsSample";
            this.flpDisplayTextsSample.Size = new System.Drawing.Size(689, 45);
            this.flpDisplayTextsSample.TabIndex = 7;
            // 
            // lblDisplayText1Sample
            // 
            this.lblDisplayText1Sample.AutoSize = true;
            this.lblDisplayText1Sample.Font = new System.Drawing.Font("VAG Rounded", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText1Sample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDisplayText1Sample.Location = new System.Drawing.Point(0, 0);
            this.lblDisplayText1Sample.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblDisplayText1Sample.Name = "lblDisplayText1Sample";
            this.lblDisplayText1Sample.Size = new System.Drawing.Size(310, 41);
            this.lblDisplayText1Sample.TabIndex = 2;
            this.lblDisplayText1Sample.Text = "K-Cup Coffee $1.99";
            this.lblDisplayText1Sample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDisplayText2Sample
            // 
            this.lblDisplayText2Sample.AutoSize = true;
            this.lblDisplayText2Sample.Font = new System.Drawing.Font("VAG Rounded", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText2Sample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDisplayText2Sample.Location = new System.Drawing.Point(314, 16);
            this.lblDisplayText2Sample.Margin = new System.Windows.Forms.Padding(0, 16, 4, 0);
            this.lblDisplayText2Sample.Name = "lblDisplayText2Sample";
            this.lblDisplayText2Sample.Size = new System.Drawing.Size(258, 22);
            this.lblDisplayText2Sample.TabIndex = 6;
            this.lblDisplayText2Sample.Text = "Choose your flavor at counter";
            this.lblDisplayText2Sample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbParameter
            // 
            this.cmbParameter.BackColor = System.Drawing.Color.White;
            this.cmbParameter.DropDownHeight = 125;
            this.cmbParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParameter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbParameter.Font = new System.Drawing.Font("VAG Rounded", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbParameter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.cmbParameter.FormattingEnabled = true;
            this.cmbParameter.IntegralHeight = false;
            this.cmbParameter.ItemHeight = 33;
            this.cmbParameter.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbParameter.Location = new System.Drawing.Point(71, 13);
            this.cmbParameter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbParameter.Name = "cmbParameter";
            this.cmbParameter.Size = new System.Drawing.Size(80, 41);
            this.cmbParameter.TabIndex = 3;
            // 
            // lblComboDispSample
            // 
            this.lblComboDispSample.Font = new System.Drawing.Font("VAG Rounded", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComboDispSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblComboDispSample.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Dropdown_Btn_1;
            this.lblComboDispSample.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblComboDispSample.Location = new System.Drawing.Point(5, 11);
            this.lblComboDispSample.Name = "lblComboDispSample";
            this.lblComboDispSample.Size = new System.Drawing.Size(152, 47);
            this.lblComboDispSample.TabIndex = 5;
            this.lblComboDispSample.Text = "0";
            this.lblComboDispSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScreenFooter1
            // 
            this.lblScreenFooter1.Font = new System.Drawing.Font("VAG Rounded", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenFooter1.ForeColor = System.Drawing.Color.White;
            this.lblScreenFooter1.Location = new System.Drawing.Point(40, 926);
            this.lblScreenFooter1.Margin = new System.Windows.Forms.Padding(40, 0, 3, 0);
            this.lblScreenFooter1.Name = "lblScreenFooter1";
            this.lblScreenFooter1.Size = new System.Drawing.Size(873, 140);
            this.lblScreenFooter1.TabIndex = 8;
            this.lblScreenFooter1.Text = "Footer 1\r\nLine 2\r\nLine 3";
            this.lblScreenFooter1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScreenFooter2
            // 
            this.lblScreenFooter2.Font = new System.Drawing.Font("VAG Rounded", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenFooter2.ForeColor = System.Drawing.Color.White;
            this.lblScreenFooter2.Location = new System.Drawing.Point(225, 1086);
            this.lblScreenFooter2.Margin = new System.Windows.Forms.Padding(225, 20, 3, 20);
            this.lblScreenFooter2.Name = "lblScreenFooter2";
            this.lblScreenFooter2.Size = new System.Drawing.Size(500, 70);
            this.lblScreenFooter2.TabIndex = 9;
            this.lblScreenFooter2.Text = "Look for the Chuck E.\'s Express Ordering signs to pick up your cups.";
            this.lblScreenFooter2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmProductSaleDrinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 1652);
            this.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.Name = "frmProductSaleDrinks";
            this.Text = "frmProductSaleBasic";
            this.Load += new System.EventHandler(this.frmProductSaleDrinks_Load);
            this.panelBG.ResumeLayout(false);
            this.flpParameters.ResumeLayout(false);
            this.flpDrinkParams.ResumeLayout(false);
            this.panelParameterSample.ResumeLayout(false);
            this.flpDisplayTextsSample.ResumeLayout(false);
            this.flpDisplayTextsSample.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel flpParameters;
        private System.Windows.Forms.FlowLayoutPanel flpDrinkParams;
        internal System.Windows.Forms.Label lblDisplayText1Sample;
        public System.Windows.Forms.Panel panelParameterSample;
        private System.Windows.Forms.Label lblComboDispSample;
        private System.Windows.Forms.ComboBox cmbParameter;
        internal System.Windows.Forms.Label lblDisplayText2Sample;
        private System.Windows.Forms.FlowLayoutPanel flpDisplayTextsSample;
        internal System.Windows.Forms.Label lblScreenTitle2;
        internal System.Windows.Forms.Label lblScreenFooter1;
        internal System.Windows.Forms.Label lblScreenFooter2;
        private System.Windows.Forms.Panel pnlImage;
    }
}