namespace Parafait_Kiosk
{
    partial class frmLinkRelatedCustomer
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
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblRelatedCustomer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbRelation = new System.Windows.Forms.ComboBox();
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblCustomerValue = new System.Windows.Forms.Label();
            this.lblRelatedCustomerValue = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.White;
            this.lblCustomer.Location = new System.Drawing.Point(126, 322);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(812, 86);
            this.lblCustomer.TabIndex = 2;
            this.lblCustomer.Text = "Customer :";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRelatedCustomer
            // 
            this.lblRelatedCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblRelatedCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelatedCustomer.ForeColor = System.Drawing.Color.White;
            this.lblRelatedCustomer.Location = new System.Drawing.Point(136, 408);
            this.lblRelatedCustomer.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblRelatedCustomer.Name = "lblRelatedCustomer";
            this.lblRelatedCustomer.Size = new System.Drawing.Size(802, 86);
            this.lblRelatedCustomer.TabIndex = 3;
            this.lblRelatedCustomer.Text = "Related Customer :";
            this.lblRelatedCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(134, 508);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(804, 86);
            this.label2.TabIndex = 4;
            this.label2.Text = "Relationship Type :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(995, 851);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(250, 125);
            this.btnSave.TabIndex = 1029;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(667, 851);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(250, 125);
            this.btnCancel.TabIndex = 1030;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.cmbRelation);
            this.panel2.Location = new System.Drawing.Point(964, 510);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(325, 84);
            this.panel2.TabIndex = 1040;
            // 
            // cmbRelation
            // 
            this.cmbRelation.BackColor = System.Drawing.Color.White;
            this.cmbRelation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRelation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRelation.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRelation.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbRelation.FormattingEnabled = true;
            this.cmbRelation.Location = new System.Drawing.Point(13, 16);
            this.cmbRelation.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbRelation.Name = "cmbRelation";
            this.cmbRelation.Size = new System.Drawing.Size(304, 56);
            this.cmbRelation.TabIndex = 1037;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1030);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 1041;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // lblCustomerValue
            // 
            this.lblCustomerValue.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerValue.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerValue.ForeColor = System.Drawing.Color.White;
            this.lblCustomerValue.Location = new System.Drawing.Point(969, 322);
            this.lblCustomerValue.Name = "lblCustomerValue";
            this.lblCustomerValue.Size = new System.Drawing.Size(772, 86);
            this.lblCustomerValue.TabIndex = 1042;
            this.lblCustomerValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRelatedCustomerValue
            // 
            this.lblRelatedCustomerValue.BackColor = System.Drawing.Color.Transparent;
            this.lblRelatedCustomerValue.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelatedCustomerValue.ForeColor = System.Drawing.Color.White;
            this.lblRelatedCustomerValue.Location = new System.Drawing.Point(969, 408);
            this.lblRelatedCustomerValue.Name = "lblRelatedCustomerValue";
            this.lblRelatedCustomerValue.Size = new System.Drawing.Size(783, 86);
            this.lblRelatedCustomerValue.TabIndex = 1043;
            this.lblRelatedCustomerValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmLinkRelatedCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.lblRelatedCustomerValue);
            this.Controls.Add(this.lblCustomerValue);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblRelatedCustomer);
            this.Controls.Add(this.lblCustomer);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmLinkRelatedCustomer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Link Related Customer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLinkRelatedCustomer_Closing);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblRelatedCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbRelation;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Label lblCustomerValue;
        private System.Windows.Forms.Label lblRelatedCustomerValue;
    }
}