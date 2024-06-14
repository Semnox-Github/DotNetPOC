﻿namespace Semnox.Parafait.Booking
{
    partial class ReservationDiscountsListUI
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
            this.flpDiscounts = new System.Windows.Forms.FlowLayoutPanel();
            this.SampleButtonDiscount = new System.Windows.Forms.Button();
            this.dvgDiscountsDTOList = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.discountsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.discountNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountPercentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpDiscounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgDiscountsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountsDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // flpDiscounts
            // 
            this.flpDiscounts.Controls.Add(this.SampleButtonDiscount);
            this.flpDiscounts.Location = new System.Drawing.Point(0, 0);
            this.flpDiscounts.Name = "flpDiscounts";
            this.flpDiscounts.Size = new System.Drawing.Size(586, 286);
            this.flpDiscounts.TabIndex = 0;
            // 
            // SampleButtonDiscount
            // 
            this.SampleButtonDiscount.BackColor = System.Drawing.Color.Transparent;
            this.SampleButtonDiscount.BackgroundImage = global::Semnox.Parafait.Booking.Properties.Resources.DiplayGroupButton;
            this.SampleButtonDiscount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SampleButtonDiscount.FlatAppearance.BorderSize = 0;
            this.SampleButtonDiscount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonDiscount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SampleButtonDiscount.ForeColor = System.Drawing.Color.White;
            this.SampleButtonDiscount.Location = new System.Drawing.Point(4, 4);
            this.SampleButtonDiscount.Margin = new System.Windows.Forms.Padding(4);
            this.SampleButtonDiscount.Name = "SampleButtonDiscount";
            this.SampleButtonDiscount.Size = new System.Drawing.Size(170, 113);
            this.SampleButtonDiscount.TabIndex = 3;
            this.SampleButtonDiscount.Text = "Sample";
            this.SampleButtonDiscount.UseVisualStyleBackColor = false;
            this.SampleButtonDiscount.Visible = false;
            // 
            // dvgDiscountsDTOList
            // 
            this.dvgDiscountsDTOList.AllowUserToAddRows = false;
            this.dvgDiscountsDTOList.AutoGenerateColumns = false;
            this.dvgDiscountsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgDiscountsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.discountNameDataGridViewTextBoxColumn,
            this.discountPercentageDataGridViewTextBoxColumn,
            this.discountAmountDataGridViewTextBoxColumn});
            this.dvgDiscountsDTOList.DataSource = this.discountsDTOListBS;
            this.dvgDiscountsDTOList.Location = new System.Drawing.Point(4, 288);
            this.dvgDiscountsDTOList.Name = "dvgDiscountsDTOList";
            this.dvgDiscountsDTOList.ReadOnly = true;
            this.dvgDiscountsDTOList.Size = new System.Drawing.Size(578, 103);
            this.dvgDiscountsDTOList.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Semnox.Parafait.Booking.Properties.Resources.button_normal;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(327, 398);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 42);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Semnox.Parafait.Booking.Properties.Resources.button_normal;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(122, 398);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(138, 42);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // discountsDTOListBS
            // 
            this.discountsDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountsDTO);
            // 
            // discountNameDataGridViewTextBoxColumn
            // 
            this.discountNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.discountNameDataGridViewTextBoxColumn.DataPropertyName = "DiscountName";
            this.discountNameDataGridViewTextBoxColumn.HeaderText = "Discount Name";
            this.discountNameDataGridViewTextBoxColumn.Name = "discountNameDataGridViewTextBoxColumn";
            this.discountNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // discountPercentageDataGridViewTextBoxColumn
            // 
            this.discountPercentageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.discountPercentageDataGridViewTextBoxColumn.DataPropertyName = "DiscountPercentage";
            this.discountPercentageDataGridViewTextBoxColumn.HeaderText = "Discount Percentage";
            this.discountPercentageDataGridViewTextBoxColumn.Name = "discountPercentageDataGridViewTextBoxColumn";
            this.discountPercentageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // discountAmountDataGridViewTextBoxColumn
            // 
            this.discountAmountDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.discountAmountDataGridViewTextBoxColumn.DataPropertyName = "DiscountAmount";
            this.discountAmountDataGridViewTextBoxColumn.HeaderText = "Discount Amount";
            this.discountAmountDataGridViewTextBoxColumn.Name = "discountAmountDataGridViewTextBoxColumn";
            this.discountAmountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ReservationDiscountsListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 445);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dvgDiscountsDTOList);
            this.Controls.Add(this.flpDiscounts);
            this.Name = "ReservationDiscountsListUI";
            this.Text = "Reservation Discounts";
            this.Load += new System.EventHandler(this.ReservationDiscountsListUI_Load);
            this.flpDiscounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvgDiscountsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountsDTOListBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpDiscounts;
        private System.Windows.Forms.DataGridView dvgDiscountsDTOList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button SampleButtonDiscount;
        private System.Windows.Forms.BindingSource discountsDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountPercentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountAmountDataGridViewTextBoxColumn;
    }
}