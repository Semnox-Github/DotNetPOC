namespace Semnox.Parafait.Device.PaymentGateway.Mercury
{
    /// <summary>
    /// Desginer Class for  frmStatus
    /// </summary>
    partial class frmStatus
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
            this.lbl_Status_Errors = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Status_Errors
            // 
            this.lbl_Status_Errors.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Status_Errors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Status_Errors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Status_Errors.Location = new System.Drawing.Point(0, 3);
            this.lbl_Status_Errors.Name = "lbl_Status_Errors";
            this.lbl_Status_Errors.Size = new System.Drawing.Size(666, 71);
            this.lbl_Status_Errors.TabIndex = 3;
            this.lbl_Status_Errors.Text = "Status";
            this.lbl_Status_Errors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Enabled = false;
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.Black;
            this.btn_Cancel.Location = new System.Drawing.Point(255, 93);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(160, 43);
            this.btn_Cancel.TabIndex = 65;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // frmStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 149);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.lbl_Status_Errors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmStatus";
            this.Text = "Status";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Status_Errors;
        private System.Windows.Forms.Button btn_Cancel;
    }
}