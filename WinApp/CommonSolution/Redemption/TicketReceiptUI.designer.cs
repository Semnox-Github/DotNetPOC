namespace Semnox.Parafait.Redemption
{
    partial class TicketReceiptUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtFromdate = new System.Windows.Forms.TextBox();
            this.txtTodate = new System.Windows.Forms.TextBox();
            this.chbFlagged = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtBalanceTicketTo = new System.Windows.Forms.TextBox();
            this.txtBalanceTicketFrom = new System.Windows.Forms.TextBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.txtReceiptNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvManualTicket = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manualTicketReceiptNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSuspected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnRowRemarks = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketReceiptDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnKeyPad = new System.Windows.Forms.Button();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManualTicket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketReceiptDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Controls.Add(this.btnClear);
            this.gpFilter.Controls.Add(this.txtFromdate);
            this.gpFilter.Controls.Add(this.txtTodate);
            this.gpFilter.Controls.Add(this.chbFlagged);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtBalanceTicketTo);
            this.gpFilter.Controls.Add(this.txtBalanceTicketFrom);
            this.gpFilter.Controls.Add(this.dtpToDate);
            this.gpFilter.Controls.Add(this.label6);
            this.gpFilter.Controls.Add(this.dtpFromDate);
            this.gpFilter.Controls.Add(this.txtReceiptNo);
            this.gpFilter.Controls.Add(this.label4);
            this.gpFilter.Controls.Add(this.label3);
            this.gpFilter.Controls.Add(this.label2);
            this.gpFilter.Controls.Add(this.label1);
            this.gpFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpFilter.Location = new System.Drawing.Point(2, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(890, 84);
            this.gpFilter.TabIndex = 0;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Search Options";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Semnox.Parafait.Redemption.Properties.Resources.ClearTrx;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(774, 20);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(99, 51);
            this.btnClear.TabIndex = 15;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtFromdate
            // 
            this.txtFromdate.Location = new System.Drawing.Point(122, 52);
            this.txtFromdate.Name = "txtFromdate";
            this.txtFromdate.Size = new System.Drawing.Size(72, 22);
            this.txtFromdate.TabIndex = 14;
            // 
            // txtTodate
            // 
            this.txtTodate.Location = new System.Drawing.Point(389, 52);
            this.txtTodate.Name = "txtTodate";
            this.txtTodate.Size = new System.Drawing.Size(72, 22);
            this.txtTodate.TabIndex = 13;
            // 
            // chbFlagged
            // 
            this.chbFlagged.Location = new System.Drawing.Point(526, 55);
            this.chbFlagged.Name = "chbFlagged";
            this.chbFlagged.Size = new System.Drawing.Size(111, 24);
            this.chbFlagged.TabIndex = 12;
            this.chbFlagged.Text = "Is Suspect";
            this.chbFlagged.UseVisualStyleBackColor = true;
            this.chbFlagged.CheckedChanged += new System.EventHandler(this.chbFlagged_CheckedChanged);
            this.chbFlagged.Enter += new System.EventHandler(this.SearchFieldEnter);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Semnox.Parafait.Redemption.Properties.Resources.Search_Btn_Normal;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(661, 19);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(99, 51);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtBalanceTicketTo
            // 
            this.txtBalanceTicketTo.Location = new System.Drawing.Point(526, 19);
            this.txtBalanceTicketTo.Name = "txtBalanceTicketTo";
            this.txtBalanceTicketTo.Size = new System.Drawing.Size(100, 22);
            this.txtBalanceTicketTo.TabIndex = 10;
            this.txtBalanceTicketTo.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtBalanceTicketTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBalanceTicketFrom_KeyPress);
            // 
            // txtBalanceTicketFrom
            // 
            this.txtBalanceTicketFrom.Location = new System.Drawing.Point(389, 19);
            this.txtBalanceTicketFrom.Name = "txtBalanceTicketFrom";
            this.txtBalanceTicketFrom.Size = new System.Drawing.Size(100, 22);
            this.txtBalanceTicketFrom.TabIndex = 9;
            this.txtBalanceTicketFrom.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtBalanceTicketFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBalanceTicketFrom_KeyPress);
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(389, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(100, 22);
            this.dtpToDate.TabIndex = 8;
            this.dtpToDate.Value = new System.DateTime(2017, 12, 26, 18, 18, 38, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(293, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "To Date:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Checked = false;
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(122, 52);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(100, 22);
            this.dtpFromDate.TabIndex = 5;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // txtReceiptNo
            // 
            this.txtReceiptNo.Location = new System.Drawing.Point(122, 20);
            this.txtReceiptNo.Name = "txtReceiptNo";
            this.txtReceiptNo.Size = new System.Drawing.Size(100, 22);
            this.txtReceiptNo.TabIndex = 4;
            this.txtReceiptNo.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtReceiptNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(495, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "and";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(229, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Balance Ticket Between:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "From Date:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Receipt No:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Semnox.Parafait.Redemption.Properties.Resources.CompleteTrx;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(40, 305);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 51);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Semnox.Parafait.Redemption.Properties.Resources.CancelLine;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(180, 305);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 51);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvManualTicket
            // 
            this.dgvManualTicket.AllowUserToAddRows = false;
            this.dgvManualTicket.AllowUserToDeleteRows = false;
            this.dgvManualTicket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvManualTicket.AutoGenerateColumns = false;
            this.dgvManualTicket.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvManualTicket.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvManualTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvManualTicket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManualTicket.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.manualTicketReceiptNoDataGridViewTextBoxColumn,
            this.ticketsDataGridViewTextBoxColumn,
            this.balanceTicketsDataGridViewTextBoxColumn,
            this.IsSuspected,
            this.btnRowRemarks,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn});
            this.dgvManualTicket.DataSource = this.ticketReceiptDTOBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvManualTicket.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvManualTicket.GridColor = System.Drawing.Color.White;
            this.dgvManualTicket.Location = new System.Drawing.Point(2, 93);
            this.dgvManualTicket.Margin = new System.Windows.Forms.Padding(4);
            this.dgvManualTicket.Name = "dgvManualTicket";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvManualTicket.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvManualTicket.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvManualTicket.RowTemplate.Height = 30;
            this.dgvManualTicket.Size = new System.Drawing.Size(1133, 202);
            this.dgvManualTicket.TabIndex = 14;
            this.dgvManualTicket.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvManualTicket_CellContentClick);
            this.dgvManualTicket.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvManualTicket_Scroll);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // manualTicketReceiptNoDataGridViewTextBoxColumn
            // 
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.DataPropertyName = "ManualTicketReceiptNo";
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.HeaderText = "Ticket Receipt No";
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.Name = "manualTicketReceiptNoDataGridViewTextBoxColumn";
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ticketsDataGridViewTextBoxColumn
            // 
            this.ticketsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ticketsDataGridViewTextBoxColumn.DataPropertyName = "Tickets";
            this.ticketsDataGridViewTextBoxColumn.HeaderText = "Tickets";
            this.ticketsDataGridViewTextBoxColumn.Name = "ticketsDataGridViewTextBoxColumn";
            this.ticketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // balanceTicketsDataGridViewTextBoxColumn
            // 
            this.balanceTicketsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.balanceTicketsDataGridViewTextBoxColumn.DataPropertyName = "BalanceTickets";
            this.balanceTicketsDataGridViewTextBoxColumn.HeaderText = "Balance Tickets";
            this.balanceTicketsDataGridViewTextBoxColumn.Name = "balanceTicketsDataGridViewTextBoxColumn";
            this.balanceTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // IsSuspected
            // 
            this.IsSuspected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IsSuspected.DataPropertyName = "IsSuspected";
            this.IsSuspected.HeaderText = "Mark Suspected";
            this.IsSuspected.Name = "IsSuspected";
            // 
            // btnRowRemarks
            // 
            this.btnRowRemarks.HeaderText = "Remarks";
            this.btnRowRemarks.Name = "btnRowRemarks";
            this.btnRowRemarks.Text = "...";
            this.btnRowRemarks.UseColumnTextForButtonValue = true;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ticketReceiptDTOBindingSource
            // 
            this.ticketReceiptDTOBindingSource.DataSource = typeof(Semnox.Parafait.Redemption.TicketReceiptDTO);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(2, 362);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1133, 18);
            this.lblMessage.TabIndex = 15;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnKeyPad
            // 
            this.btnKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.BorderSize = 0;
            this.btnKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyPad.Image = global::Semnox.Parafait.Redemption.Properties.Resources.keyboard;
            this.btnKeyPad.Location = new System.Drawing.Point(898, 41);
            this.btnKeyPad.Name = "btnKeyPad";
            this.btnKeyPad.Size = new System.Drawing.Size(68, 46);
            this.btnKeyPad.TabIndex = 99;
            this.btnKeyPad.UseVisualStyleBackColor = false;
            // 
            // TicketReceiptUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1140, 384);
            this.Controls.Add(this.btnKeyPad);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.dgvManualTicket);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gpFilter);
            this.Name = "TicketReceiptUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Ticket Receipt";
            this.Load += new System.EventHandler(this.TicketReceiptUI_Load);
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManualTicket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketReceiptDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.TextBox txtReceiptNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBalanceTicketTo;
        private System.Windows.Forms.TextBox txtBalanceTicketFrom;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.DataGridView dgvManualTicket;
        private System.Windows.Forms.BindingSource ticketReceiptDTOBindingSource;
        private System.Windows.Forms.CheckBox chbFlagged;
        private System.Windows.Forms.TextBox txtFromdate;
        private System.Windows.Forms.TextBox txtTodate;        
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn manualTicketReceiptNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSuspected;
        private System.Windows.Forms.DataGridViewButtonColumn btnRowRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnKeyPad;
    }
}

