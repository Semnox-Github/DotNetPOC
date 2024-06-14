namespace Parafait_FnB_Kiosk
{
    partial class frmProductSaleBasic
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
            this.flpComboParameterSample = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDisplayText1 = new System.Windows.Forms.Label();
            this.panelParameterSelection = new System.Windows.Forms.Panel();
            this.cmbParameter = new System.Windows.Forms.ComboBox();
            this.lblComboDispSample = new System.Windows.Forms.Label();
            this.lblDisplayText2 = new System.Windows.Forms.Label();
            this.panelBG.SuspendLayout();
            this.flpParameters.SuspendLayout();
            this.flpComboParameterSample.SuspendLayout();
            this.panelParameterSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.Controls.Add(this.flpParameters);
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
            this.panelAddItem.Location = new System.Drawing.Point(0, 358);
            // 
            // flpParameters
            // 
            this.flpParameters.Controls.Add(this.flpComboParameterSample);
            this.flpParameters.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpParameters.Location = new System.Drawing.Point(0, 132);
            this.flpParameters.Margin = new System.Windows.Forms.Padding(0);
            this.flpParameters.Name = "flpParameters";
            this.flpParameters.Size = new System.Drawing.Size(955, 198);
            this.flpParameters.TabIndex = 5;
            // 
            // flpComboParameterSample
            // 
            this.flpComboParameterSample.Controls.Add(this.lblDisplayText1);
            this.flpComboParameterSample.Controls.Add(this.panelParameterSelection);
            this.flpComboParameterSample.Controls.Add(this.lblDisplayText2);
            this.flpComboParameterSample.Location = new System.Drawing.Point(0, 5);
            this.flpComboParameterSample.Margin = new System.Windows.Forms.Padding(0, 5, 4, 5);
            this.flpComboParameterSample.Name = "flpComboParameterSample";
            this.flpComboParameterSample.Size = new System.Drawing.Size(955, 185);
            this.flpComboParameterSample.TabIndex = 0;
            // 
            // lblDisplayText1
            // 
            this.lblDisplayText1.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblDisplayText1.Location = new System.Drawing.Point(0, 0);
            this.lblDisplayText1.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblDisplayText1.Name = "lblDisplayText1";
            this.lblDisplayText1.Size = new System.Drawing.Size(955, 49);
            this.lblDisplayText1.TabIndex = 2;
            this.lblDisplayText1.Text = "Parameter Display Text 1";
            this.lblDisplayText1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelParameterSelection
            // 
            this.panelParameterSelection.BackColor = System.Drawing.Color.Transparent;
            this.panelParameterSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelParameterSelection.Controls.Add(this.cmbParameter);
            this.panelParameterSelection.Controls.Add(this.lblComboDispSample);
            this.panelParameterSelection.Location = new System.Drawing.Point(130, 54);
            this.panelParameterSelection.Margin = new System.Windows.Forms.Padding(130, 5, 4, 5);
            this.panelParameterSelection.Name = "panelParameterSelection";
            this.panelParameterSelection.Size = new System.Drawing.Size(695, 74);
            this.panelParameterSelection.TabIndex = 4;
            // 
            // cmbParameter
            // 
            this.cmbParameter.BackColor = System.Drawing.Color.White;
            this.cmbParameter.DropDownHeight = 125;
            this.cmbParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParameter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbParameter.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbParameter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.cmbParameter.FormattingEnabled = true;
            this.cmbParameter.IntegralHeight = false;
            this.cmbParameter.ItemHeight = 44;
            this.cmbParameter.Location = new System.Drawing.Point(117, 15);
            this.cmbParameter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbParameter.Name = "cmbParameter";
            this.cmbParameter.Size = new System.Drawing.Size(467, 52);
            this.cmbParameter.TabIndex = 3;
            // 
            // lblComboDispSample
            // 
            this.lblComboDispSample.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComboDispSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblComboDispSample.Image = global::Parafait_FnB_Kiosk.Properties.Resources.White_Drop_Down;
            this.lblComboDispSample.Location = new System.Drawing.Point(97, 4);
            this.lblComboDispSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblComboDispSample.Name = "lblComboDispSample";
            this.lblComboDispSample.Size = new System.Drawing.Size(508, 68);
            this.lblComboDispSample.TabIndex = 5;
            this.lblComboDispSample.Text = "Selected Value";
            this.lblComboDispSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDisplayText2
            // 
            this.lblDisplayText2.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText2.ForeColor = System.Drawing.Color.White;
            this.lblDisplayText2.Location = new System.Drawing.Point(0, 133);
            this.lblDisplayText2.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblDisplayText2.Name = "lblDisplayText2";
            this.lblDisplayText2.Size = new System.Drawing.Size(955, 50);
            this.lblDisplayText2.TabIndex = 5;
            this.lblDisplayText2.Text = "Parameter Display Text 2\r\nLine 2";
            this.lblDisplayText2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmProductSaleBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 663);
            this.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.Name = "frmProductSaleBasic";
            this.Text = "frmProductSaleBasic";
            this.Load += new System.EventHandler(this.frmProductSaleBasic_Load);
            this.panelBG.ResumeLayout(false);
            this.flpParameters.ResumeLayout(false);
            this.flpComboParameterSample.ResumeLayout(false);
            this.panelParameterSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel flpParameters;
        private System.Windows.Forms.FlowLayoutPanel flpComboParameterSample;
        internal System.Windows.Forms.Label lblDisplayText1;
        public System.Windows.Forms.Panel panelParameterSelection;
        private System.Windows.Forms.Label lblComboDispSample;
        private System.Windows.Forms.ComboBox cmbParameter;
        internal System.Windows.Forms.Label lblDisplayText2;
    }
}