namespace Parafait_Kiosk
{
    partial class frmRedeemTokens
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TimerMoney = new System.Windows.Forms.Timer(this.components);
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.panelGrid = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCashInserted = new System.Windows.Forms.DataGridView();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.Denomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnCompleteNEWCard = new System.Windows.Forms.Button();
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.btnCompleteReload = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtAvlblTokens = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCashInserted)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.TabIndex = 1056;
            // 
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(668, 863);
            this.btnPrev.TabIndex = 138;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(1010, 863);
            // 
            // TimerMoney
            // 
            this.TimerMoney.Tick += new System.EventHandler(this.TimerMoney_Tick);
            // 
            // panelGrid
            // 
            this.panelGrid.BackColor = System.Drawing.Color.Transparent;
            this.panelGrid.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.table3;
            this.panelGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelGrid.Controls.Add(this.label4);
            this.panelGrid.Controls.Add(this.label3);
            this.panelGrid.Controls.Add(this.label2);
            this.panelGrid.Controls.Add(this.dgvCashInserted);
            this.panelGrid.Location = new System.Drawing.Point(644, 677);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(661, 165);
            this.panelGrid.TabIndex = 145;
            this.panelGrid.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(518, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Points";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(326, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Quantity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(73, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Denomination";
            // 
            // dgvCashInserted
            // 
            this.dgvCashInserted.AllowUserToAddRows = false;
            this.dgvCashInserted.AllowUserToDeleteRows = false;
            this.dgvCashInserted.AllowUserToResizeColumns = false;
            this.dgvCashInserted.AllowUserToResizeRows = false;
            this.dgvCashInserted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCashInserted.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvCashInserted.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCashInserted.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.DarkKhaki;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCashInserted.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCashInserted.ColumnHeadersVisible = false;
            this.dgvCashInserted.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image,
            this.Denomination,
            this.Quantity,
            this.Amount});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Khaki;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Khaki;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCashInserted.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCashInserted.EnableHeadersVisualStyles = false;
            this.dgvCashInserted.GridColor = System.Drawing.Color.White;
            this.dgvCashInserted.Location = new System.Drawing.Point(0, 47);
            this.dgvCashInserted.Name = "dgvCashInserted";
            this.dgvCashInserted.ReadOnly = true;
            this.dgvCashInserted.RowHeadersVisible = false;
            this.dgvCashInserted.RowTemplate.Height = 40;
            this.dgvCashInserted.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCashInserted.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCashInserted.Size = new System.Drawing.Size(660, 99);
            this.dgvCashInserted.TabIndex = 1;
            // 
            // Image
            // 
            this.Image.HeaderText = "";
            this.Image.Image = global::Parafait_Kiosk.Properties.Resources.Generic_Coin_Note;
            this.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            this.Image.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Image.Visible = false;
            // 
            // Denomination
            // 
            this.Denomination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Denomination.HeaderText = "Denomination";
            this.Denomination.Name = "Denomination";
            this.Denomination.ReadOnly = true;
            this.Denomination.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Denomination.Width = 269;
            // 
            // Quantity
            // 
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Quantity.Width = 196;
            // 
            // Amount
            // 
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Amount.Width = 195;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1030);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 146;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnCompleteNEWCard
            // 
            this.btnCompleteNEWCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCompleteNEWCard.BackColor = System.Drawing.Color.Transparent;
            this.btnCompleteNEWCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.New_Play_Pass_Button_big;
            this.btnCompleteNEWCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCompleteNEWCard.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnCompleteNEWCard.FlatAppearance.BorderSize = 0;
            this.btnCompleteNEWCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCompleteNEWCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCompleteNEWCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompleteNEWCard.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompleteNEWCard.ForeColor = System.Drawing.Color.White;
            this.btnCompleteNEWCard.Location = new System.Drawing.Point(383, 413);
            this.btnCompleteNEWCard.Name = "btnCompleteNEWCard";
            this.btnCompleteNEWCard.Size = new System.Drawing.Size(550, 410);
            this.btnCompleteNEWCard.TabIndex = 147;
            this.btnCompleteNEWCard.Text = "Get NEW Card";
            this.btnCompleteNEWCard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCompleteNEWCard.UseVisualStyleBackColor = false;
            this.btnCompleteNEWCard.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(183, 132);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1536, 114);
            this.lblGreeting1.TabIndex = 149;
            this.lblGreeting1.Text = "Please insert the tokens and press complete";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeOut.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_SmallBox;
            this.lblTimeOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeOut.FlatAppearance.BorderSize = 0;
            this.lblTimeOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeOut.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblTimeOut.Location = new System.Drawing.Point(1725, 28);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(142, 110);
            this.lblTimeOut.TabIndex = 150;
            this.lblTimeOut.Text = "Redeem in Progress";
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // btnCompleteReload
            // 
            this.btnCompleteReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCompleteReload.BackColor = System.Drawing.Color.Transparent;
            this.btnCompleteReload.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Recharge_Play_Pass_Big;
            this.btnCompleteReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCompleteReload.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnCompleteReload.FlatAppearance.BorderSize = 0;
            this.btnCompleteReload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCompleteReload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCompleteReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompleteReload.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompleteReload.ForeColor = System.Drawing.Color.White;
            this.btnCompleteReload.Location = new System.Drawing.Point(995, 413);
            this.btnCompleteReload.Name = "btnCompleteReload";
            this.btnCompleteReload.Size = new System.Drawing.Size(550, 410);
            this.btnCompleteReload.TabIndex = 151;
            this.btnCompleteReload.Text = "Load to Existing Card";
            this.btnCompleteReload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCompleteReload.UseVisualStyleBackColor = false;
            this.btnCompleteReload.Click += new System.EventHandler(this.btnCompleteReload_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.txtAvlblTokens);
            this.panel2.Location = new System.Drawing.Point(906, 263);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 66);
            this.panel2.TabIndex = 1055;
            // 
            // txtAvlblTokens
            // 
            this.txtAvlblTokens.BackColor = System.Drawing.Color.Transparent;
            this.txtAvlblTokens.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAvlblTokens.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtAvlblTokens.Location = new System.Drawing.Point(16, 10);
            this.txtAvlblTokens.Name = "txtAvlblTokens";
            this.txtAvlblTokens.Size = new System.Drawing.Size(378, 46);
            this.txtAvlblTokens.TabIndex = 1048;
            this.txtAvlblTokens.Text = "9";
            this.txtAvlblTokens.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(376, 267);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(514, 52);
            this.label1.TabIndex = 1054;
            this.label1.Text = "Tokens Inserted:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(190, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1529, 113);
            this.button1.TabIndex = 1057;
            this.button1.Text = "Redeem Token";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // frmRedeemTokens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCompleteReload);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.btnCompleteNEWCard);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmRedeemTokens";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmRedeemTokens";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNewcard_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRedeemTokens_FormClosed);
            this.Load += new System.EventHandler(this.frmRedeemTokens_Load);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.panelGrid, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.btnCompleteNEWCard, 0);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.lblTimeOut, 0);
            this.Controls.SetChildIndex(this.btnCompleteReload, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.panelGrid.ResumeLayout(false);
            this.panelGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCashInserted)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        //private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Timer TimerMoney;
        private System.Windows.Forms.Timer exitTimer;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCashInserted;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn Denomination;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnCompleteNEWCard;
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Button lblTimeOut;
        private System.Windows.Forms.Button btnCompleteReload;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label txtAvlblTokens;
        private System.Windows.Forms.Label label1;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button button1;
    }
}
