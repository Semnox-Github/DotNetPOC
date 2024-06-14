namespace Parafait_Kiosk
{
    partial class frmLogMessage
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
            this.btnIgnore = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnIgnore
            // 
            this.btnIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIgnore.BackColor = System.Drawing.Color.MintCream;
            this.btnIgnore.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Cancel_copy;
            this.btnIgnore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnIgnore.FlatAppearance.BorderColor = System.Drawing.Color.MintCream;
            this.btnIgnore.FlatAppearance.BorderSize = 0;
            this.btnIgnore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.IndianRed;
            this.btnIgnore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.IndianRed;
            this.btnIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIgnore.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnore.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnIgnore.Location = new System.Drawing.Point(655, 267);
            this.btnIgnore.Margin = new System.Windows.Forms.Padding(4);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(27, 24);
            this.btnIgnore.TabIndex = 0;
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.MintCream;
            this.txtLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtLog.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(5, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(678, 289);
            this.txtLog.TabIndex = 1;
            // 
            // frmLogMessage
            // 
            this.AcceptButton = this.btnIgnore;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.ClientSize = new System.Drawing.Size(688, 299);
            this.Controls.Add(this.btnIgnore);
            this.Controls.Add(this.txtLog);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmLogMessage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLogMessage";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.TextBox txtLog;
    }
}