namespace Parafait_POS.Cards
{
    partial class frmDeactivateCards
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMultipleCards = new System.Windows.Forms.DataGridView();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Card_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcBonus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCourtesy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcRemove = new System.Windows.Forms.DataGridViewButtonColumn();
	    this.buttonRefund = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultipleCards)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMultipleCards
            // 
            this.dgvMultipleCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvMultipleCards.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMultipleCards.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMultipleCards.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMultipleCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMultipleCards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialNumber,
            this.Card_Number,
            this.dcCustomer,
            this.dcCredits,
            this.dcBonus,
            this.dcCourtesy,
            this.dcTickets,
            this.dcRemove});
            this.dgvMultipleCards.EnableHeadersVisualStyles = false;
            this.dgvMultipleCards.Location = new System.Drawing.Point(12, 18);
            this.dgvMultipleCards.Name = "dgvMultipleCards";
            this.dgvMultipleCards.ReadOnly = true;
            this.dgvMultipleCards.RowHeadersVisible = false;
            this.dgvMultipleCards.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMultipleCards.Size = new System.Drawing.Size(762, 404);
            this.dgvMultipleCards.TabIndex = 1;
            this.dgvMultipleCards.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMultipleCards_CellClick);
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBoxMessageLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Firebrick;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 477);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(786, 26);
            this.textBoxMessageLine.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(570, 435);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(97, 35);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.BackColor = System.Drawing.Color.Transparent;
            this.buttonOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonOK.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonOK.FlatAppearance.BorderSize = 0;
            this.buttonOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(118, 435);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(97, 35);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "Deactivate";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // SerialNumber
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SerialNumber.DefaultCellStyle = dataGridViewCellStyle2;
            this.SerialNumber.HeaderText = "Serial#";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.ReadOnly = true;
            this.SerialNumber.Width = 50;
            // 
            // Card_Number
            // 
            this.Card_Number.HeaderText = "Card Number";
            this.Card_Number.Name = "Card_Number";
            this.Card_Number.ReadOnly = true;
            this.Card_Number.Width = 111;
            // 
            // dcCustomer
            // 
            this.dcCustomer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dcCustomer.HeaderText = "Customer";
            this.dcCustomer.Name = "dcCustomer";
            this.dcCustomer.ReadOnly = true;
            // 
            // dcCredits
            // 
            this.dcCredits.HeaderText = "Credits";
            this.dcCredits.Name = "dcCredits";
            this.dcCredits.ReadOnly = true;
            // 
            // dcBonus
            // 
            this.dcBonus.HeaderText = "Bonus";
            this.dcBonus.Name = "dcBonus";
            this.dcBonus.ReadOnly = true;
            // 
            // dcCourtesy
            // 
            this.dcCourtesy.HeaderText = "Courtesy";
            this.dcCourtesy.Name = "dcCourtesy";
            this.dcCourtesy.ReadOnly = true;
            // 
            // dcTickets
            // 
            this.dcTickets.HeaderText = "Tickets";
            this.dcTickets.Name = "dcTickets";
            this.dcTickets.ReadOnly = true;
            // 
            // dcRemove
            // 
            this.dcRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcRemove.HeaderText = "X";
            this.dcRemove.Name = "dcRemove";
            this.dcRemove.ReadOnly = true;
            this.dcRemove.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dcRemove.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dcRemove.Text = "X";
            this.dcRemove.UseColumnTextForButtonValue = true;
            this.dcRemove.Width = 28;
            
	    // 
            // buttonRefund
            // 
            this.buttonRefund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRefund.BackColor = System.Drawing.Color.Transparent;
            this.buttonRefund.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonRefund.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefund.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonRefund.FlatAppearance.BorderSize = 0;
            this.buttonRefund.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonRefund.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonRefund.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonRefund.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRefund.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRefund.ForeColor = System.Drawing.Color.White;
            this.buttonRefund.Location = new System.Drawing.Point(345, 435);
            this.buttonRefund.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonRefund.Name = "buttonRefund";
            this.buttonRefund.Size = new System.Drawing.Size(97, 35);
            this.buttonRefund.TabIndex = 7;
            this.buttonRefund.Text = "Refund";
            this.buttonRefund.UseVisualStyleBackColor = false;
            this.buttonRefund.Click += new System.EventHandler(this.buttonRefund_Click);

	    // 
            // frmDeactivateCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(786, 503);
            this.Controls.Add(this.buttonRefund);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.dgvMultipleCards);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDeactivateCards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Deactivate Cards";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInputPhysicalCards_FormClosed);
            this.Load += new System.EventHandler(this.frmInputPhysicalCards_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultipleCards)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMultipleCards;
        private System.Windows.Forms.TextBox textBoxMessageLine;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Card_Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcBonus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCourtesy;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTickets;
        private System.Windows.Forms.DataGridViewButtonColumn dcRemove;
        private System.Windows.Forms.Button buttonRefund;
    }
}
