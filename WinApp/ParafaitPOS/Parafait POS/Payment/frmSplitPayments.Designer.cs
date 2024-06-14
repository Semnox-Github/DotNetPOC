namespace Parafait_POS.Payment
{
    partial class frmSplitPayments
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rbSplitByItems = new System.Windows.Forms.RadioButton();
            this.rbSplitEqual = new System.Windows.Forms.RadioButton();
            this.nudNoOfGuests = new System.Windows.Forms.NumericUpDown();
            this.panelSample = new System.Windows.Forms.Panel();
            this.lblBalaneAmountSample = new System.Windows.Forms.Label();
            this.lblBalanceSample = new System.Windows.Forms.Label();
            this.txtReferenceSample = new System.Windows.Forms.TextBox();
            this.lblReferenceSample = new System.Windows.Forms.Label();
            this.dgvTrxSample = new System.Windows.Forms.DataGridView();
            this.btnPaySample = new System.Windows.Forms.Button();
            this.btnPrintSample = new System.Windows.Forms.Button();
            this.flpSplits = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSelectSample = new System.Windows.Forms.Button();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.verticalScrollBarViewSample = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoOfGuests)).BeginInit();
            this.panelSample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxSample)).BeginInit();
            this.flpSplits.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.lblTotalAmount);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.rbSplitByItems);
            this.panel1.Controls.Add(this.rbSplitEqual);
            this.panel1.Controls.Add(this.nudNoOfGuests);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(779, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 490);
            this.panel1.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.ClearTrx;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(115, 152);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 44);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoEllipsis = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(109, 12);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(111, 28);
            this.lblTotalAmount.TabIndex = 8;
            this.lblTotalAmount.Text = "Total Amount:";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Total Amount:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(115, 214);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 44);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.NewTrx;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(18, 214);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 44);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.Transparent;
            this.btnGo.BackgroundImage = global::Parafait_POS.Properties.Resources.CompleteTrx;
            this.btnGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGo.FlatAppearance.BorderSize = 0;
            this.btnGo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.Color.White;
            this.btnGo.Location = new System.Drawing.Point(19, 152);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(90, 44);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Split";
            this.btnGo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "No. of Guests:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbSplitByItems
            // 
            this.rbSplitByItems.AutoSize = true;
            this.rbSplitByItems.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rbSplitByItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbSplitByItems.Location = new System.Drawing.Point(112, 109);
            this.rbSplitByItems.Margin = new System.Windows.Forms.Padding(4);
            this.rbSplitByItems.Name = "rbSplitByItems";
            this.rbSplitByItems.Size = new System.Drawing.Size(108, 21);
            this.rbSplitByItems.TabIndex = 2;
            this.rbSplitByItems.Text = "Split by Items";
            this.rbSplitByItems.UseVisualStyleBackColor = true;
            // 
            // rbSplitEqual
            // 
            this.rbSplitEqual.AutoSize = true;
            this.rbSplitEqual.Checked = true;
            this.rbSplitEqual.Location = new System.Drawing.Point(11, 109);
            this.rbSplitEqual.Margin = new System.Windows.Forms.Padding(4);
            this.rbSplitEqual.Name = "rbSplitEqual";
            this.rbSplitEqual.Size = new System.Drawing.Size(93, 21);
            this.rbSplitEqual.TabIndex = 1;
            this.rbSplitEqual.TabStop = true;
            this.rbSplitEqual.Text = "Split Equal";
            this.rbSplitEqual.UseVisualStyleBackColor = true;
            // 
            // nudNoOfGuests
            // 
            this.nudNoOfGuests.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudNoOfGuests.Location = new System.Drawing.Point(112, 57);
            this.nudNoOfGuests.Margin = new System.Windows.Forms.Padding(4);
            this.nudNoOfGuests.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudNoOfGuests.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudNoOfGuests.Name = "nudNoOfGuests";
            this.nudNoOfGuests.Size = new System.Drawing.Size(72, 44);
            this.nudNoOfGuests.TabIndex = 0;
            this.nudNoOfGuests.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNoOfGuests.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // panelSample
            // 
            this.panelSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSample.Controls.Add(this.verticalScrollBarViewSample);
            this.panelSample.Controls.Add(this.btnSelectSample);
            this.panelSample.Controls.Add(this.lblBalaneAmountSample);
            this.panelSample.Controls.Add(this.lblBalanceSample);
            this.panelSample.Controls.Add(this.txtReferenceSample);
            this.panelSample.Controls.Add(this.lblReferenceSample);
            this.panelSample.Controls.Add(this.dgvTrxSample);
            this.panelSample.Controls.Add(this.btnPaySample);
            this.panelSample.Controls.Add(this.btnPrintSample);
            this.panelSample.Location = new System.Drawing.Point(3, 3);
            this.panelSample.Name = "panelSample";
            this.panelSample.Size = new System.Drawing.Size(492, 372);
            this.panelSample.TabIndex = 1;
            // 
            // lblBalaneAmountSample
            // 
            this.lblBalaneAmountSample.AutoSize = true;
            this.lblBalaneAmountSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalaneAmountSample.Location = new System.Drawing.Point(290, 340);
            this.lblBalaneAmountSample.Name = "lblBalaneAmountSample";
            this.lblBalaneAmountSample.Size = new System.Drawing.Size(26, 17);
            this.lblBalaneAmountSample.TabIndex = 11;
            this.lblBalaneAmountSample.Text = "99";
            // 
            // lblBalanceSample
            // 
            this.lblBalanceSample.Location = new System.Drawing.Point(223, 334);
            this.lblBalanceSample.Name = "lblBalanceSample";
            this.lblBalanceSample.Size = new System.Drawing.Size(68, 27);
            this.lblBalanceSample.TabIndex = 10;
            this.lblBalanceSample.Text = "Balance:";
            this.lblBalanceSample.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReferenceSample
            // 
            this.txtReferenceSample.Location = new System.Drawing.Point(196, 7);
            this.txtReferenceSample.Name = "txtReferenceSample";
            this.txtReferenceSample.Size = new System.Drawing.Size(249, 23);
            this.txtReferenceSample.TabIndex = 9;
            this.txtReferenceSample.Validated += new System.EventHandler(this.txtReferenceSample_Validated);
            // 
            // lblReferenceSample
            // 
            this.lblReferenceSample.Location = new System.Drawing.Point(107, 8);
            this.lblReferenceSample.Name = "lblReferenceSample";
            this.lblReferenceSample.Size = new System.Drawing.Size(83, 20);
            this.lblReferenceSample.TabIndex = 8;
            this.lblReferenceSample.Text = "Reference:";
            this.lblReferenceSample.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvTrxSample
            // 
            this.dgvTrxSample.AllowDrop = true;
            this.dgvTrxSample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrxSample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrxSample.Location = new System.Drawing.Point(4, 36);
            this.dgvTrxSample.Name = "dgvTrxSample";
            this.dgvTrxSample.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvTrxSample.Size = new System.Drawing.Size(441, 288);
            this.dgvTrxSample.TabIndex = 7;
            this.dgvTrxSample.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTrxSample_CellMouseDown);
            this.dgvTrxSample.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvTrxSample_DragDrop);
            this.dgvTrxSample.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvTrxSample_DragEnter);
            // 
            // btnPaySample
            // 
            this.btnPaySample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPaySample.BackColor = System.Drawing.Color.Transparent;
            this.btnPaySample.BackgroundImage = global::Parafait_POS.Properties.Resources.payment_buttonRound;
            this.btnPaySample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPaySample.FlatAppearance.BorderSize = 0;
            this.btnPaySample.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPaySample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPaySample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPaySample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaySample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaySample.ForeColor = System.Drawing.Color.Transparent;
            this.btnPaySample.Location = new System.Drawing.Point(91, 328);
            this.btnPaySample.Name = "btnPaySample";
            this.btnPaySample.Size = new System.Drawing.Size(82, 39);
            this.btnPaySample.TabIndex = 6;
            this.btnPaySample.Text = "Pay";
            this.btnPaySample.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPaySample.UseVisualStyleBackColor = false;
            this.btnPaySample.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // btnPrintSample
            // 
            this.btnPrintSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrintSample.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintSample.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrintSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrintSample.FlatAppearance.BorderSize = 0;
            this.btnPrintSample.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintSample.ForeColor = System.Drawing.Color.Transparent;
            this.btnPrintSample.Location = new System.Drawing.Point(3, 328);
            this.btnPrintSample.Name = "btnPrintSample";
            this.btnPrintSample.Size = new System.Drawing.Size(82, 39);
            this.btnPrintSample.TabIndex = 5;
            this.btnPrintSample.Text = "Print";
            this.btnPrintSample.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintSample.UseVisualStyleBackColor = false;
            this.btnPrintSample.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // flpSplits
            // 
            this.flpSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSplits.AutoScroll = true;
            this.flpSplits.Controls.Add(this.panelSample);
            this.flpSplits.Location = new System.Drawing.Point(0, 0);
            this.flpSplits.Name = "flpSplits";
            this.flpSplits.Size = new System.Drawing.Size(732, 490);
            this.flpSplits.TabIndex = 2;
            // 
            // btnSelectSample
            // 
            this.btnSelectSample.AutoSize = true;
            this.btnSelectSample.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectSample.Location = new System.Drawing.Point(13, 2);
            this.btnSelectSample.Name = "btnSelectSample";
            this.btnSelectSample.Size = new System.Drawing.Size(83, 32);
            this.btnSelectSample.TabIndex = 13;
            this.btnSelectSample.Text = "Select";
            this.btnSelectSample.UseVisualStyleBackColor = true;
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = null;
            this.verticalScrollBarView1.Location = new System.Drawing.Point(735, 0);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = this.flpSplits;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 490);
            this.verticalScrollBarView1.TabIndex = 3;
            // 
            // verticalScrollBarViewSample
            // 
            this.verticalScrollBarViewSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarViewSample.AutoHide = false;
            this.verticalScrollBarViewSample.DataGridView = this.dgvTrxSample;
            this.verticalScrollBarViewSample.Location = new System.Drawing.Point(447, 36);
            this.verticalScrollBarViewSample.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarViewSample.Name = "verticalScrollBarViewSample";
            this.verticalScrollBarViewSample.ScrollableControl = this.flpSplits;
            this.verticalScrollBarViewSample.Size = new System.Drawing.Size(40, 288);
            this.verticalScrollBarViewSample.TabIndex = 4;
            // 
            // frmSplitPayments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 490);
            this.Controls.Add(this.verticalScrollBarView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flpSplits);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "frmSplitPayments";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Split Payments";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSplitPayments_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoOfGuests)).EndInit();
            this.panelSample.ResumeLayout(false);
            this.panelSample.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxSample)).EndInit();
            this.flpSplits.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbSplitByItems;
        private System.Windows.Forms.RadioButton rbSplitEqual;
        private System.Windows.Forms.NumericUpDown nudNoOfGuests;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Panel panelSample;
        private System.Windows.Forms.FlowLayoutPanel flpSplits;
        private System.Windows.Forms.Button btnPaySample;
        private System.Windows.Forms.Button btnPrintSample;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvTrxSample;
        private System.Windows.Forms.TextBox txtReferenceSample;
        private System.Windows.Forms.Label lblReferenceSample;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblBalaneAmountSample;
        private System.Windows.Forms.Label lblBalanceSample;
        private System.Windows.Forms.Button btnSelectSample;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarViewSample;
    }
}