namespace Semnox.Parafait.User
{
    partial class AddAgentsUI
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
            this.grpAgentForm = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblPartner = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblMobileno = new System.Windows.Forms.Label();
            this.txtMobileNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.drpUserSelect = new System.Windows.Forms.ComboBox();
            this.txtCommission = new System.Windows.Forms.TextBox();
            this.drpPartnerSelect = new System.Windows.Forms.ComboBox();
            this.btnAgentUserSave = new System.Windows.Forms.Button();
            this.grpAgentForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpAgentForm
            // 
            this.grpAgentForm.Controls.Add(this.label1);
            this.grpAgentForm.Controls.Add(this.chkActive);
            this.grpAgentForm.Controls.Add(this.lblPartner);
            this.grpAgentForm.Controls.Add(this.lblName);
            this.grpAgentForm.Controls.Add(this.lblMobileno);
            this.grpAgentForm.Controls.Add(this.txtMobileNo);
            this.grpAgentForm.Controls.Add(this.label4);
            this.grpAgentForm.Controls.Add(this.drpUserSelect);
            this.grpAgentForm.Controls.Add(this.txtCommission);
            this.grpAgentForm.Controls.Add(this.drpPartnerSelect);
            this.grpAgentForm.Controls.Add(this.btnAgentUserSave);
            this.grpAgentForm.Location = new System.Drawing.Point(4, -2);
            this.grpAgentForm.Name = "grpAgentForm";
            this.grpAgentForm.Size = new System.Drawing.Size(419, 215);
            this.grpAgentForm.TabIndex = 22;
            this.grpAgentForm.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(71, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 36;
            this.label1.Text = "Active :";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(135, 152);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(15, 14);
            this.chkActive.TabIndex = 35;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // lblPartner
            // 
            this.lblPartner.AutoSize = true;
            this.lblPartner.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartner.Location = new System.Drawing.Point(60, 41);
            this.lblPartner.Name = "lblPartner";
            this.lblPartner.Size = new System.Drawing.Size(59, 15);
            this.lblPartner.TabIndex = 22;
            this.lblPartner.Text = "Partner  :";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(61, 81);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(58, 15);
            this.lblName.TabIndex = 32;
            this.lblName.Text = "Login ID :";
            // 
            // lblMobileno
            // 
            this.lblMobileno.AutoSize = true;
            this.lblMobileno.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMobileno.Location = new System.Drawing.Point(51, 118);
            this.lblMobileno.Name = "lblMobileno";
            this.lblMobileno.Size = new System.Drawing.Size(68, 15);
            this.lblMobileno.TabIndex = 24;
            this.lblMobileno.Text = "Mobile No :";
            // 
            // txtMobileNo
            // 
            this.txtMobileNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtMobileNo.Location = new System.Drawing.Point(135, 115);
            this.txtMobileNo.Name = "txtMobileNo";
            this.txtMobileNo.Size = new System.Drawing.Size(248, 21);
            this.txtMobileNo.TabIndex = 25;
            this.txtMobileNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowNumbersKeyPressed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 15);
            this.label4.TabIndex = 26;
            this.label4.Text = "Commission % :";
            this.label4.Visible = false;
            // 
            // drpUserSelect
            // 
            this.drpUserSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpUserSelect.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.drpUserSelect.FormattingEnabled = true;
            this.drpUserSelect.Location = new System.Drawing.Point(135, 73);
            this.drpUserSelect.Name = "drpUserSelect";
            this.drpUserSelect.Size = new System.Drawing.Size(248, 23);
            this.drpUserSelect.TabIndex = 29;
            // 
            // txtCommission
            // 
            this.txtCommission.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtCommission.Location = new System.Drawing.Point(135, 11);
            this.txtCommission.Name = "txtCommission";
            this.txtCommission.Size = new System.Drawing.Size(248, 21);
            this.txtCommission.TabIndex = 27;
            this.txtCommission.Visible = false;
            this.txtCommission.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CommissionKeyPressed);
            // 
            // drpPartnerSelect
            // 
            this.drpPartnerSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpPartnerSelect.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.drpPartnerSelect.FormattingEnabled = true;
            this.drpPartnerSelect.Location = new System.Drawing.Point(135, 38);
            this.drpPartnerSelect.Name = "drpPartnerSelect";
            this.drpPartnerSelect.Size = new System.Drawing.Size(248, 23);
            this.drpPartnerSelect.TabIndex = 28;
            // 
            // btnAgentUserSave
            // 
            this.btnAgentUserSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgentUserSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnAgentUserSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnAgentUserSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgentUserSave.Location = new System.Drawing.Point(303, 176);
            this.btnAgentUserSave.Name = "btnAgentUserSave";
            this.btnAgentUserSave.Size = new System.Drawing.Size(71, 23);
            this.btnAgentUserSave.TabIndex = 33;
            this.btnAgentUserSave.Text = "Save";
            this.btnAgentUserSave.UseVisualStyleBackColor = true;
            this.btnAgentUserSave.Click += new System.EventHandler(this.btnAgentUserSave_Click);
            // 
            // AddAgentsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(427, 215);
            this.Controls.Add(this.grpAgentForm);
            this.Name = "AddAgentsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agent Details";
            this.grpAgentForm.ResumeLayout(false);
            this.grpAgentForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpAgentForm;
        private System.Windows.Forms.Label lblPartner;
        private System.Windows.Forms.Button btnAgentUserSave;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblMobileno;
        private System.Windows.Forms.TextBox txtMobileNo;
        private System.Windows.Forms.ComboBox drpUserSelect;
        private System.Windows.Forms.TextBox txtCommission;
        private System.Windows.Forms.ComboBox drpPartnerSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkActive;

    }
}