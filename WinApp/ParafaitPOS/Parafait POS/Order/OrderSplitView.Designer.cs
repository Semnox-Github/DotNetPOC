namespace Parafait_POS
{
    partial class OrderSplitView
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
            this.btnSplitEqual = new System.Windows.Forms.Button();
            this.btnAddSplit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUndoSplit = new System.Windows.Forms.Button();
            this.flpSplits = new System.Windows.Forms.FlowLayoutPanel();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalBalanceAmount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSplitEqual
            // 
            this.btnSplitEqual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSplitEqual.BackColor = System.Drawing.Color.Transparent;
            this.btnSplitEqual.BackgroundImage = global::Parafait_POS.Properties.Resources.CompleteTrx;
            this.btnSplitEqual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSplitEqual.FlatAppearance.BorderSize = 0;
            this.btnSplitEqual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSplitEqual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSplitEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplitEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSplitEqual.ForeColor = System.Drawing.Color.White;
            this.btnSplitEqual.Location = new System.Drawing.Point(300, 395);
            this.btnSplitEqual.Name = "btnSplitEqual";
            this.btnSplitEqual.Size = new System.Drawing.Size(90, 44);
            this.btnSplitEqual.TabIndex = 17;
            this.btnSplitEqual.Text = "Split Equal";
            this.btnSplitEqual.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSplitEqual.UseVisualStyleBackColor = false;
            this.btnSplitEqual.Click += new System.EventHandler(this.btnSplitEqual_Click);
            // 
            // btnAddSplit
            // 
            this.btnAddSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddSplit.BackColor = System.Drawing.Color.Transparent;
            this.btnAddSplit.BackgroundImage = global::Parafait_POS.Properties.Resources.Add_Btn_Normal;
            this.btnAddSplit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddSplit.FlatAppearance.BorderSize = 0;
            this.btnAddSplit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddSplit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddSplit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSplit.ForeColor = System.Drawing.Color.White;
            this.btnAddSplit.Location = new System.Drawing.Point(204, 395);
            this.btnAddSplit.Name = "btnAddSplit";
            this.btnAddSplit.Size = new System.Drawing.Size(90, 44);
            this.btnAddSplit.TabIndex = 16;
            this.btnAddSplit.Text = "Add";
            this.btnAddSplit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddSplit.UseVisualStyleBackColor = false;
            this.btnAddSplit.Click += new System.EventHandler(this.btnAddSplit_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.ClearTrx;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(492, 395);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 44);
            this.btnClear.TabIndex = 15;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(108, 395);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 44);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.NewTrx;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(12, 395);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 44);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUndoSplit
            // 
            this.btnUndoSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUndoSplit.BackColor = System.Drawing.Color.Transparent;
            this.btnUndoSplit.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnUndoSplit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUndoSplit.FlatAppearance.BorderSize = 0;
            this.btnUndoSplit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnUndoSplit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnUndoSplit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndoSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUndoSplit.ForeColor = System.Drawing.Color.White;
            this.btnUndoSplit.Location = new System.Drawing.Point(396, 395);
            this.btnUndoSplit.Name = "btnUndoSplit";
            this.btnUndoSplit.Size = new System.Drawing.Size(90, 44);
            this.btnUndoSplit.TabIndex = 12;
            this.btnUndoSplit.Text = "Undo Split";
            this.btnUndoSplit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUndoSplit.UseVisualStyleBackColor = false;
            this.btnUndoSplit.Click += new System.EventHandler(this.btnUndoSplit_Click);
            // 
            // flpSplits
            // 
            this.flpSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSplits.AutoScroll = true;
            this.flpSplits.Location = new System.Drawing.Point(0, -1);
            this.flpSplits.Name = "flpSplits";
            this.flpSplits.Size = new System.Drawing.Size(948, 390);
            this.flpSplits.TabIndex = 18;
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = null;
            this.verticalScrollBarView1.Location = new System.Drawing.Point(951, -1);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = this.flpSplits;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 390);
            this.verticalScrollBarView1.TabIndex = 19;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalAmount.AutoEllipsis = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(837, 392);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(111, 28);
            this.lblTotalAmount.TabIndex = 21;
            this.lblTotalAmount.Text = "Total Amount:";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(734, 395);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "Total Amount:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalBalanceAmount
            // 
            this.lblTotalBalanceAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalBalanceAmount.AutoEllipsis = true;
            this.lblTotalBalanceAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalBalanceAmount.Location = new System.Drawing.Point(837, 419);
            this.lblTotalBalanceAmount.Name = "lblTotalBalanceAmount";
            this.lblTotalBalanceAmount.Size = new System.Drawing.Size(111, 28);
            this.lblTotalBalanceAmount.TabIndex = 23;
            this.lblTotalBalanceAmount.Text = "Total Amount:";
            this.lblTotalBalanceAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(734, 422);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 20);
            this.label3.TabIndex = 22;
            this.label3.Text = "Total Balance:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OrderSplitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 451);
            this.Controls.Add(this.lblTotalBalanceAmount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.verticalScrollBarView1);
            this.Controls.Add(this.flpSplits);
            this.Controls.Add(this.btnSplitEqual);
            this.Controls.Add(this.btnAddSplit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnUndoSplit);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OrderSplitView";
            this.Text = "OrderSplitView";
            this.Load += new System.EventHandler(this.OrderSplitView_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSplitEqual;
        private System.Windows.Forms.Button btnAddSplit;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUndoSplit;
        private System.Windows.Forms.FlowLayoutPanel flpSplits;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalBalanceAmount;
        private System.Windows.Forms.Label label3;
    }
}