using System;

namespace Parafait_Kiosk
{
    partial class BaseFormKiosk
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
            //this.btnPrev = new System.Windows.Forms.Button();
            //this.btnCancel = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            //btnHome
            //
            this.btnHome.TabIndex = 162;
            //generic property
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(31, 28);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(153, 151);
            this.btnHome.Text = "GO HOME";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            //frmCardTransaction and frmCashTransaction defines their own btnHome.Click click event.
            //btnHome.margin is defined for frmCashTransaction,frnCardTransaction,frmPaymentMode,frmChooseProduct




            //// 
            //// btnPrev
            //// 
            //this.btnPrev.Location = new System.Drawing.Point(13, 226);
            //this.btnPrev.Size = new System.Drawing.Size(75, 23);
            //this.btnPrev.TabIndex = 0;
            ////Set generic properties
            //this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            //this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            //this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            //this.btnPrev.FlatAppearance.BorderSize = 0;
            //this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            //this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnPrev.ForeColor = System.Drawing.Color.White;
            //this.btnPrev.Name = "btnPrev";
            //this.btnPrev.Text = "Back";
            //this.btnPrev.UseVisualStyleBackColor = false;
            ////BackgroundImage value needs to be changed in frmFAQ 
            //this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            ////font needs to be changed in frmFAQ, frmPaymentMode, frmTransferFrom, frmCustomer
            //this.btnPrev.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ////Click handler needs to be changed in frmFAQ
            //this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            ////frmCustomer calls btnPrev as btnClose it needs to be replaced. 
            ////following properties/evernt needs to be set in each child form 
            ////this.btnPrev.Anchor , this.btnPrev.Location, this.btnPrev.MouseDown+, this.btnPrev.MouseUp+, this.btnPrev.Size,this.btnPrev.TabIndex
            //// this.btnPrev.FlatAppearance.CheckedBackColor – needs to be set in frmCustomer
            //this.btnPrev.Visible = true;
            //// 
            //// btnCancel
            //// 
            //this.btnCancel.Location = new System.Drawing.Point(13, 326);
            //this.btnCancel.Size = new System.Drawing.Size(75, 23);
            //this.btnCancel.TabIndex = 1;
            ////Set generic properties
            //this.btnCancel.Visible = false;
            //this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            //this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            //this.btnCancel.FlatAppearance.BorderSize = 0;
            //this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            //this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnCancel.ForeColor = System.Drawing.Color.White;
            //this.btnCancel.Name = "BtnCancel";
            //this.btnCancel.Text = "Cancel";
            //this.btnCancel.UseVisualStyleBackColor = false;
            //this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            ////specific font for for frmCustomerDashboard, frmAdmin, frmPaymentMode, FrmRegisgterTnC
            //this.btnCancel.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ////specifi bordercolor for frmCustomerDashboard, frmCardTransaction, FrmRegisgterTnC, frmAgeGate
            //this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            ////specifi BackgroundImage for frmCustomerDashboard, frmAdmin, FrmRegisgterTnC
            //this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            ////set ImageAlign for frmCustomerDashboard,FrmRegisgterTnC,frmAgeGate
            ////set Margin for frmAdmin
            ////set TabStop for frmCardTransaction
            ////following properties/evernt needs to be set in each child form 
            ////this.btnCancel.Anchor , this.btnCancel.Location, this.btnCancel.Size,this.btnCancel.TabIndex
            //// FrmRegisgterTnC,frmAgeGate calls Cancel btn as btnNo. Need to change
            // 
            // BaseFormKiosk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnHome);
            this.Name = "BaseFormKiosk";
            this.Text = "BaseFormKiosk";
            this.ResumeLayout(false);

        }
        
        #endregion

        public System.Windows.Forms.Button btnHome;
    }
}