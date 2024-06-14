namespace Semnox.Core.GenericUtilities
{
    partial class BigVerticalScrollBarView
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
            // btnDownArrow
            //  
            this.btnDown.Size = new System.Drawing.Size(63, 63);
            this.btnDown.BackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Scroll_Down_Button;
            // 
            // btnUpArrow
            //  
            this.btnUp.Size = new System.Drawing.Size(63, 63);
            this.btnDown.BackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Scroll_Up_Button;
            // 
            // BigVerticalScrollBarView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "BigVerticalScrollBarView";
            this.Size = new System.Drawing.Size(63, 150);
            this.ResumeLayout(false);
        }

        #endregion
         
    } 
}
