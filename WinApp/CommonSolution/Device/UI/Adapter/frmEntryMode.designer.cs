namespace Semnox.Parafait.Device.PaymentGateway.AdapterUI
{
    /// <summary>
    /// Desginer Class for frmEntryMode
    /// </summary>
    partial class frmEntryMode
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
            this.gpEntryMode = new System.Windows.Forms.GroupBox();
            this.rbtnKeyed = new System.Windows.Forms.RadioButton();
            this.rbtnSwiped = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gpEntryMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpEntryMode
            // 
            this.gpEntryMode.Controls.Add(this.rbtnKeyed);
            this.gpEntryMode.Controls.Add(this.rbtnSwiped);
            this.gpEntryMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpEntryMode.Location = new System.Drawing.Point(12, 12);
            this.gpEntryMode.Name = "gpEntryMode";
            this.gpEntryMode.Size = new System.Drawing.Size(386, 101);
            this.gpEntryMode.TabIndex = 0;
            this.gpEntryMode.TabStop = false;
            this.gpEntryMode.Text = "Please select entry mode.";
            // 
            // rbtnKeyed
            // 
            this.rbtnKeyed.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnKeyed.Location = new System.Drawing.Point(232, 36);
            this.rbtnKeyed.Name = "rbtnKeyed";
            this.rbtnKeyed.Size = new System.Drawing.Size(139, 45);
            this.rbtnKeyed.TabIndex = 1;
            this.rbtnKeyed.Text = "Keyed";
            this.rbtnKeyed.UseVisualStyleBackColor = true;
            // 
            // rbtnSwiped
            // 
            this.rbtnSwiped.Checked = true;
            this.rbtnSwiped.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSwiped.Location = new System.Drawing.Point(53, 36);
            this.rbtnSwiped.Name = "rbtnSwiped";
            this.rbtnSwiped.Size = new System.Drawing.Size(139, 45);
            this.rbtnSwiped.TabIndex = 0;
            this.rbtnSwiped.TabStop = true;
            this.rbtnSwiped.Text = "Swiped";
            this.rbtnSwiped.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(223, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(142, 46);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(44, 130);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(142, 46);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmEntryMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(412, 190);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gpEntryMode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEntryMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entry Mode Selection";
            this.gpEntryMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpEntryMode;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbtnKeyed;
        private System.Windows.Forms.RadioButton rbtnSwiped;
        private System.Windows.Forms.Button btnOK;
    }
}