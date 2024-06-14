namespace Parafait_POS
{
    partial class frmCardActivityZoom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCardActivityZoom));
            this.labelPurchases = new System.Windows.Forms.Label();
            this.dataGridViewPurchases = new System.Windows.Forms.DataGridView();
            this.dcBtnCardActivityTrxPrint = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewGamePlay = new System.Windows.Forms.DataGridView();
            this.ReverseGamePlay = new System.Windows.Forms.DataGridViewButtonColumn();
            this.labelGamePlay = new System.Windows.Forms.Label();
            this.verticalScrollBarView2 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.horizontalScrollBarView2 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.horizontalScrollBarView1 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPurchases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGamePlay)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPurchases
            // 
            this.labelPurchases.AutoSize = true;
            this.labelPurchases.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelPurchases.ForeColor = System.Drawing.Color.White;
            this.labelPurchases.Location = new System.Drawing.Point(12, 9);
            this.labelPurchases.Name = "labelPurchases";
            this.labelPurchases.Size = new System.Drawing.Size(139, 16);
            this.labelPurchases.TabIndex = 2;
            this.labelPurchases.Text = "Purchases and Tasks";
            // 
            // dataGridViewPurchases
            // 
            this.dataGridViewPurchases.AllowUserToAddRows = false;
            this.dataGridViewPurchases.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewPurchases.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewPurchases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPurchases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPurchases.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGridViewPurchases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPurchases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcBtnCardActivityTrxPrint});
            this.dataGridViewPurchases.Location = new System.Drawing.Point(12, 28);
            this.dataGridViewPurchases.Name = "dataGridViewPurchases";
            this.dataGridViewPurchases.ReadOnly = true;
            this.dataGridViewPurchases.RowHeadersVisible = false;
            this.dataGridViewPurchases.RowTemplate.ReadOnly = true;
            this.dataGridViewPurchases.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewPurchases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPurchases.Size = new System.Drawing.Size(1079, 229);
            this.dataGridViewPurchases.TabIndex = 3;
            this.dataGridViewPurchases.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPurchases_CellContentClick);
            // 
            // dcBtnCardActivityTrxPrint
            // 
            this.dcBtnCardActivityTrxPrint.HeaderText = "P";
            this.dcBtnCardActivityTrxPrint.Image = global::Parafait_POS.Properties.Resources.printer;
            this.dcBtnCardActivityTrxPrint.Name = "dcBtnCardActivityTrxPrint";
            this.dcBtnCardActivityTrxPrint.ReadOnly = true;
            this.dcBtnCardActivityTrxPrint.Width = 20;
            // 
            // dataGridViewGamePlay
            // 
            this.dataGridViewGamePlay.AllowUserToAddRows = false;
            this.dataGridViewGamePlay.AllowUserToDeleteRows = false;
            this.dataGridViewGamePlay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGamePlay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewGamePlay.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGridViewGamePlay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGamePlay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReverseGamePlay});
            this.dataGridViewGamePlay.Location = new System.Drawing.Point(12, 338);
            this.dataGridViewGamePlay.Name = "dataGridViewGamePlay";
            this.dataGridViewGamePlay.ReadOnly = true;
            this.dataGridViewGamePlay.RowHeadersVisible = false;
            this.dataGridViewGamePlay.RowTemplate.ReadOnly = true;
            this.dataGridViewGamePlay.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewGamePlay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGamePlay.Size = new System.Drawing.Size(1079, 225);
            this.dataGridViewGamePlay.TabIndex = 24;
            this.dataGridViewGamePlay.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGamePlay_CellContentClick);
            // 
            // ReverseGamePlay
            // 
            this.ReverseGamePlay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ReverseGamePlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReverseGamePlay.HeaderText = "X";
            this.ReverseGamePlay.Name = "ReverseGamePlay";
            this.ReverseGamePlay.ReadOnly = true;
            this.ReverseGamePlay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ReverseGamePlay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ReverseGamePlay.Text = "X";
            this.ReverseGamePlay.UseColumnTextForButtonValue = true;
            this.ReverseGamePlay.Width = 20;
            // 
            // labelGamePlay
            // 
            this.labelGamePlay.AutoSize = true;
            this.labelGamePlay.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelGamePlay.ForeColor = System.Drawing.Color.White;
            this.labelGamePlay.Location = new System.Drawing.Point(6, 319);
            this.labelGamePlay.Name = "labelGamePlay";
            this.labelGamePlay.Size = new System.Drawing.Size(84, 16);
            this.labelGamePlay.TabIndex = 25;
            this.labelGamePlay.Text = "Game Plays";
            // 
            // verticalScrollBarView2
            // 
            this.verticalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView2.AutoHide = false;
            this.verticalScrollBarView2.DataGridView = this.dataGridViewGamePlay;
            this.verticalScrollBarView2.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonBackgroundImage")));
            this.verticalScrollBarView2.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView2.Location = new System.Drawing.Point(1094, 338);
            this.verticalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView2.Name = "verticalScrollBarView2";
            this.verticalScrollBarView2.ScrollableControl = null;
            this.verticalScrollBarView2.ScrollViewer = null;
            this.verticalScrollBarView2.Size = new System.Drawing.Size(49, 225);
            this.verticalScrollBarView2.TabIndex = 26;
            this.verticalScrollBarView2.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonBackgroundImage")));
            this.verticalScrollBarView2.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonDisabledBackgroundImage")));
            // 
            // horizontalScrollBarView2
            // 
            this.horizontalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView2.AutoHide = false;
            this.horizontalScrollBarView2.DataGridView = this.dataGridViewGamePlay;
            this.horizontalScrollBarView2.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.LeftButtonBackgroundImage")));
            this.horizontalScrollBarView2.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView2.Location = new System.Drawing.Point(9, 566);
            this.horizontalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView2.Name = "horizontalScrollBarView2";
            this.horizontalScrollBarView2.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.RightButtonBackgroundImage")));
            this.horizontalScrollBarView2.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView2.ScrollableControl = null;
            this.horizontalScrollBarView2.ScrollViewer = null;
            this.horizontalScrollBarView2.Size = new System.Drawing.Size(1082, 40);
            this.horizontalScrollBarView2.TabIndex = 27;
            // 
            // horizontalScrollBarView1
            // 
            this.horizontalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView1.AutoHide = false;
            this.horizontalScrollBarView1.DataGridView = this.dataGridViewPurchases;
            this.horizontalScrollBarView1.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonBackgroundImage")));
            this.horizontalScrollBarView1.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.Location = new System.Drawing.Point(9, 260);
            this.horizontalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView1.Name = "horizontalScrollBarView1";
            this.horizontalScrollBarView1.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonBackgroundImage")));
            this.horizontalScrollBarView1.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.ScrollableControl = null;
            this.horizontalScrollBarView1.ScrollViewer = null;
            this.horizontalScrollBarView1.Size = new System.Drawing.Size(1082, 40);
            this.horizontalScrollBarView1.TabIndex = 28;
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = this.dataGridViewPurchases;
            this.verticalScrollBarView1.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonBackgroundImage")));
            this.verticalScrollBarView1.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView1.Location = new System.Drawing.Point(1094, 28);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.ScrollViewer = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(49, 229);
            this.verticalScrollBarView1.TabIndex = 29;
            this.verticalScrollBarView1.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonBackgroundImage")));
            this.verticalScrollBarView1.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonDisabledBackgroundImage")));
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.AutoEllipsis = true;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(548, 622);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(99, 49);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // frmCardActivityZoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1163, 683);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.verticalScrollBarView1);
            this.Controls.Add(this.horizontalScrollBarView1);
            this.Controls.Add(this.horizontalScrollBarView2);
            this.Controls.Add(this.verticalScrollBarView2);
            this.Controls.Add(this.labelGamePlay);
            this.Controls.Add(this.dataGridViewGamePlay);
            this.Controls.Add(this.dataGridViewPurchases);
            this.Controls.Add(this.labelPurchases);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmCardActivityZoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Card Activities";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPurchases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGamePlay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPurchases;
        private System.Windows.Forms.DataGridView dataGridViewPurchases;
        private System.Windows.Forms.DataGridViewImageColumn dcBtnCardActivityTrxPrint;
        private System.Windows.Forms.DataGridView dataGridViewGamePlay;
        private System.Windows.Forms.DataGridViewButtonColumn ReverseGamePlay;
        private System.Windows.Forms.Label labelGamePlay;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView2;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView2;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView1;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private System.Windows.Forms.Button btnClose;
    }
}