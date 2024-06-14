namespace Semnox.Parafait.Customer
{
    partial class CustomerDetailUITestForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLookup = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnRelationship = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(105, 593);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLookup
            // 
            this.btnLookup.Location = new System.Drawing.Point(216, 593);
            this.btnLookup.Name = "btnLookup";
            this.btnLookup.Size = new System.Drawing.Size(75, 23);
            this.btnLookup.TabIndex = 1;
            this.btnLookup.Text = "Lookup";
            this.btnLookup.UseVisualStyleBackColor = true;
            this.btnLookup.Click += new System.EventHandler(this.btnLookup_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(315, 593);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.TabIndex = 2;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnRelationship
            // 
            this.btnRelationship.Location = new System.Drawing.Point(426, 593);
            this.btnRelationship.Name = "btnRelationship";
            this.btnRelationship.Size = new System.Drawing.Size(75, 23);
            this.btnRelationship.TabIndex = 3;
            this.btnRelationship.Text = "Relationship";
            this.btnRelationship.UseVisualStyleBackColor = true;
            this.btnRelationship.Click += new System.EventHandler(this.btnRelationship_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 570);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "label1";
            // 
            // CustomerDetailUITestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 628);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnRelationship);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.btnLookup);
            this.Controls.Add(this.btnSave);
            this.Name = "CustomerDetailUITestForm";
            this.Text = "CustomerDetailUITestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLookup;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnRelationship;
        private System.Windows.Forms.Label lblStatus;
    }
}