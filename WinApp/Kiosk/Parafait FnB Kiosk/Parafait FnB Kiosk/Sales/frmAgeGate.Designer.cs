namespace Parafait_FnB_Kiosk
{
    partial class frmAgeGate
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
            this.panelParameterSelection = new System.Windows.Forms.Panel();
            this.lblMonth = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDay = new System.Windows.Forms.Label();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblYear = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.panelConfirm = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.panelBG.SuspendLayout();
            this.flpChoiceParameter.SuspendLayout();
            this.panelParameterSelection.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelConfirm.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackColor = System.Drawing.Color.Transparent;
            this.panelBG.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.ProductSalePopUp3;
            this.panelBG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelBG.Controls.Add(this.lblScreenTitle);
            this.panelBG.Controls.Add(this.btnClose);
            this.panelBG.Controls.Add(this.flpChoiceParameter);
            this.panelBG.Controls.Add(this.panelConfirm);
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(955, 907);
            this.panelBG.TabIndex = 5;
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(145, 67);
            this.lblScreenTitle.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Size = new System.Drawing.Size(664, 166);
            this.lblScreenTitle.TabIndex = 5;
            this.lblScreenTitle.Text = "YOU MUST BE OF LEGAL DRINKING AGE TO ORDER THIS BEVERAGE";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Close_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            // 
            // flpChoiceParameter
            // 
            this.flpChoiceParameter.Controls.Add(this.panelParameterSelection);
            this.flpChoiceParameter.Controls.Add(this.panel1);
            this.flpChoiceParameter.Controls.Add(this.panel2);
            this.flpChoiceParameter.Location = new System.Drawing.Point(115, 236);
            this.flpChoiceParameter.Margin = new System.Windows.Forms.Padding(0, 5, 4, 5);
            this.flpChoiceParameter.Name = "flpChoiceParameter";
            this.flpChoiceParameter.Size = new System.Drawing.Size(717, 503);
            this.flpChoiceParameter.TabIndex = 2;
            // 
            // panelParameterSelection
            // 
            this.panelParameterSelection.BackColor = System.Drawing.Color.Transparent;
            this.panelParameterSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelParameterSelection.Controls.Add(this.lblMonth);
            this.panelParameterSelection.Controls.Add(this.cmbMonth);
            this.panelParameterSelection.Location = new System.Drawing.Point(3, 10);
            this.panelParameterSelection.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.panelParameterSelection.Name = "panelParameterSelection";
            this.panelParameterSelection.Size = new System.Drawing.Size(625, 153);
            this.panelParameterSelection.TabIndex = 5;
            // 
            // lblMonth
            // 
            this.lblMonth.Font = new System.Drawing.Font("Bango Pro", 56F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblMonth.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Dropdown_Btn_2;
            this.lblMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMonth.Location = new System.Drawing.Point(26, 7);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(578, 144);
            this.lblMonth.TabIndex = 5;
            this.lblMonth.Text = "Month";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbMonth
            // 
            this.cmbMonth.BackColor = System.Drawing.Color.White;
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMonth.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.ItemHeight = 115;
            this.cmbMonth.Location = new System.Drawing.Point(189, 21);
            this.cmbMonth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbMonth.MaxDropDownItems = 12;
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(256, 123);
            this.cmbMonth.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Controls.Add(this.lblDay);
            this.panel1.Controls.Add(this.cmbDay);
            this.panel1.Location = new System.Drawing.Point(3, 176);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 153);
            this.panel1.TabIndex = 6;
            // 
            // lblDay
            // 
            this.lblDay.Font = new System.Drawing.Font("Bango Pro", 56F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDay.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Dropdown_Btn_2;
            this.lblDay.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDay.Location = new System.Drawing.Point(26, 5);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(578, 144);
            this.lblDay.TabIndex = 5;
            this.lblDay.Text = "Day";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbDay
            // 
            this.cmbDay.BackColor = System.Drawing.Color.White;
            this.cmbDay.DropDownHeight = 1600;
            this.cmbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDay.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.IntegralHeight = false;
            this.cmbDay.ItemHeight = 115;
            this.cmbDay.Location = new System.Drawing.Point(189, 19);
            this.cmbDay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(256, 123);
            this.cmbDay.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.lblYear);
            this.panel2.Controls.Add(this.cmbYear);
            this.panel2.Location = new System.Drawing.Point(3, 342);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(625, 153);
            this.panel2.TabIndex = 7;
            // 
            // lblYear
            // 
            this.lblYear.Font = new System.Drawing.Font("Bango Pro", 56F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblYear.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Dropdown_Btn_2;
            this.lblYear.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblYear.Location = new System.Drawing.Point(26, 5);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(578, 144);
            this.lblYear.TabIndex = 5;
            this.lblYear.Text = "Year";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbYear
            // 
            this.cmbYear.BackColor = System.Drawing.Color.White;
            this.cmbYear.DropDownHeight = 1600;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbYear.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.IntegralHeight = false;
            this.cmbYear.ItemHeight = 115;
            this.cmbYear.Location = new System.Drawing.Point(189, 20);
            this.cmbYear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(256, 123);
            this.cmbYear.TabIndex = 3;
            // 
            // panelConfirm
            // 
            this.panelConfirm.Controls.Add(this.btnConfirm);
            this.panelConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConfirm.Location = new System.Drawing.Point(0, 742);
            this.panelConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelConfirm.Name = "panelConfirm";
            this.panelConfirm.Size = new System.Drawing.Size(955, 165);
            this.panelConfirm.TabIndex = 4;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Green_Btn;
            this.btnConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnConfirm.Location = new System.Drawing.Point(304, 40);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(346, 68);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "CONFIRM";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // frmAgeGate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Turquoise;
            this.ClientSize = new System.Drawing.Size(955, 907);
            this.Controls.Add(this.panelBG);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "frmAgeGate";
            this.Text = "BaseFormProductSale";
            this.TransparencyKey = System.Drawing.Color.Turquoise;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.frmAgeGate_Load);
            this.panelBG.ResumeLayout(false);
            this.flpChoiceParameter.ResumeLayout(false);
            this.panelParameterSelection.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panelConfirm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelBG;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.FlowLayoutPanel flpChoiceParameter;
        public System.Windows.Forms.Panel panelConfirm;
        private System.Windows.Forms.Button btnConfirm;
        public System.Windows.Forms.Panel panelParameterSelection;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.ComboBox cmbMonth;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.ComboBox cmbDay;
        public System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label lblScreenTitle;
    }
}