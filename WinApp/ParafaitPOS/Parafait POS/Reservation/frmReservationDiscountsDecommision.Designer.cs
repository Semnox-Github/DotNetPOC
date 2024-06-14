namespace Parafait_POS.Reservation
{
    partial class frmReservationDiscounts
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanelDiscounts = new System.Windows.Forms.FlowLayoutPanel();
            this.SampleButtonDiscount = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvSelected = new System.Windows.Forms.DataGridView();
            this.dcSelected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.flowLayoutPanelDiscounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanelDiscounts
            // 
            this.flowLayoutPanelDiscounts.AutoScroll = true;
            this.flowLayoutPanelDiscounts.Controls.Add(this.SampleButtonDiscount);
            this.flowLayoutPanelDiscounts.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelDiscounts.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelDiscounts.Name = "flowLayoutPanelDiscounts";
            this.flowLayoutPanelDiscounts.Size = new System.Drawing.Size(586, 286);
            this.flowLayoutPanelDiscounts.TabIndex = 2;
            // 
            // SampleButtonDiscount
            // 
            this.SampleButtonDiscount.BackColor = System.Drawing.Color.Transparent;
            this.SampleButtonDiscount.BackgroundImage = global::Parafait_POS.Properties.Resources.DiplayGroupButton;
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
            this.SampleButtonDiscount.TabIndex = 2;
            this.SampleButtonDiscount.Text = "Sample";
            this.SampleButtonDiscount.UseVisualStyleBackColor = false;
            this.SampleButtonDiscount.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.button_normal;
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
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // dgvSelected
            // 
            this.dgvSelected.AllowUserToAddRows = false;
            this.dgvSelected.AllowUserToDeleteRows = false;
            this.dgvSelected.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelected.BackgroundColor = System.Drawing.Color.Azure;
            this.dgvSelected.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelected.ColumnHeadersVisible = false;
            this.dgvSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcSelected});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Azure;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Azure;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelected.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelected.GridColor = System.Drawing.Color.Azure;
            this.dgvSelected.Location = new System.Drawing.Point(4, 288);
            this.dgvSelected.Name = "dgvSelected";
            this.dgvSelected.RowHeadersVisible = false;
            this.dgvSelected.Size = new System.Drawing.Size(578, 103);
            this.dgvSelected.TabIndex = 5;
            // 
            // dcSelected
            // 
            this.dcSelected.HeaderText = "Selected";
            this.dcSelected.Name = "dcSelected";
            this.dcSelected.ReadOnly = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.button_normal;
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
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ReservationDiscounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 445);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvSelected);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.flowLayoutPanelDiscounts);
            this.Name = "ReservationDiscounts";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reservation Discounts";
            this.Load += new System.EventHandler(this.ReservationDiscounts_Load);
            this.flowLayoutPanelDiscounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDiscounts;
        private System.Windows.Forms.Button SampleButtonDiscount;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSelected;
        private System.Windows.Forms.Button btnClose;

    }
}