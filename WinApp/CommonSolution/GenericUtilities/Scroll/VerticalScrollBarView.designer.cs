namespace Semnox.Core.GenericUtilities
{
    partial class VerticalScrollBarView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDownArrow
            // 
            this.btnDown.BackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Down_Button;
            this.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDown.FlatAppearance.BorderSize = 0;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Location = new System.Drawing.Point(0, 110);
            this.btnDown.Name = "btnDownArrow";
            this.btnDown.Size = new System.Drawing.Size(40, 40);
            this.btnDown.TabIndex = 1;
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnUpArrow
            // 
            this.btnUp.BackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Up_Button;
            this.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUp.FlatAppearance.BorderSize = 0;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Location = new System.Drawing.Point(0, 0);
            this.btnUp.Name = "btnUpArrow";
            this.btnUp.Size = new System.Drawing.Size(40, 40);
            this.btnUp.TabIndex = 0;
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // DataGridVerticalScrollBarView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Name = "DataGridVerticalScrollBarView";
            this.Size = new System.Drawing.Size(40, 150);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button btnUp;
        protected System.Windows.Forms.Button btnDown;
    }
}
