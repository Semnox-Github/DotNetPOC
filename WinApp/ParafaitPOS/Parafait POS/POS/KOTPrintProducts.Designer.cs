namespace Parafait_POS
{
    partial class KOTPrintProducts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KOTPrintProducts));
            this.dgvOrderLines = new System.Windows.Forms.DataGridView();
            this.PrintKOT = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrintKOT = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderLines)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOrderLines
            // 
            this.dgvOrderLines.AllowUserToAddRows = false;
            this.dgvOrderLines.AllowUserToDeleteRows = false;
            this.dgvOrderLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrderLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PrintKOT});
            this.dgvOrderLines.Location = new System.Drawing.Point(6, 7);
            this.dgvOrderLines.Name = "dgvOrderLines";
            this.dgvOrderLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderLines.Size = new System.Drawing.Size(758, 311);
            this.dgvOrderLines.TabIndex = 1;
            // 
            // PrintKOT
            // 
            this.PrintKOT.FalseValue = "N";
            this.PrintKOT.HeaderText = "Print KOT?";
            this.PrintKOT.Name = "PrintKOT";
            this.PrintKOT.TrueValue = "Y";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(429, 328);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 45);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // btnPrintKOT
            // 
            this.btnPrintKOT.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintKOT.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrintKOT.BackgroundImage")));
            this.btnPrintKOT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintKOT.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnPrintKOT.FlatAppearance.BorderSize = 0;
            this.btnPrintKOT.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintKOT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintKOT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintKOT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintKOT.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintKOT.ForeColor = System.Drawing.Color.White;
            this.btnPrintKOT.Location = new System.Drawing.Point(243, 328);
            this.btnPrintKOT.Name = "btnPrintKOT";
            this.btnPrintKOT.Size = new System.Drawing.Size(95, 45);
            this.btnPrintKOT.TabIndex = 6;
            this.btnPrintKOT.Text = "Print KOT";
            this.btnPrintKOT.UseVisualStyleBackColor = false;
            this.btnPrintKOT.Click += new System.EventHandler(this.btnPrintKOT_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 382);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(771, 26);
            this.txtMessage.TabIndex = 8;
            this.txtMessage.Text = "Message";
            // 
            // KOTPrintProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(771, 408);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnPrintKOT);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvOrderLines);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KOTPrintProducts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print KOT";
            this.Load += new System.EventHandler(this.KOTPrintProducts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOrderLines;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PrintKOT;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrintKOT;
        private System.Windows.Forms.TextBox txtMessage;
    }
}