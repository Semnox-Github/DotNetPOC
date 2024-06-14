namespace Semnox.Parafait.Device.Lockers
{
    partial class frmMainUI
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnZonePicker = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(63, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "Event";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnZonePicker
            // 
            this.btnZonePicker.Location = new System.Drawing.Point(63, 78);
            this.btnZonePicker.Name = "btnZonePicker";
            this.btnZonePicker.Size = new System.Drawing.Size(124, 30);
            this.btnZonePicker.TabIndex = 5;
            this.btnZonePicker.Text = "Zone Picker";
            this.btnZonePicker.UseVisualStyleBackColor = true;
            this.btnZonePicker.Click += new System.EventHandler(this.btnZonePicker_Click);
            // 
            // frmMainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnZonePicker);
            this.Controls.Add(this.button1);
            this.Name = "frmMainUI";
            this.Text = "main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnZonePicker;
    }
}