namespace Semnox.Core.GenericUtilities
{
    partial class BigHorizontalScrollBarView
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
            this.SuspendLayout();
            // 
            // btnLeft
            // 
            this.btnLeft.BackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Scroll_Left_Button;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.Size = new System.Drawing.Size(63, 63);
            // 
            // btnRight
            // 
            this.btnRight.BackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Scroll_Right_Button;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.Location = new System.Drawing.Point(99, 0);
            this.btnRight.Size = new System.Drawing.Size(63, 63);
            // 
            // BigHorizontalScrollBarView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "BigHorizontalScrollBarView";
            this.Size = new System.Drawing.Size(162, 63);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
