namespace Parafait_FnB_Kiosk
{
    partial class frmProductSaleSubs
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
            this.flpSideParameter = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDisplayText1 = new System.Windows.Forms.Label();
            this.panelSideSelection = new System.Windows.Forms.Panel();
            this.cmbSideParameter = new System.Windows.Forms.ComboBox();
            this.lblSideParam = new System.Windows.Forms.Label();
            this.lblDisplayText2 = new System.Windows.Forms.Label();
            this.lblScreenTitle2 = new System.Windows.Forms.Label();
            this.flpSubParams = new System.Windows.Forms.FlowLayoutPanel();
            this.panelParameterSample = new System.Windows.Forms.Panel();
            this.flpDisplayTextsSample = new System.Windows.Forms.FlowLayoutPanel();
            this.chkParamSample = new System.Windows.Forms.CheckBox();
            this.lblDisplayText1Sample = new System.Windows.Forms.Label();
            this.cmbParameter = new System.Windows.Forms.ComboBox();
            this.lblComboDispSample = new System.Windows.Forms.Label();
            this.lblScreenFooter1 = new System.Windows.Forms.Label();
            this.panelBG.SuspendLayout();
            this.flpParameters.SuspendLayout();
            this.flpSideParameter.SuspendLayout();
            this.panelSideSelection.SuspendLayout();
            this.flpSubParams.SuspendLayout();
            this.panelParameterSample.SuspendLayout();
            this.flpDisplayTextsSample.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Drinks_Pop_up_Screen_Purple_Bg;
            this.panelBG.Controls.Add(this.flpParameters);
            this.panelBG.Size = new System.Drawing.Size(955, 1088);
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
            this.panelAddItem.Location = new System.Drawing.Point(0, 783);
            // 
            // flpParameters
            // 
            this.flpParameters.Controls.Add(this.flpSideParameter);
            this.flpParameters.Controls.Add(this.lblScreenTitle2);
            this.flpParameters.Controls.Add(this.flpSubParams);
            this.flpParameters.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpParameters.Location = new System.Drawing.Point(0, 135);
            this.flpParameters.Margin = new System.Windows.Forms.Padding(0);
            this.flpParameters.Name = "flpParameters";
            this.flpParameters.Size = new System.Drawing.Size(955, 790);
            this.flpParameters.TabIndex = 5;
            // 
            // flpSideParameter
            // 
            this.flpSideParameter.Controls.Add(this.lblDisplayText1);
            this.flpSideParameter.Controls.Add(this.panelSideSelection);
            this.flpSideParameter.Controls.Add(this.lblDisplayText2);
            this.flpSideParameter.Location = new System.Drawing.Point(0, 5);
            this.flpSideParameter.Margin = new System.Windows.Forms.Padding(0, 5, 4, 5);
            this.flpSideParameter.Name = "flpSideParameter";
            this.flpSideParameter.Size = new System.Drawing.Size(955, 160);
            this.flpSideParameter.TabIndex = 0;
            // 
            // lblDisplayText1
            // 
            this.lblDisplayText1.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblDisplayText1.Location = new System.Drawing.Point(0, 0);
            this.lblDisplayText1.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblDisplayText1.Name = "lblDisplayText1";
            this.lblDisplayText1.Size = new System.Drawing.Size(955, 42);
            this.lblDisplayText1.TabIndex = 2;
            this.lblDisplayText1.Text = "Parameter Display Text 1";
            this.lblDisplayText1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSideSelection
            // 
            this.panelSideSelection.BackColor = System.Drawing.Color.Transparent;
            this.panelSideSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelSideSelection.Controls.Add(this.cmbSideParameter);
            this.panelSideSelection.Controls.Add(this.lblSideParam);
            this.panelSideSelection.Location = new System.Drawing.Point(130, 47);
            this.panelSideSelection.Margin = new System.Windows.Forms.Padding(130, 5, 4, 5);
            this.panelSideSelection.Name = "panelSideSelection";
            this.panelSideSelection.Size = new System.Drawing.Size(695, 74);
            this.panelSideSelection.TabIndex = 4;
            // 
            // cmbSideParameter
            // 
            this.cmbSideParameter.BackColor = System.Drawing.Color.White;
            this.cmbSideParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSideParameter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSideParameter.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSideParameter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.cmbSideParameter.FormattingEnabled = true;
            this.cmbSideParameter.IntegralHeight = false;
            this.cmbSideParameter.ItemHeight = 44;
            this.cmbSideParameter.Location = new System.Drawing.Point(133, 15);
            this.cmbSideParameter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSideParameter.Name = "cmbSideParameter";
            this.cmbSideParameter.Size = new System.Drawing.Size(404, 52);
            this.cmbSideParameter.TabIndex = 3;
            // 
            // lblSideParam
            // 
            this.lblSideParam.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSideParam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblSideParam.Image = global::Parafait_FnB_Kiosk.Properties.Resources.White_Drop_Down;
            this.lblSideParam.Location = new System.Drawing.Point(97, 4);
            this.lblSideParam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSideParam.Name = "lblSideParam";
            this.lblSideParam.Size = new System.Drawing.Size(508, 68);
            this.lblSideParam.TabIndex = 5;
            this.lblSideParam.Text = "Selected Value";
            this.lblSideParam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDisplayText2
            // 
            this.lblDisplayText2.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText2.ForeColor = System.Drawing.Color.White;
            this.lblDisplayText2.Location = new System.Drawing.Point(0, 126);
            this.lblDisplayText2.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblDisplayText2.Name = "lblDisplayText2";
            this.lblDisplayText2.Size = new System.Drawing.Size(955, 27);
            this.lblDisplayText2.TabIndex = 5;
            this.lblDisplayText2.Text = "Parameter Display Text 2";
            this.lblDisplayText2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblScreenTitle2
            // 
            this.lblScreenTitle2.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblScreenTitle2.Location = new System.Drawing.Point(0, 170);
            this.lblScreenTitle2.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblScreenTitle2.Name = "lblScreenTitle2";
            this.lblScreenTitle2.Size = new System.Drawing.Size(955, 42);
            this.lblScreenTitle2.TabIndex = 9;
            this.lblScreenTitle2.Text = "Screen Title 2";
            this.lblScreenTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpSubParams
            // 
            this.flpSubParams.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Subs_Screen_Background;
            this.flpSubParams.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpSubParams.Controls.Add(this.panelParameterSample);
            this.flpSubParams.Controls.Add(this.lblScreenFooter1);
            this.flpSubParams.Location = new System.Drawing.Point(32, 217);
            this.flpSubParams.Margin = new System.Windows.Forms.Padding(32, 5, 4, 5);
            this.flpSubParams.Name = "flpSubParams";
            this.flpSubParams.Size = new System.Drawing.Size(884, 566);
            this.flpSubParams.TabIndex = 0;
            // 
            // panelParameterSample
            // 
            this.panelParameterSample.BackColor = System.Drawing.Color.Transparent;
            this.panelParameterSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelParameterSample.Controls.Add(this.flpDisplayTextsSample);
            this.panelParameterSample.Controls.Add(this.cmbParameter);
            this.panelParameterSample.Controls.Add(this.lblComboDispSample);
            this.panelParameterSample.Location = new System.Drawing.Point(15, 0);
            this.panelParameterSample.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.panelParameterSample.Name = "panelParameterSample";
            this.panelParameterSample.Size = new System.Drawing.Size(869, 60);
            this.panelParameterSample.TabIndex = 4;
            // 
            // flpDisplayTextsSample
            // 
            this.flpDisplayTextsSample.Controls.Add(this.chkParamSample);
            this.flpDisplayTextsSample.Controls.Add(this.lblDisplayText1Sample);
            this.flpDisplayTextsSample.Location = new System.Drawing.Point(38, 6);
            this.flpDisplayTextsSample.Name = "flpDisplayTextsSample";
            this.flpDisplayTextsSample.Size = new System.Drawing.Size(546, 44);
            this.flpDisplayTextsSample.TabIndex = 7;
            // 
            // chkParamSample
            // 
            this.chkParamSample.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkParamSample.FlatAppearance.BorderSize = 0;
            this.chkParamSample.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkParamSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkParamSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkParamSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkParamSample.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Check_Yes;
            this.chkParamSample.Location = new System.Drawing.Point(3, 3);
            this.chkParamSample.Name = "chkParamSample";
            this.chkParamSample.Size = new System.Drawing.Size(34, 34);
            this.chkParamSample.TabIndex = 7;
            this.chkParamSample.UseVisualStyleBackColor = true;
            // 
            // lblDisplayText1Sample
            // 
            this.lblDisplayText1Sample.AutoEllipsis = true;
            this.lblDisplayText1Sample.Font = new System.Drawing.Font("VAG Rounded", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText1Sample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDisplayText1Sample.Location = new System.Drawing.Point(55, 2);
            this.lblDisplayText1Sample.Margin = new System.Windows.Forms.Padding(15, 2, 0, 0);
            this.lblDisplayText1Sample.Name = "lblDisplayText1Sample";
            this.lblDisplayText1Sample.Size = new System.Drawing.Size(486, 37);
            this.lblDisplayText1Sample.TabIndex = 2;
            this.lblDisplayText1Sample.Text = "Mozarella Cheese";
            this.lblDisplayText1Sample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.cmbParameter.Location = new System.Drawing.Point(617, 10);
            this.cmbParameter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbParameter.Name = "cmbParameter";
            this.cmbParameter.Size = new System.Drawing.Size(213, 41);
            this.cmbParameter.TabIndex = 3;
            // 
            // lblComboDispSample
            // 
            this.lblComboDispSample.Font = new System.Drawing.Font("VAG Rounded", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComboDispSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblComboDispSample.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Dropdown_Btn_3;
            this.lblComboDispSample.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblComboDispSample.Location = new System.Drawing.Point(582, 6);
            this.lblComboDispSample.Name = "lblComboDispSample";
            this.lblComboDispSample.Size = new System.Drawing.Size(284, 48);
            this.lblComboDispSample.TabIndex = 5;
            this.lblComboDispSample.Text = "Extra";
            this.lblComboDispSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScreenFooter1
            // 
            this.lblScreenFooter1.AutoSize = true;
            this.lblScreenFooter1.Font = new System.Drawing.Font("VAG Rounded", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenFooter1.ForeColor = System.Drawing.Color.Gray;
            this.lblScreenFooter1.Location = new System.Drawing.Point(100, 60);
            this.lblScreenFooter1.Margin = new System.Windows.Forms.Padding(100, 0, 3, 0);
            this.lblScreenFooter1.Name = "lblScreenFooter1";
            this.lblScreenFooter1.Size = new System.Drawing.Size(467, 44);
            this.lblScreenFooter1.TabIndex = 8;
            this.lblScreenFooter1.Text = "Deselect to eliminate items.";
            this.lblScreenFooter1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmProductSaleSubs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 1088);
            this.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.Name = "frmProductSaleSubs";
            this.Text = "frmProductSaleBasic";
            this.Load += new System.EventHandler(this.frmProductSaleSubs_Load);
            this.panelBG.ResumeLayout(false);
            this.flpParameters.ResumeLayout(false);
            this.flpSideParameter.ResumeLayout(false);
            this.panelSideSelection.ResumeLayout(false);
            this.flpSubParams.ResumeLayout(false);
            this.flpSubParams.PerformLayout();
            this.panelParameterSample.ResumeLayout(false);
            this.flpDisplayTextsSample.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel flpParameters;
        private System.Windows.Forms.FlowLayoutPanel flpSubParams;
        internal System.Windows.Forms.Label lblDisplayText1Sample;
        public System.Windows.Forms.Panel panelParameterSample;
        private System.Windows.Forms.Label lblComboDispSample;
        private System.Windows.Forms.ComboBox cmbParameter;
        private System.Windows.Forms.FlowLayoutPanel flpDisplayTextsSample;
        internal System.Windows.Forms.Label lblScreenFooter1;
        private System.Windows.Forms.CheckBox chkParamSample;
        private System.Windows.Forms.FlowLayoutPanel flpSideParameter;
        internal System.Windows.Forms.Label lblDisplayText1;
        public System.Windows.Forms.Panel panelSideSelection;
        private System.Windows.Forms.Label lblSideParam;
        private System.Windows.Forms.ComboBox cmbSideParameter;
        internal System.Windows.Forms.Label lblDisplayText2;
        internal System.Windows.Forms.Label lblScreenTitle2;
    }
}