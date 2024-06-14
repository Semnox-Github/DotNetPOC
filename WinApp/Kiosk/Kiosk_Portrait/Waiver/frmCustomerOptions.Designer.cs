namespace Parafait_Kiosk
{
    partial class frmCustomerOptions
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
            this.btnExistingCustomer = new System.Windows.Forms.Button();
            this.btnNewRegistration = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            //this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnExistingCustomer
            // 
            this.btnExistingCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnExistingCustomer.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Registered_Customer_Big;
            this.btnExistingCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExistingCustomer.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnExistingCustomer.FlatAppearance.BorderSize = 0;
            this.btnExistingCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExistingCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExistingCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExistingCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F);
            this.btnExistingCustomer.ForeColor = System.Drawing.Color.White;
            this.btnExistingCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExistingCustomer.Location = new System.Drawing.Point(89, 631);
            this.btnExistingCustomer.Name = "btnExistingCustomer";
            this.btnExistingCustomer.Size = new System.Drawing.Size(894, 319);
            this.btnExistingCustomer.TabIndex = 1;
            this.btnExistingCustomer.Text = "Already Registered";
            this.btnExistingCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExistingCustomer.UseVisualStyleBackColor = false;
            this.btnExistingCustomer.Click += new System.EventHandler(this.btnRegCust_Click);
            // 
            // btnNewRegistration
            // 
            this.btnNewRegistration.BackColor = System.Drawing.Color.Transparent;
            this.btnNewRegistration.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.register_pass_big;
            this.btnNewRegistration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNewRegistration.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnNewRegistration.FlatAppearance.BorderSize = 0;
            this.btnNewRegistration.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewRegistration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewRegistration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewRegistration.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewRegistration.ForeColor = System.Drawing.Color.White;
            this.btnNewRegistration.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewRegistration.Location = new System.Drawing.Point(89, 344);
            this.btnNewRegistration.Name = "btnNewRegistration";
            this.btnNewRegistration.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNewRegistration.Size = new System.Drawing.Size(894, 319);
            this.btnNewRegistration.TabIndex = 2;
            this.btnNewRegistration.Text = "New Customer";
            this.btnNewRegistration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewRegistration.UseVisualStyleBackColor = false;
            this.btnNewRegistration.Click += new System.EventHandler(this.btnNewCust_Click);
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1870);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
            this.txtMessage.TabIndex = 3;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(377, 1670);
            this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(405, 130);
            this.btnCancel.TabIndex = 1056;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            //this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmCustomerOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnExistingCustomer);
            this.Controls.Add(this.btnNewRegistration);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmCustomerOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Option";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCustomerOptions_Load);
            this.Controls.SetChildIndex(this.btnNewRegistration, 0);
            this.Controls.SetChildIndex(this.btnExistingCustomer, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnNewRegistration;
        private System.Windows.Forms.Button btnExistingCustomer;
        private System.Windows.Forms.Button txtMessage;
        //private System.Windows.Forms.Button btnCancel;
    }
}