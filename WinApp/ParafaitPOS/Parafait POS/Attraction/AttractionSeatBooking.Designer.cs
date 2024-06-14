namespace Parafait_POS
{
    partial class AttractionSeatBooking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttractionSeatBooking));
            this.dgvLayout = new System.Windows.Forms.DataGridView();
            this.test = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSeatsSelected = new System.Windows.Forms.Label();
            this.panelScreenPosition = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblScreenPosition = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.btnCancelBooking = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalSeats = new System.Windows.Forms.Label();
            this.lblBookedSeats = new System.Windows.Forms.Label();
            this.lblFacility = new System.Windows.Forms.Label();
            this.cmbFacility = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayout)).BeginInit();
            this.panelScreenPosition.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvLayout
            // 
            this.dgvLayout.AllowUserToAddRows = false;
            this.dgvLayout.AllowUserToDeleteRows = false;
            this.dgvLayout.AllowUserToResizeColumns = false;
            this.dgvLayout.AllowUserToResizeRows = false;
            this.dgvLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLayout.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.dgvLayout.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvLayout.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLayout.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.test});
            this.dgvLayout.EnableHeadersVisualStyles = false;
            this.dgvLayout.GridColor = System.Drawing.Color.White;
            this.dgvLayout.Location = new System.Drawing.Point(3, 6);
            this.dgvLayout.Name = "dgvLayout";
            this.dgvLayout.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvLayout.RowHeadersWidth = 80;
            this.dgvLayout.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvLayout.ShowEditingIcon = false;
            this.dgvLayout.Size = new System.Drawing.Size(881, 452);
            this.dgvLayout.TabIndex = 7;
            this.dgvLayout.SelectionChanged += new System.EventHandler(this.dgvLayout_SelectionChanged);
            // 
            // test
            // 
            this.test.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.test.HeaderText = "1";
            this.test.Name = "test";
            this.test.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.test.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.test.Width = 37;
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.BackColor = System.Drawing.Color.Transparent;
            this.btnDone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDone.BackgroundImage")));
            this.btnDone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDone.FlatAppearance.BorderColor = System.Drawing.Color.Aquamarine;
            this.btnDone.FlatAppearance.BorderSize = 0;
            this.btnDone.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDone.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDone.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Location = new System.Drawing.Point(888, 364);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(134, 43);
            this.btnDone.TabIndex = 10;
            this.btnDone.Text = "Confirm Booking";
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Aquamarine;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(888, 460);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(134, 43);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Back";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(898, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Seats Selected";
            // 
            // lblSeatsSelected
            // 
            this.lblSeatsSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSeatsSelected.BackColor = System.Drawing.Color.LightCyan;
            this.lblSeatsSelected.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeatsSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeatsSelected.ForeColor = System.Drawing.Color.DimGray;
            this.lblSeatsSelected.Location = new System.Drawing.Point(890, 134);
            this.lblSeatsSelected.Name = "lblSeatsSelected";
            this.lblSeatsSelected.Size = new System.Drawing.Size(134, 130);
            this.lblSeatsSelected.TabIndex = 13;
            this.lblSeatsSelected.Text = "A1, A2, A3, A4, A5, A6, A7, A8";
            // 
            // panelScreenPosition
            // 
            this.panelScreenPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelScreenPosition.BackColor = System.Drawing.Color.Transparent;
            this.panelScreenPosition.Controls.Add(this.label4);
            this.panelScreenPosition.Controls.Add(this.lblScreenPosition);
            this.panelScreenPosition.Controls.Add(this.label5);
            this.panelScreenPosition.Location = new System.Drawing.Point(3, 464);
            this.panelScreenPosition.Name = "panelScreenPosition";
            this.panelScreenPosition.Size = new System.Drawing.Size(881, 42);
            this.panelScreenPosition.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.BackColor = System.Drawing.Color.IndianRed;
            this.label4.Location = new System.Drawing.Point(0, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 40);
            this.label4.TabIndex = 4;
            // 
            // lblScreenPosition
            // 
            this.lblScreenPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreenPosition.BackColor = System.Drawing.Color.IndianRed;
            this.lblScreenPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenPosition.ForeColor = System.Drawing.Color.Silver;
            this.lblScreenPosition.Location = new System.Drawing.Point(0, 15);
            this.lblScreenPosition.Name = "lblScreenPosition";
            this.lblScreenPosition.Size = new System.Drawing.Size(881, 27);
            this.lblScreenPosition.TabIndex = 1;
            this.lblScreenPosition.Text = "Screen / Stage This Way";
            this.lblScreenPosition.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.Color.IndianRed;
            this.label5.Location = new System.Drawing.Point(818, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 40);
            this.label5.TabIndex = 5;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSelection.BackColor = System.Drawing.Color.Transparent;
            this.btnClearSelection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearSelection.BackgroundImage")));
            this.btnClearSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClearSelection.FlatAppearance.BorderColor = System.Drawing.Color.Aquamarine;
            this.btnClearSelection.FlatAppearance.BorderSize = 0;
            this.btnClearSelection.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClearSelection.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearSelection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSelection.ForeColor = System.Drawing.Color.White;
            this.btnClearSelection.Location = new System.Drawing.Point(889, 267);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(134, 43);
            this.btnClearSelection.TabIndex = 15;
            this.btnClearSelection.Text = "Clear Selection";
            this.btnClearSelection.UseVisualStyleBackColor = false;
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // btnCancelBooking
            // 
            this.btnCancelBooking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancelBooking.BackgroundImage")));
            this.btnCancelBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancelBooking.FlatAppearance.BorderColor = System.Drawing.Color.Aquamarine;
            this.btnCancelBooking.FlatAppearance.BorderSize = 0;
            this.btnCancelBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelBooking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelBooking.ForeColor = System.Drawing.Color.White;
            this.btnCancelBooking.Location = new System.Drawing.Point(888, 412);
            this.btnCancelBooking.Name = "btnCancelBooking";
            this.btnCancelBooking.Size = new System.Drawing.Size(134, 43);
            this.btnCancelBooking.TabIndex = 16;
            this.btnCancelBooking.Text = "Cancel Booking";
            this.btnCancelBooking.UseVisualStyleBackColor = false;
            this.btnCancelBooking.Click += new System.EventHandler(this.btnCancelBooking_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(904, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Total Seats:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(886, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Booked Seats:";
            // 
            // lblTotalSeats
            // 
            this.lblTotalSeats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalSeats.AutoSize = true;
            this.lblTotalSeats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalSeats.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTotalSeats.Location = new System.Drawing.Point(991, 66);
            this.lblTotalSeats.Name = "lblTotalSeats";
            this.lblTotalSeats.Size = new System.Drawing.Size(35, 16);
            this.lblTotalSeats.TabIndex = 19;
            this.lblTotalSeats.Text = "999";
            this.lblTotalSeats.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBookedSeats
            // 
            this.lblBookedSeats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBookedSeats.AutoSize = true;
            this.lblBookedSeats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookedSeats.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblBookedSeats.Location = new System.Drawing.Point(992, 91);
            this.lblBookedSeats.Name = "lblBookedSeats";
            this.lblBookedSeats.Size = new System.Drawing.Size(35, 16);
            this.lblBookedSeats.TabIndex = 20;
            this.lblBookedSeats.Text = "999";
            this.lblBookedSeats.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFacility
            // 
            this.lblFacility.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFacility.AutoSize = true;
            this.lblFacility.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFacility.ForeColor = System.Drawing.Color.DimGray;
            this.lblFacility.Location = new System.Drawing.Point(925, 3);
            this.lblFacility.Name = "lblFacility";
            this.lblFacility.Size = new System.Drawing.Size(59, 16);
            this.lblFacility.TabIndex = 21;
            this.lblFacility.Text = "Facility";
            // 
            // cmbFacility
            // 
            this.cmbFacility.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFacility.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFacility.FormattingEnabled = true;
            this.cmbFacility.Location = new System.Drawing.Point(894, 22);
            this.cmbFacility.Name = "cmbFacility";
            this.cmbFacility.Size = new System.Drawing.Size(121, 24);
            this.cmbFacility.TabIndex = 22;
            this.cmbFacility.SelectedIndexChanged += new System.EventHandler(this.cmbFacility_SelectedIndexChanged);
            // 
            // AttractionSeatBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 509);
            this.Controls.Add(this.cmbFacility);
            this.Controls.Add(this.lblFacility);
            this.Controls.Add(this.lblBookedSeats);
            this.Controls.Add(this.lblTotalSeats);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancelBooking);
            this.Controls.Add(this.btnClearSelection);
            this.Controls.Add(this.panelScreenPosition);
            this.Controls.Add(this.lblSeatsSelected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.dgvLayout);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AttractionSeatBooking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attraction Seat Booking";
            this.Load += new System.EventHandler(this.AttractionSeatBooking_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayout)).EndInit();
            this.panelScreenPosition.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLayout;
        private System.Windows.Forms.DataGridViewImageColumn test;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSeatsSelected;
        private System.Windows.Forms.Panel panelScreenPosition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblScreenPosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.Button btnCancelBooking;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTotalSeats;
        private System.Windows.Forms.Label lblBookedSeats;
        private System.Windows.Forms.Label lblFacility;
        private System.Windows.Forms.ComboBox cmbFacility;
    }
}