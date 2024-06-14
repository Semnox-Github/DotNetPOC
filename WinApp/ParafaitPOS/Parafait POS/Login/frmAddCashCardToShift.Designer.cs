namespace Parafait_POS
{
    partial class frmAddCashCardToShift
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCash = new System.Windows.Forms.TextBox();
            this.txtCards = new System.Windows.Forms.TextBox();
            this.ReasonComboBox = new System.Windows.Forms.ComboBox();
            this.lookupValuesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.rbIN = new System.Windows.Forms.RadioButton();
            this.rbOUT = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.CashCardInOut = new System.Windows.Forms.GroupBox();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lookupValuesDTOBindingSource)).BeginInit();
            this.CashCardInOut.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 143);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(160, 35);
            this.label1.TabIndex = 13;
            this.label1.Text = "Cash:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 188);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(160, 35);
            this.label2.TabIndex = 14;
            this.label2.Text = "Cards:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(160, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Paid:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 100);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(160, 35);
            this.label4.TabIndex = 12;
            this.label4.Text = "Reason:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(19, 237);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(160, 35);
            this.label5.TabIndex = 15;
            this.label5.Text = "Remarks:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCash
            // 
            this.txtCash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCash.Location = new System.Drawing.Point(211, 143);
            this.txtCash.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtCash.Name = "txtCash";
            this.txtCash.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCash.Size = new System.Drawing.Size(159, 24);
            this.txtCash.TabIndex = 4;
            this.txtCash.Click += new System.EventHandler(this.TxtCash_Click);
            // 
            // txtCards
            // 
            this.txtCards.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCards.Location = new System.Drawing.Point(211, 188);
            this.txtCards.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtCards.Name = "txtCards";
            this.txtCards.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCards.Size = new System.Drawing.Size(159, 24);
            this.txtCards.TabIndex = 5;
            this.txtCards.Click += new System.EventHandler(this.TxtCards_Click);
            // 
            // ReasonComboBox
            // 
            this.ReasonComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ReasonComboBox.DataSource = this.lookupValuesDTOBindingSource;
            this.ReasonComboBox.DisplayMember = "Description";
            this.ReasonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReasonComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ReasonComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReasonComboBox.FormattingEnabled = true;
            this.ReasonComboBox.Location = new System.Drawing.Point(211, 100);
            this.ReasonComboBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ReasonComboBox.Name = "ReasonComboBox";
            this.ReasonComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ReasonComboBox.Size = new System.Drawing.Size(333, 26);
            this.ReasonComboBox.TabIndex = 3;
            this.ReasonComboBox.ValueMember = "LookupValueId";
            // 
            // lookupValuesDTOBindingSource
            // 
            this.lookupValuesDTOBindingSource.DataSource = typeof(Semnox.Core.Utilities.LookupValuesDTO);
            // 
            // txtRemarks
            // 
            this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(211, 237);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtRemarks.Size = new System.Drawing.Size(333, 70);
            this.txtRemarks.TabIndex = 6;
            this.txtRemarks.Click += new System.EventHandler(this.TxtRemarks_Click);
            // 
            // rbIN
            // 
            this.rbIN.AutoSize = true;
            this.rbIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbIN.Location = new System.Drawing.Point(211, 60);
            this.rbIN.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rbIN.Name = "rbIN";
            this.rbIN.Size = new System.Drawing.Size(40, 22);
            this.rbIN.TabIndex = 1;
            this.rbIN.Text = "IN";
            this.rbIN.UseVisualStyleBackColor = true;
            // 
            // rbOUT
            // 
            this.rbOUT.AutoSize = true;
            this.rbOUT.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbOUT.Location = new System.Drawing.Point(289, 60);
            this.rbOUT.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rbOUT.Name = "rbOUT";
            this.rbOUT.Size = new System.Drawing.Size(58, 22);
            this.rbOUT.TabIndex = 2;
            this.rbOUT.Text = "OUT";
            this.rbOUT.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Beige;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(308, 347);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 55);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatAppearance.BorderColor = System.Drawing.Color.Beige;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOkay.ForeColor = System.Drawing.Color.White;
            this.btnOkay.Location = new System.Drawing.Point(154, 347);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(5);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(133, 55);
            this.btnOkay.TabIndex = 7;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.BtnOkay_Click);
            // 
            // CashCardInOut
            // 
            this.CashCardInOut.Controls.Add(this.rbOUT);
            this.CashCardInOut.Controls.Add(this.ReasonComboBox);
            this.CashCardInOut.Controls.Add(this.rbIN);
            this.CashCardInOut.Controls.Add(this.txtRemarks);
            this.CashCardInOut.Controls.Add(this.btnOkay);
            this.CashCardInOut.Controls.Add(this.btnCancel);
            this.CashCardInOut.Controls.Add(this.btnShowNumPad);
            this.CashCardInOut.Controls.Add(this.label5);
            this.CashCardInOut.Controls.Add(this.label3);
            this.CashCardInOut.Controls.Add(this.label4);
            this.CashCardInOut.Controls.Add(this.txtCards);
            this.CashCardInOut.Controls.Add(this.txtCash);
            this.CashCardInOut.Controls.Add(this.label1);
            this.CashCardInOut.Controls.Add(this.label2);
            this.CashCardInOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CashCardInOut.Location = new System.Drawing.Point(3, -5);
            this.CashCardInOut.Name = "CashCardInOut";
            this.CashCardInOut.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CashCardInOut.Size = new System.Drawing.Size(572, 430);
            this.CashCardInOut.TabIndex = 10;
            this.CashCardInOut.TabStop = false;
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MintCream;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowNumPad.Location = new System.Drawing.Point(530, 375);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 9;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.BtnShowNumPad_Click);
            // 
            // frmAddCashCardToShift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(580, 428);
            this.Controls.Add(this.CashCardInOut);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmAddCashCardToShift";
            this.Text = "Shift Cash In and Out";
            this.Load += new System.EventHandler(this.frmAddCashCardToShift_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookupValuesDTOBindingSource)).EndInit();
            this.CashCardInOut.ResumeLayout(false);
            this.CashCardInOut.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCash;
        private System.Windows.Forms.TextBox txtCards;
        private System.Windows.Forms.ComboBox ReasonComboBox;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.RadioButton rbIN;
        private System.Windows.Forms.RadioButton rbOUT;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.GroupBox CashCardInOut;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.BindingSource lookupValuesDTOBindingSource;
    }
}