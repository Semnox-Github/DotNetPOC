namespace Parafait_FnB_Kiosk
{
    partial class BaseFormProductSale
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
            this.panelBG = new System.Windows.Forms.Panel();
            this.lblScreenTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.flpChoiceParameter = new System.Windows.Forms.FlowLayoutPanel();
            this.lblChoiceDisplayText1 = new System.Windows.Forms.Label();
            this.panelChoiceParameter = new System.Windows.Forms.Panel();
            this.btnChoiceTrueSample = new System.Windows.Forms.RadioButton();
            this.btnChoiceFalseSample = new System.Windows.Forms.RadioButton();
            this.panelAddItem = new System.Windows.Forms.Panel();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblLiteralTotalAmount = new System.Windows.Forms.Label();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.panelBG.SuspendLayout();
            this.flpChoiceParameter.SuspendLayout();
            this.panelChoiceParameter.SuspendLayout();
            this.panelAddItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackColor = System.Drawing.Color.Transparent;
            this.panelBG.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.ProductSalePopUp1;
            this.panelBG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBG.Controls.Add(this.lblScreenTitle);
            this.panelBG.Controls.Add(this.btnClose);
            this.panelBG.Controls.Add(this.flpChoiceParameter);
            this.panelBG.Controls.Add(this.panelAddItem);
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(955, 663);
            this.panelBG.TabIndex = 5;
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(132, 85);
            this.lblScreenTitle.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Size = new System.Drawing.Size(680, 50);
            this.lblScreenTitle.TabIndex = 5;
            this.lblScreenTitle.Text = "Screen Title";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Close_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(819, 54);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 76);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // flpChoiceParameter
            // 
            this.flpChoiceParameter.Controls.Add(this.lblChoiceDisplayText1);
            this.flpChoiceParameter.Controls.Add(this.panelChoiceParameter);
            this.flpChoiceParameter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpChoiceParameter.Location = new System.Drawing.Point(0, 358);
            this.flpChoiceParameter.Margin = new System.Windows.Forms.Padding(0, 5, 4, 5);
            this.flpChoiceParameter.Name = "flpChoiceParameter";
            this.flpChoiceParameter.Size = new System.Drawing.Size(955, 140);
            this.flpChoiceParameter.TabIndex = 2;
            // 
            // lblChoiceDisplayText1
            // 
            this.lblChoiceDisplayText1.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChoiceDisplayText1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblChoiceDisplayText1.Location = new System.Drawing.Point(0, 0);
            this.lblChoiceDisplayText1.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblChoiceDisplayText1.Name = "lblChoiceDisplayText1";
            this.lblChoiceDisplayText1.Size = new System.Drawing.Size(955, 49);
            this.lblChoiceDisplayText1.TabIndex = 2;
            this.lblChoiceDisplayText1.Text = "Parameter Display Text 1";
            this.lblChoiceDisplayText1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelChoiceParameter
            // 
            this.panelChoiceParameter.Controls.Add(this.btnChoiceTrueSample);
            this.panelChoiceParameter.Controls.Add(this.btnChoiceFalseSample);
            this.panelChoiceParameter.Location = new System.Drawing.Point(3, 59);
            this.panelChoiceParameter.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.panelChoiceParameter.Name = "panelChoiceParameter";
            this.panelChoiceParameter.Size = new System.Drawing.Size(952, 77);
            this.panelChoiceParameter.TabIndex = 3;
            // 
            // btnChoiceTrueSample
            // 
            this.btnChoiceTrueSample.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnChoiceTrueSample.BackColor = System.Drawing.Color.Transparent;
            this.btnChoiceTrueSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnChoiceTrueSample.FlatAppearance.BorderSize = 0;
            this.btnChoiceTrueSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChoiceTrueSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChoiceTrueSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChoiceTrueSample.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChoiceTrueSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnChoiceTrueSample.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Green_Btn;
            this.btnChoiceTrueSample.Location = new System.Drawing.Point(483, 5);
            this.btnChoiceTrueSample.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChoiceTrueSample.Name = "btnChoiceTrueSample";
            this.btnChoiceTrueSample.Size = new System.Drawing.Size(346, 68);
            this.btnChoiceTrueSample.TabIndex = 4;
            this.btnChoiceTrueSample.Text = "Sure";
            this.btnChoiceTrueSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnChoiceTrueSample.UseVisualStyleBackColor = false;
            // 
            // btnChoiceFalseSample
            // 
            this.btnChoiceFalseSample.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnChoiceFalseSample.BackColor = System.Drawing.Color.Transparent;
            this.btnChoiceFalseSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnChoiceFalseSample.FlatAppearance.BorderSize = 0;
            this.btnChoiceFalseSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChoiceFalseSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChoiceFalseSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChoiceFalseSample.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChoiceFalseSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnChoiceFalseSample.Image = global::Parafait_FnB_Kiosk.Properties.Resources.White_Btn;
            this.btnChoiceFalseSample.Location = new System.Drawing.Point(107, 5);
            this.btnChoiceFalseSample.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChoiceFalseSample.Name = "btnChoiceFalseSample";
            this.btnChoiceFalseSample.Size = new System.Drawing.Size(346, 68);
            this.btnChoiceFalseSample.TabIndex = 3;
            this.btnChoiceFalseSample.Text = "No thanks";
            this.btnChoiceFalseSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnChoiceFalseSample.UseVisualStyleBackColor = false;
            // 
            // panelAddItem
            // 
            this.panelAddItem.Controls.Add(this.lblTotalAmount);
            this.panelAddItem.Controls.Add(this.lblLiteralTotalAmount);
            this.panelAddItem.Controls.Add(this.btnAddItem);
            this.panelAddItem.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAddItem.Location = new System.Drawing.Point(0, 498);
            this.panelAddItem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelAddItem.Name = "panelAddItem";
            this.panelAddItem.Size = new System.Drawing.Size(955, 165);
            this.panelAddItem.TabIndex = 4;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.ForeColor = System.Drawing.Color.White;
            this.lblTotalAmount.Location = new System.Drawing.Point(288, 52);
            this.lblTotalAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(190, 61);
            this.lblTotalAmount.TabIndex = 4;
            this.lblTotalAmount.Text = "$ 999.99";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLiteralTotalAmount
            // 
            this.lblLiteralTotalAmount.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLiteralTotalAmount.ForeColor = System.Drawing.Color.White;
            this.lblLiteralTotalAmount.Location = new System.Drawing.Point(16, 52);
            this.lblLiteralTotalAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLiteralTotalAmount.Name = "lblLiteralTotalAmount";
            this.lblLiteralTotalAmount.Size = new System.Drawing.Size(267, 61);
            this.lblLiteralTotalAmount.TabIndex = 3;
            this.lblLiteralTotalAmount.Text = "Item Total:";
            this.lblLiteralTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItem.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Green_Btn;
            this.btnAddItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAddItem.FlatAppearance.BorderSize = 0;
            this.btnAddItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnAddItem.Location = new System.Drawing.Point(486, 49);
            this.btnAddItem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(346, 68);
            this.btnAddItem.TabIndex = 2;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // BaseFormProductSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Turquoise;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(955, 663);
            this.Controls.Add(this.panelBG);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "BaseFormProductSale";
            this.Text = "BaseFormProductSale";
            this.TransparencyKey = System.Drawing.Color.Turquoise;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.panelBG.ResumeLayout(false);
            this.flpChoiceParameter.ResumeLayout(false);
            this.panelChoiceParameter.ResumeLayout(false);
            this.panelAddItem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label lblLiteralTotalAmount;
        internal System.Windows.Forms.Label lblTotalAmount;
        internal System.Windows.Forms.Label lblChoiceDisplayText1;
        private System.Windows.Forms.Panel panelChoiceParameter;
        private System.Windows.Forms.RadioButton btnChoiceTrueSample;
        private System.Windows.Forms.RadioButton btnChoiceFalseSample;
        private System.Windows.Forms.Button btnAddItem;
        public System.Windows.Forms.Panel panelBG;
        internal System.Windows.Forms.Label lblScreenTitle;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.FlowLayoutPanel flpChoiceParameter;
        public System.Windows.Forms.Panel panelAddItem;
    }
}