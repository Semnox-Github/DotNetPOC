namespace Parafait_POS
{
    partial class SplitView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.btnComplete = new System.Windows.Forms.Button();
            this.verticalScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblBalaneAmount = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.dgvTrxSample = new System.Windows.Forms.DataGridView();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTrxId = new System.Windows.Forms.Label();
            this.lblTrxIdValue = new System.Windows.Forms.Label();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxSample)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.lblTrxId);
            this.panel.Controls.Add(this.lblTrxIdValue);
            this.panel.Controls.Add(this.label2);
            this.panel.Controls.Add(this.lblStatus);
            this.panel.Controls.Add(this.btnComplete);
            this.panel.Controls.Add(this.verticalScrollBarView);
            this.panel.Controls.Add(this.btnSelect);
            this.panel.Controls.Add(this.lblBalaneAmount);
            this.panel.Controls.Add(this.lblBalance);
            this.panel.Controls.Add(this.txtReference);
            this.panel.Controls.Add(this.lblReference);
            this.panel.Controls.Add(this.dgvTrxSample);
            this.panel.Controls.Add(this.btnPay);
            this.panel.Controls.Add(this.btnPrint);
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(415, 395);
            this.panel.TabIndex = 2;
            // 
            // btnComplete
            // 
            this.btnComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnComplete.BackColor = System.Drawing.Color.Transparent;
            this.btnComplete.BackgroundImage = global::Parafait_POS.Properties.Resources.CompleteTrx;
            this.btnComplete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnComplete.FlatAppearance.BorderSize = 0;
            this.btnComplete.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplete.ForeColor = System.Drawing.Color.Transparent;
            this.btnComplete.Location = new System.Drawing.Point(179, 351);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(82, 39);
            this.btnComplete.TabIndex = 14;
            this.btnComplete.Text = "Complete";
            this.btnComplete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // verticalScrollBarView
            // 
            this.verticalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView.AutoHide = false;
            this.verticalScrollBarView.DataGridView = null;
            this.verticalScrollBarView.Location = new System.Drawing.Point(371, 66);
            this.verticalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView.Name = "verticalScrollBarView";
            this.verticalScrollBarView.ScrollableControl = null;
            this.verticalScrollBarView.Size = new System.Drawing.Size(40, 281);
            this.verticalScrollBarView.TabIndex = 13;
            // 
            // btnSelect
            // 
            this.btnSelect.AutoSize = true;
            this.btnSelect.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(4, 28);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(83, 32);
            this.btnSelect.TabIndex = 12;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // lblBalaneAmount
            // 
            this.lblBalaneAmount.AutoSize = true;
            this.lblBalaneAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalaneAmount.Location = new System.Drawing.Point(334, 361);
            this.lblBalaneAmount.Name = "lblBalaneAmount";
            this.lblBalaneAmount.Size = new System.Drawing.Size(26, 17);
            this.lblBalaneAmount.TabIndex = 11;
            this.lblBalaneAmount.Text = "99";
            // 
            // lblBalance
            // 
            this.lblBalance.Location = new System.Drawing.Point(267, 355);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(68, 27);
            this.lblBalance.TabIndex = 10;
            this.lblBalance.Text = "Balance:";
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReference
            // 
            this.txtReference.Location = new System.Drawing.Point(226, 37);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(184, 20);
            this.txtReference.TabIndex = 9;
            this.txtReference.Visible = false;
            // 
            // lblReference
            // 
            this.lblReference.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lblReference.Location = new System.Drawing.Point(99, 34);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(121, 20);
            this.lblReference.TabIndex = 8;
            this.lblReference.Text = "Reference:";
            this.lblReference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblReference.Visible = false;
            // 
            // dgvTrxSample
            // 
            this.dgvTrxSample.AllowDrop = true;
            this.dgvTrxSample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrxSample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrxSample.Location = new System.Drawing.Point(4, 66);
            this.dgvTrxSample.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.dgvTrxSample.Name = "dgvTrxSample";
            this.dgvTrxSample.Size = new System.Drawing.Size(365, 281);
            this.dgvTrxSample.TabIndex = 7;
            // 
            // btnPay
            // 
            this.btnPay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPay.BackColor = System.Drawing.Color.Transparent;
            this.btnPay.BackgroundImage = global::Parafait_POS.Properties.Resources.payment_buttonRound;
            this.btnPay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPay.FlatAppearance.BorderSize = 0;
            this.btnPay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPay.ForeColor = System.Drawing.Color.Transparent;
            this.btnPay.Location = new System.Drawing.Point(91, 351);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(82, 39);
            this.btnPay.TabIndex = 6;
            this.btnPay.Text = "Pay";
            this.btnPay.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(3, 351);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(82, 39);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lblStatus.Location = new System.Drawing.Point(341, 4);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(57, 22);
            this.lblStatus.TabIndex = 15;
            this.lblStatus.Text = "Open";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(267, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 22);
            this.label2.TabIndex = 16;
            this.label2.Text = "Status:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTrxId
            // 
            this.lblTrxId.AutoSize = true;
            this.lblTrxId.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrxId.Location = new System.Drawing.Point(13, 3);
            this.lblTrxId.Name = "lblTrxId";
            this.lblTrxId.Size = new System.Drawing.Size(58, 22);
            this.lblTrxId.TabIndex = 18;
            this.lblTrxId.Text = "TrxId:";
            this.lblTrxId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTrxIdValue
            // 
            this.lblTrxIdValue.AutoSize = true;
            this.lblTrxIdValue.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lblTrxIdValue.Location = new System.Drawing.Point(77, 3);
            this.lblTrxIdValue.Name = "lblTrxIdValue";
            this.lblTrxIdValue.Size = new System.Drawing.Size(43, 22);
            this.lblTrxIdValue.TabIndex = 17;
            this.lblTrxIdValue.Text = "100";
            this.lblTrxIdValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SplitView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel);
            this.Name = "SplitView";
            this.Size = new System.Drawing.Size(415, 395);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxSample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label lblBalaneAmount;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.DataGridView dgvTrxSample;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSelect;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblTrxId;
        private System.Windows.Forms.Label lblTrxIdValue;
    }
}
