namespace Semnox.Parafait.Transaction
{
    partial class TrxUserLogsUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrxUserLogsUI));
            this.btnClose = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.dgvViewTransaction = new System.Windows.Forms.DataGridView();
            this.activityDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approverIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvalTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxUserLogsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvViewTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trxUserLogsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
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
            this.btnClose.Location = new System.Drawing.Point(397, 331);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 45);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 382);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(847, 26);
            this.txtMessage.TabIndex = 9;
            this.txtMessage.Text = "Message";
            // 
            // dgvViewTransaction
            // 
            this.dgvViewTransaction.AllowUserToAddRows = false;
            this.dgvViewTransaction.AllowUserToDeleteRows = false;
            this.dgvViewTransaction.AutoGenerateColumns = false;
            this.dgvViewTransaction.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvViewTransaction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvViewTransaction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.activityDateDataGridViewTextBoxColumn,
            this.productNameDataGridViewTextBoxColumn,
            this.productNameDateDataGridViewTextBoxColumn,
            this.trxIdDataGridViewTextBoxColumn,
            this.lineIdDataGridViewTextBoxColumn,
            this.loginIdDataGridViewTextBoxColumn,
            this.actionDataGridViewTextBoxColumn,
            this.activityDataGridViewTextBoxColumn,
            this.approverIdDataGridViewTextBoxColumn,
            this.approvalTimeDataGridViewTextBoxColumn});
            this.dgvViewTransaction.DataSource = this.trxUserLogsDTOBindingSource;
            this.dgvViewTransaction.Location = new System.Drawing.Point(12, 12);
            this.dgvViewTransaction.Name = "dgvViewTransaction";
            this.dgvViewTransaction.ReadOnly = true;
            this.dgvViewTransaction.Size = new System.Drawing.Size(823, 313);
            this.dgvViewTransaction.TabIndex = 10;
            // 
            // activityDateDataGridViewTextBoxColumn
            // 
            this.activityDateDataGridViewTextBoxColumn.DataPropertyName = "ActivityDate";
            this.activityDateDataGridViewTextBoxColumn.HeaderText = "Activity Date";
            this.activityDateDataGridViewTextBoxColumn.Name = "activityDateDataGridViewTextBoxColumn";
            this.activityDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "Product Name";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            this.productNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // productNameDateDataGridViewTextBoxColumn
            // 
            this.productNameDateDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDateDataGridViewTextBoxColumn.HeaderText = "Product Name";
            this.productNameDateDataGridViewTextBoxColumn.Name = "productNameDateDataGridViewTextBoxColumn";
            this.productNameDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.productNameDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // trxIdDataGridViewTextBoxColumn
            // 
            this.trxIdDataGridViewTextBoxColumn.DataPropertyName = "TrxId";
            this.trxIdDataGridViewTextBoxColumn.HeaderText = "TrxId";
            this.trxIdDataGridViewTextBoxColumn.Name = "trxIdDataGridViewTextBoxColumn";
            this.trxIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.trxIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // lineIdDataGridViewTextBoxColumn
            // 
            this.lineIdDataGridViewTextBoxColumn.DataPropertyName = "LineId";
            this.lineIdDataGridViewTextBoxColumn.HeaderText = "LineId";
            this.lineIdDataGridViewTextBoxColumn.Name = "lineIdDataGridViewTextBoxColumn";
            this.lineIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.lineIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // loginIdDataGridViewTextBoxColumn
            // 
            this.loginIdDataGridViewTextBoxColumn.DataPropertyName = "LoginId";
            this.loginIdDataGridViewTextBoxColumn.HeaderText = "Login Id";
            this.loginIdDataGridViewTextBoxColumn.Name = "loginIdDataGridViewTextBoxColumn";
            this.loginIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // actionDataGridViewTextBoxColumn
            // 
            this.actionDataGridViewTextBoxColumn.DataPropertyName = "Action";
            this.actionDataGridViewTextBoxColumn.HeaderText = "Action";
            this.actionDataGridViewTextBoxColumn.Name = "actionDataGridViewTextBoxColumn";
            this.actionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // activityDataGridViewTextBoxColumn
            // 
            this.activityDataGridViewTextBoxColumn.DataPropertyName = "Activity";
            this.activityDataGridViewTextBoxColumn.HeaderText = "Activity";
            this.activityDataGridViewTextBoxColumn.Name = "activityDataGridViewTextBoxColumn";
            this.activityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // approverIdDataGridViewTextBoxColumn
            // 
            this.approverIdDataGridViewTextBoxColumn.DataPropertyName = "ApproverId";
            this.approverIdDataGridViewTextBoxColumn.HeaderText = "Approver Id";
            this.approverIdDataGridViewTextBoxColumn.Name = "approverIdDataGridViewTextBoxColumn";
            this.approverIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // approvalTimeDataGridViewTextBoxColumn
            // 
            this.approvalTimeDataGridViewTextBoxColumn.DataPropertyName = "ApprovalTime";
            this.approvalTimeDataGridViewTextBoxColumn.HeaderText = "Approval Time";
            this.approvalTimeDataGridViewTextBoxColumn.Name = "approvalTimeDataGridViewTextBoxColumn";
            this.approvalTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // trxUserLogsDTOBindingSource
            // 
            this.trxUserLogsDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.TrxUserLogsDTO);
            // 
            // TrxUserLogsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 408);
            this.Controls.Add(this.dgvViewTransaction);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrxUserLogsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserLogs";
            this.Load += new System.EventHandler(this.TrxUserLogsUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvViewTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trxUserLogsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.DataGridView dgvViewTransaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource trxUserLogsDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn activityDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn activityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approverIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvalTimeDataGridViewTextBoxColumn;
    }
}

