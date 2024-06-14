using System;
using System.Windows.Forms;

namespace Parafait_POS.Redemption
{
    partial class frmReprintTicketReceipt
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
            this.lblIssueDate = new System.Windows.Forms.Label();
            this.lblFromTime = new System.Windows.Forms.Label();
            this.lblToTime = new System.Windows.Forms.Label();
            this.dtpIssueDate = new System.Windows.Forms.DateTimePicker();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.dgvTicketDetails = new System.Windows.Forms.DataGridView();
            this.dataGridViewTicketId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manualTicketReceiptNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isSuspectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.issueDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reprintCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketReceiptDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.txtTicketNumber = new System.Windows.Forms.TextBox();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblTicketNumber = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.cmbSearchToTime = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.cmbSearchFromTime = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.btnKeyPad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketReceiptDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIssueDate
            // 
            this.lblIssueDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblIssueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIssueDate.Location = new System.Drawing.Point(58, 31);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.Size = new System.Drawing.Size(79, 17);
            this.lblIssueDate.TabIndex = 2;
            this.lblIssueDate.Text = "Issue Date:";
            // 
            // lblFromTime
            // 
            this.lblFromTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromTime.Location = new System.Drawing.Point(414, 31);
            this.lblFromTime.Name = "lblFromTime";
            this.lblFromTime.Size = new System.Drawing.Size(79, 17);
            this.lblFromTime.TabIndex = 3;
            this.lblFromTime.Text = "From Time:";
            // 
            // lblToTime
            // 
            this.lblToTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToTime.Location = new System.Drawing.Point(646, 31);
            this.lblToTime.Name = "lblToTime";
            this.lblToTime.Size = new System.Drawing.Size(64, 17);
            this.lblToTime.TabIndex = 4;
            this.lblToTime.Text = "To Time:";
            // 
            // dtpIssueDate
            // 
            this.dtpIssueDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpIssueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpIssueDate.Location = new System.Drawing.Point(140, 28);
            this.dtpIssueDate.Name = "dtpIssueDate";
            this.dtpIssueDate.Size = new System.Drawing.Size(249, 24);
            this.dtpIssueDate.TabIndex = 5;
            this.dtpIssueDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dtpIssueDate_MouseDown);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(134, 391);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(92, 47);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(289, 391);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(94, 47);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // dgvTicketDetails
            // 
            this.dgvTicketDetails.AllowUserToAddRows = false;
            this.dgvTicketDetails.AllowUserToDeleteRows = false;
            this.dgvTicketDetails.AllowUserToOrderColumns = true;
            this.dgvTicketDetails.AllowUserToResizeColumns = false;
            this.dgvTicketDetails.AllowUserToResizeRows = false;
            this.dgvTicketDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTicketDetails.AutoGenerateColumns = false;
            this.dgvTicketDetails.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvTicketDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTicketDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTicketDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTicketDetails.ColumnHeadersHeight = 38;
            this.dgvTicketDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTicketId,
            this.idDataGridViewTextBoxColumn,
            this.manualTicketReceiptNoDataGridViewTextBoxColumn,
            this.ticketsDataGridViewTextBoxColumn,
            this.balanceTicketsDataGridViewTextBoxColumn,
            this.isSuspectedDataGridViewCheckBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.issueDateDataGridViewTextBoxColumn,
            this.reprintCountDataGridViewTextBoxColumn});
            this.dgvTicketDetails.DataSource = this.ticketReceiptDTOBindingSource;
            this.dgvTicketDetails.EnableHeadersVisualStyles = false;
            this.dgvTicketDetails.GridColor = System.Drawing.Color.White;
            this.dgvTicketDetails.Location = new System.Drawing.Point(15, 129);
            this.dgvTicketDetails.MultiSelect = false;
            this.dgvTicketDetails.Name = "dgvTicketDetails";
            this.dgvTicketDetails.ReadOnly = true;
            this.dgvTicketDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTicketDetails.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTicketDetails.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvTicketDetails.RowTemplate.Height = 40;
            this.dgvTicketDetails.RowTemplate.ReadOnly = true;
            this.dgvTicketDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTicketDetails.ShowEditingIcon = false;
            this.dgvTicketDetails.Size = new System.Drawing.Size(814, 246);
            this.dgvTicketDetails.TabIndex = 12;
            this.dgvTicketDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTicketDetails_CellClick);
            this.dgvTicketDetails.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvTicketDetails_Scroll);
            // 
            // dataGridViewTicketId
            // 
            this.dataGridViewTicketId.HeaderText = "Ticket Id";
            this.dataGridViewTicketId.Name = "dataGridViewTicketId";
            this.dataGridViewTicketId.ReadOnly = true;
            this.dataGridViewTicketId.Visible = false;
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
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.DataPropertyName = "ManualTicketReceiptNo";
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.HeaderText = "Ticket Receipt No";
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.Name = "manualTicketReceiptNoDataGridViewTextBoxColumn";
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.manualTicketReceiptNoDataGridViewTextBoxColumn.Width = 250;
            // 
            // ticketsDataGridViewTextBoxColumn
            // 
            this.ticketsDataGridViewTextBoxColumn.DataPropertyName = "Tickets";
            this.ticketsDataGridViewTextBoxColumn.HeaderText = "Tickets";
            this.ticketsDataGridViewTextBoxColumn.Name = "ticketsDataGridViewTextBoxColumn";
            this.ticketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.ticketsDataGridViewTextBoxColumn.Visible = false;
            // 
            // balanceTicketsDataGridViewTextBoxColumn
            // 
            this.balanceTicketsDataGridViewTextBoxColumn.DataPropertyName = "BalanceTickets";
            this.balanceTicketsDataGridViewTextBoxColumn.HeaderText = "Tickets";
            this.balanceTicketsDataGridViewTextBoxColumn.Name = "balanceTicketsDataGridViewTextBoxColumn";
            this.balanceTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isSuspectedDataGridViewCheckBoxColumn
            // 
            this.isSuspectedDataGridViewCheckBoxColumn.DataPropertyName = "IsSuspected";
            this.isSuspectedDataGridViewCheckBoxColumn.HeaderText = "IsSuspected?";
            this.isSuspectedDataGridViewCheckBoxColumn.Name = "isSuspectedDataGridViewCheckBoxColumn";
            this.isSuspectedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isSuspectedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // issueDateDataGridViewTextBoxColumn
            // 
            this.issueDateDataGridViewTextBoxColumn.DataPropertyName = "IssueDate";
            this.issueDateDataGridViewTextBoxColumn.HeaderText = "Issue Date";
            this.issueDateDataGridViewTextBoxColumn.Name = "issueDateDataGridViewTextBoxColumn";
            this.issueDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.issueDateDataGridViewTextBoxColumn.Width = 300;
            // 
            // reprintCountDataGridViewTextBoxColumn
            // 
            this.reprintCountDataGridViewTextBoxColumn.DataPropertyName = "ReprintCount";
            this.reprintCountDataGridViewTextBoxColumn.HeaderText = "ReprintCount";
            this.reprintCountDataGridViewTextBoxColumn.Name = "reprintCountDataGridViewTextBoxColumn";
            this.reprintCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.reprintCountDataGridViewTextBoxColumn.Visible = false;
            // 
            // ticketReceiptDTOBindingSource
            // 
            this.ticketReceiptDTOBindingSource.DataSource = typeof(Semnox.Parafait.Redemption.TicketReceiptDTO);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.Search_Btn_Normal;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(660, 73);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 40);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(499, 77);
            this.txtCardNumber.MaxLength = 10;
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(140, 24);
            this.txtCardNumber.TabIndex = 2;
            // 
            // txtTicketNumber
            // 
            this.txtTicketNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTicketNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTicketNumber.Location = new System.Drawing.Point(140, 78);
            this.txtTicketNumber.Name = "txtTicketNumber";
            this.txtTicketNumber.Size = new System.Drawing.Size(249, 24);
            this.txtTicketNumber.TabIndex = 1;
            this.txtTicketNumber.Text = "SDMNKRT0025698745";
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.Location = new System.Drawing.Point(397, 80);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(96, 17);
            this.lblCardNumber.TabIndex = 15;
            this.lblCardNumber.Text = "Card Number:";
            // 
            // lblTicketNumber
            // 
            this.lblTicketNumber.AutoSize = true;
            this.lblTicketNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketNumber.Location = new System.Drawing.Point(13, 80);
            this.lblTicketNumber.Name = "lblTicketNumber";
            this.lblTicketNumber.Size = new System.Drawing.Size(128, 17);
            this.lblTicketNumber.TabIndex = 14;
            this.lblTicketNumber.Text = "Ticket Receipt No: ";
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.ClearTrx;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(754, 73);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 40);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cmbSearchToTime
            // 
            this.cmbSearchToTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchToTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchToTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchToTime.FormattingEnabled = true;
            this.cmbSearchToTime.Location = new System.Drawing.Point(714, 28);
            this.cmbSearchToTime.Name = "cmbSearchToTime";
            this.cmbSearchToTime.Size = new System.Drawing.Size(116, 26);
            this.cmbSearchToTime.TabIndex = 97;
            // 
            // cmbSearchFromTime
            // 
            this.cmbSearchFromTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchFromTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchFromTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchFromTime.FormattingEnabled = true;
            this.cmbSearchFromTime.Location = new System.Drawing.Point(499, 28);
            this.cmbSearchFromTime.Name = "cmbSearchFromTime";
            this.cmbSearchFromTime.Size = new System.Drawing.Size(140, 26);
            this.cmbSearchFromTime.TabIndex = 96;
            // 
            // btnKeyPad
            // 
            this.btnKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.BorderSize = 0;
            this.btnKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyPad.Location = new System.Drawing.Point(417, 391);
            this.btnKeyPad.Name = "btnKeyPad";
            this.btnKeyPad.Size = new System.Drawing.Size(58, 47);
            this.btnKeyPad.TabIndex = 98;
            this.btnKeyPad.UseVisualStyleBackColor = false;
            // 
            // frmReprintTicketReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(872, 450);
            this.Controls.Add(this.btnKeyPad);
            this.Controls.Add(this.cmbSearchToTime);
            this.Controls.Add(this.cmbSearchFromTime);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.txtTicketNumber);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.lblTicketNumber);
            this.Controls.Add(this.dgvTicketDetails);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dtpIssueDate);
            this.Controls.Add(this.lblToTime);
            this.Controls.Add(this.lblFromTime);
            this.Controls.Add(this.lblIssueDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmReprintTicketReceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Ticket Receipt Reprint ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReprintTicketReceipt_FormClosed);
            this.Load += new System.EventHandler(this.frmReprintTicketReceipt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketReceiptDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
         


        #endregion
        private System.Windows.Forms.Label lblIssueDate;
        private System.Windows.Forms.Label lblFromTime;
        private System.Windows.Forms.Label lblToTime;
        private System.Windows.Forms.DateTimePicker dtpIssueDate;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView dgvTicketDetails;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.TextBox txtTicketNumber;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Label lblTicketNumber;
        private System.Windows.Forms.Button btnClear;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchToTime;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchFromTime;
        private System.Windows.Forms.BindingSource ticketReceiptDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTicketId;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn manualTicketReceiptNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isSuspectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn issueDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reprintCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnKeyPad;
    }
}