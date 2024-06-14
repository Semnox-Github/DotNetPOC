namespace Parafait_POS.Reservation
{
    partial class frmBasicReservationInputs
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
            this.lblBookingName = new System.Windows.Forms.Label();
            this.lblGuestQty = new System.Windows.Forms.Label();
            this.lblFromTime = new System.Windows.Forms.Label();
            this.lblToTime = new System.Windows.Forms.Label();
            this.cmbFromTime = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.cmbToTime = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.txtBookingName = new System.Windows.Forms.TextBox();
            this.txtGuestQty = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.lblAvailableUnits = new System.Windows.Forms.Label();
            this.lblAvailableUnitValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblBookingName
            // 
            this.lblBookingName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookingName.Location = new System.Drawing.Point(14, 11);
            this.lblBookingName.Name = "lblBookingName";
            this.lblBookingName.Size = new System.Drawing.Size(131, 30);
            this.lblBookingName.TabIndex = 0;
            this.lblBookingName.Text = "Booking Name:";
            this.lblBookingName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGuestQty
            // 
            this.lblGuestQty.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuestQty.Location = new System.Drawing.Point(14, 46);
            this.lblGuestQty.Name = "lblGuestQty";
            this.lblGuestQty.Size = new System.Drawing.Size(131, 30);
            this.lblGuestQty.TabIndex = 1;
            this.lblGuestQty.Text = "Guest Quantity:";
            this.lblGuestQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromTime
            // 
            this.lblFromTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromTime.Location = new System.Drawing.Point(14, 117);
            this.lblFromTime.Name = "lblFromTime";
            this.lblFromTime.Size = new System.Drawing.Size(131, 30);
            this.lblFromTime.TabIndex = 2;
            this.lblFromTime.Text = "From Time:";
            this.lblFromTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToTime
            // 
            this.lblToTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToTime.Location = new System.Drawing.Point(14, 152);
            this.lblToTime.Name = "lblToTime";
            this.lblToTime.Size = new System.Drawing.Size(131, 30);
            this.lblToTime.TabIndex = 3;
            this.lblToTime.Text = "To Time:";
            this.lblToTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFromTime
            // 
            this.cmbFromTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFromTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFromTime.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromTime.FormattingEnabled = true;
            this.cmbFromTime.Location = new System.Drawing.Point(151, 116);
            this.cmbFromTime.Name = "cmbFromTime";
            this.cmbFromTime.Size = new System.Drawing.Size(116, 31);
            this.cmbFromTime.TabIndex = 3;
            this.cmbFromTime.SelectedIndexChanged += new System.EventHandler(this.cmbTime_SelectedIndexChanged);
            this.cmbFromTime.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // cmbToTime
            // 
            this.cmbToTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbToTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbToTime.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToTime.FormattingEnabled = true;
            this.cmbToTime.Location = new System.Drawing.Point(151, 152);
            this.cmbToTime.Name = "cmbToTime";
            this.cmbToTime.Size = new System.Drawing.Size(116, 31);
            this.cmbToTime.TabIndex = 4;
            this.cmbToTime.SelectedIndexChanged += new System.EventHandler(this.cmbTime_SelectedIndexChanged);
            this.cmbToTime.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // txtBookingName
            // 
            this.txtBookingName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBookingName.Location = new System.Drawing.Point(151, 11);
            this.txtBookingName.MaxLength = 50;
            this.txtBookingName.Name = "txtBookingName";
            this.txtBookingName.Size = new System.Drawing.Size(226, 30);
            this.txtBookingName.TabIndex = 1;
            this.txtBookingName.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // txtGuestQty
            // 
            this.txtGuestQty.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuestQty.Location = new System.Drawing.Point(151, 46);
            this.txtGuestQty.MaxLength = 5;
            this.txtGuestQty.Name = "txtGuestQty";
            this.txtGuestQty.Size = new System.Drawing.Size(70, 30);
            this.txtGuestQty.TabIndex = 2;
            this.txtGuestQty.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(221, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 34);
            this.btnCancel.TabIndex = 122;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.ForeColor = System.Drawing.Color.White;
            this.btnOkay.Location = new System.Drawing.Point(78, 192);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(104, 36);
            this.btnOkay.TabIndex = 121;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(378, 186);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 41);
            this.btnShowKeyPad.TabIndex = 123;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // lblAvailableUnits
            // 
            this.lblAvailableUnits.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableUnits.Location = new System.Drawing.Point(16, 81);
            this.lblAvailableUnits.Name = "lblAvailableUnits";
            this.lblAvailableUnits.Size = new System.Drawing.Size(129, 30);
            this.lblAvailableUnits.TabIndex = 124;
            this.lblAvailableUnits.Text = "Available Units:";
            this.lblAvailableUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAvailableUnitValue
            // 
            this.lblAvailableUnitValue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableUnitValue.Location = new System.Drawing.Point(153, 81);
            this.lblAvailableUnitValue.Name = "lblAvailableUnitValue";
            this.lblAvailableUnitValue.Size = new System.Drawing.Size(62, 30);
            this.lblAvailableUnitValue.TabIndex = 125;
            this.lblAvailableUnitValue.Text = "0";
            this.lblAvailableUnitValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmBasicReservationInputs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(426, 252);
            this.Controls.Add(this.lblAvailableUnitValue);
            this.Controls.Add(this.lblAvailableUnits);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtGuestQty);
            this.Controls.Add(this.txtBookingName);
            this.Controls.Add(this.cmbToTime);
            this.Controls.Add(this.cmbFromTime);
            this.Controls.Add(this.lblToTime);
            this.Controls.Add(this.lblFromTime);
            this.Controls.Add(this.lblGuestQty);
            this.Controls.Add(this.lblBookingName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBasicReservationInputs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Booking Details";
            this.Load += new System.EventHandler(this.frmBasicReservationInputs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBookingName;
        private System.Windows.Forms.Label lblGuestQty;
        private System.Windows.Forms.Label lblFromTime;
        private System.Windows.Forms.Label lblToTime;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbFromTime;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbToTime;
        private System.Windows.Forms.TextBox txtBookingName;
        private System.Windows.Forms.TextBox txtGuestQty;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Label lblAvailableUnits;
        private System.Windows.Forms.Label lblAvailableUnitValue;
    }
}