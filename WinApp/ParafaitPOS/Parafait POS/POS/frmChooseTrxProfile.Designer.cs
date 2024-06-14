namespace Parafait_POS
{
    partial class frmChooseTrxProfile
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
            this.flpTrxProfiles = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTrxProfileDefault = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.flpOutside = new System.Windows.Forms.FlowLayoutPanel();
            this.flpTrxProfiles.SuspendLayout();
            this.flpOutside.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpTrxProfiles
            // 
            this.flpTrxProfiles.AutoSize = true;
            this.flpTrxProfiles.BackColor = System.Drawing.Color.Turquoise;
            this.flpTrxProfiles.Controls.Add(this.btnTrxProfileDefault);
            this.flpTrxProfiles.Location = new System.Drawing.Point(3, 3);
            this.flpTrxProfiles.Name = "flpTrxProfiles";
            this.flpTrxProfiles.Size = new System.Drawing.Size(121, 101);
            this.flpTrxProfiles.TabIndex = 0;
            // 
            // btnTrxProfileDefault
            // 
            this.btnTrxProfileDefault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTrxProfileDefault.BackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfileDefault.BackgroundImage = global::Parafait_POS.Properties.Resources.customer_button_pressed;
            this.btnTrxProfileDefault.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTrxProfileDefault.FlatAppearance.BorderColor = System.Drawing.Color.Azure;
            this.btnTrxProfileDefault.FlatAppearance.BorderSize = 0;
            this.btnTrxProfileDefault.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfileDefault.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfileDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrxProfileDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrxProfileDefault.ForeColor = System.Drawing.Color.White;
            this.btnTrxProfileDefault.Location = new System.Drawing.Point(3, 8);
            this.btnTrxProfileDefault.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.btnTrxProfileDefault.Name = "btnTrxProfileDefault";
            this.btnTrxProfileDefault.Size = new System.Drawing.Size(108, 83);
            this.btnTrxProfileDefault.TabIndex = 1;
            this.btnTrxProfileDefault.Tag = "-1";
            this.btnTrxProfileDefault.Text = "None";
            this.btnTrxProfileDefault.UseVisualStyleBackColor = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.buttonCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(170, 199);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(155, 55);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // flpOutside
            // 
            this.flpOutside.BackColor = System.Drawing.Color.Turquoise;
            this.flpOutside.Controls.Add(this.flpTrxProfiles);
            this.flpOutside.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpOutside.Location = new System.Drawing.Point(0, 0);
            this.flpOutside.Name = "flpOutside";
            this.flpOutside.Size = new System.Drawing.Size(497, 192);
            this.flpOutside.TabIndex = 5;
            // 
            // frmChooseTrxProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSeaGreen;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(497, 261);
            this.Controls.Add(this.flpOutside);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChooseTrxProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Dine-In / Take-Out Options";
            this.flpTrxProfiles.ResumeLayout(false);
            this.flpOutside.ResumeLayout(false);
            this.flpOutside.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpTrxProfiles;
        private System.Windows.Forms.Button btnTrxProfileDefault;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.FlowLayoutPanel flpOutside;
    }
}