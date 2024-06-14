namespace Parafait_POS
{
    partial class AttractionSchedulesOld
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttractionSchedulesOld));
            this.dgvAttractionSchedules = new System.Windows.Forms.DataGridView();
            this.PickSeats = new System.Windows.Forms.DataGridViewImageColumn();
            this.dtpAttractionDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numPadPanel = new System.Windows.Forms.Panel();
            this.lblAllocatedQty = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.chkShowPast = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbAttractionFacility = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttractionSchedules)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.numPadPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAttractionSchedules
            // 
            this.dgvAttractionSchedules.AllowUserToAddRows = false;
            this.dgvAttractionSchedules.AllowUserToDeleteRows = false;
            this.dgvAttractionSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAttractionSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttractionSchedules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PickSeats});
            this.dgvAttractionSchedules.Location = new System.Drawing.Point(7, 21);
            this.dgvAttractionSchedules.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvAttractionSchedules.Name = "dgvAttractionSchedules";
            this.dgvAttractionSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttractionSchedules.Size = new System.Drawing.Size(694, 539);
            this.dgvAttractionSchedules.TabIndex = 0;
            this.dgvAttractionSchedules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttractionSchedules_CellClick);
            this.dgvAttractionSchedules.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAttractionSchedules_DataError);
            // 
            // PickSeats
            // 
            this.PickSeats.HeaderText = "Pick Seats";
            this.PickSeats.Name = "PickSeats";
            // 
            // dtpAttractionDate
            // 
            this.dtpAttractionDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpAttractionDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpAttractionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttractionDate.Location = new System.Drawing.Point(81, 15);
            this.dtpAttractionDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpAttractionDate.Name = "dtpAttractionDate";
            this.dtpAttractionDate.Size = new System.Drawing.Size(143, 21);
            this.dtpAttractionDate.TabIndex = 1;
            this.dtpAttractionDate.ValueChanged += new System.EventHandler(this.dtpAttractionDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "For Date:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(5, 491);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(104, 43);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "Done";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(159, 491);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 43);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvAttractionSchedules);
            this.groupBox1.Controls.Add(this.numPadPanel);
            this.groupBox1.Location = new System.Drawing.Point(7, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(978, 572);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // numPadPanel
            // 
            this.numPadPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPadPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPadPanel.Controls.Add(this.lblAllocatedQty);
            this.numPadPanel.Controls.Add(this.btnCancel);
            this.numPadPanel.Controls.Add(this.label5);
            this.numPadPanel.Controls.Add(this.btnOK);
            this.numPadPanel.Controls.Add(this.lblQuantity);
            this.numPadPanel.Controls.Add(this.label2);
            this.numPadPanel.Location = new System.Drawing.Point(705, 21);
            this.numPadPanel.Name = "numPadPanel";
            this.numPadPanel.Size = new System.Drawing.Size(270, 539);
            this.numPadPanel.TabIndex = 1;
            // 
            // lblAllocatedQty
            // 
            this.lblAllocatedQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllocatedQty.AutoSize = true;
            this.lblAllocatedQty.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllocatedQty.Location = new System.Drawing.Point(151, 394);
            this.lblAllocatedQty.Name = "lblAllocatedQty";
            this.lblAllocatedQty.Size = new System.Drawing.Size(35, 19);
            this.lblAllocatedQty.TabIndex = 32;
            this.lblAllocatedQty.Text = "Qty";
            this.lblAllocatedQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(17, 394);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 18);
            this.label5.TabIndex = 30;
            this.label5.Text = "Booked Units:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQuantity
            // 
            this.lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.Location = new System.Drawing.Point(151, 364);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(35, 19);
            this.lblQuantity.TabIndex = 29;
            this.lblQuantity.Text = "Qty";
            this.lblQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(14, 364);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 18);
            this.label2.TabIndex = 28;
            this.label2.Text = "Required Units:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(230, 13);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(30, 25);
            this.btnPrev.TabIndex = 21;
            this.btnPrev.Text = "<<";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(267, 13);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 25);
            this.btnNext.TabIndex = 20;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // chkShowPast
            // 
            this.chkShowPast.AutoSize = true;
            this.chkShowPast.Location = new System.Drawing.Point(336, 15);
            this.chkShowPast.Name = "chkShowPast";
            this.chkShowPast.Size = new System.Drawing.Size(155, 20);
            this.chkShowPast.TabIndex = 22;
            this.chkShowPast.Text = "Show Past Schedules";
            this.chkShowPast.UseVisualStyleBackColor = true;
            this.chkShowPast.CheckedChanged += new System.EventHandler(this.chkShowPast_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(490, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 18);
            this.label3.TabIndex = 27;
            this.label3.Text = "Attraction:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbAttractionFacility
            // 
            this.cmbAttractionFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttractionFacility.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAttractionFacility.FormattingEnabled = true;
            this.cmbAttractionFacility.Location = new System.Drawing.Point(587, 14);
            this.cmbAttractionFacility.Name = "cmbAttractionFacility";
            this.cmbAttractionFacility.Size = new System.Drawing.Size(139, 24);
            this.cmbAttractionFacility.TabIndex = 26;
            // 
            // AttractionSchedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(993, 611);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbAttractionFacility);
            this.Controls.Add(this.chkShowPast);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpAttractionDate);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AttractionSchedules";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attraction Schedules";
            this.Load += new System.EventHandler(this.AttractionSchedules_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttractionSchedules)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.numPadPanel.ResumeLayout(false);
            this.numPadPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAttractionSchedules;
        private System.Windows.Forms.DateTimePicker dtpAttractionDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.DataGridViewImageColumn PickSeats;
        private System.Windows.Forms.CheckBox chkShowPast;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbAttractionFacility;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblAllocatedQty;
        private System.Windows.Forms.Panel numPadPanel;
    }
}