namespace Semnox.Core.GenericUtilities
{
    partial class WaitDialog
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
            this.label = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.lblProgressMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(12, 16);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(513, 30);
            this.label.TabIndex = 0;
            this.label.Text = "Processing.. Please wait..";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(12, 63);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(513, 23);
            this.pbProgress.TabIndex = 1;
            // 
            // lblProgressMessage
            // 
            this.lblProgressMessage.Font = new System.Drawing.Font("Arial", 12F);
            this.lblProgressMessage.Location = new System.Drawing.Point(15, 94);
            this.lblProgressMessage.Name = "lblProgressMessage";
            this.lblProgressMessage.Size = new System.Drawing.Size(510, 23);
            this.lblProgressMessage.TabIndex = 2;
            //
            // WaitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 120);
            this.Controls.Add(this.lblProgressMessage);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WaitDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label lblProgressMessage;
    }
}