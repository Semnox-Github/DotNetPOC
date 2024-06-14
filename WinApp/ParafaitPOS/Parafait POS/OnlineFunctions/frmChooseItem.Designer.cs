namespace Parafait_POS
{
    partial class frmChooseItem
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
            this.flpItemControl = new System.Windows.Forms.FlowLayoutPanel();
            this.gpItemBox = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.gpItemBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpItemControl
            // 
            this.flpItemControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpItemControl.Location = new System.Drawing.Point(3, 18);
            this.flpItemControl.Name = "flpItemControl";
            this.flpItemControl.Size = new System.Drawing.Size(367, 240);
            this.flpItemControl.TabIndex = 0;
            // 
            // gpItemBox
            // 
            this.gpItemBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpItemBox.Controls.Add(this.flpItemControl);
            this.gpItemBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpItemBox.Location = new System.Drawing.Point(12, 12);
            this.gpItemBox.Name = "gpItemBox";
            this.gpItemBox.Size = new System.Drawing.Size(373, 261);
            this.gpItemBox.TabIndex = 1;
            this.gpItemBox.TabStop = false;
            this.gpItemBox.Text = "Select Item Quantity";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(21, 293);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(139, 32);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(180, 293);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 32);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.BackgroundImage = global::Parafait_POS.Properties.Resources.Keypad;
            this.btnShowNumPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowNumPad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnShowNumPad.Location = new System.Drawing.Point(349, 293);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 22;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // frmChooseItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 341);
            this.Controls.Add(this.btnShowNumPad);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gpItemBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChooseItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Item Quantity";
            this.gpItemBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpItemControl;
        private System.Windows.Forms.GroupBox gpItemBox;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnShowNumPad;
    }
}