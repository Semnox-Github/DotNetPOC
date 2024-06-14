namespace Parafait_POS
{
    partial class frmFiscalReports
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
            this.btnSample = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSample
            // 
            this.btnSample.BackColor = System.Drawing.Color.Transparent;
            this.btnSample.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSample.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSample.ForeColor = System.Drawing.Color.White;
            this.btnSample.Location = new System.Drawing.Point(44, 12);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(146, 48);
            this.btnSample.TabIndex = 0;
            this.btnSample.Text = "SampleButton";
            this.btnSample.UseVisualStyleBackColor = false;
            this.btnSample.Visible = false;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // frmFiscalReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(240, 58);
            this.Controls.Add(this.btnSample);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmFiscalReports";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fiscal Printer Reports";
            this.Load += new System.EventHandler(this.frmFiscalReports_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSample;
    }
}