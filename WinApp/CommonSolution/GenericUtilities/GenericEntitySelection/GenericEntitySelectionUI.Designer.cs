namespace Semnox.Core.GenericUtilities
{
    partial class GenericEntitySelectionUI<T>
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
            this.dgvEntityList = new System.Windows.Forms.DataGridView();
            this.dgvSelectButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbEntityProperty = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.chbValue = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.genericEntityverticalScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntityList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvEntityList
            // 
            this.dgvEntityList.AllowUserToAddRows = false;
            this.dgvEntityList.AllowUserToDeleteRows = false;
            this.dgvEntityList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEntityList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvEntityList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntityList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvSelectButtonColumn});
            this.dgvEntityList.Location = new System.Drawing.Point(12, 68);
            this.dgvEntityList.Name = "dgvEntityList";
            this.dgvEntityList.ReadOnly = true;
            this.dgvEntityList.RowTemplate.Height = 50;
            this.dgvEntityList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEntityList.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvEntityList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEntityList.Size = new System.Drawing.Size(749, 348);
            this.dgvEntityList.TabIndex = 0;
            this.dgvEntityList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEntityList_CellClick);
            this.dgvEntityList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEntityList_CellDoubleClick);
            this.dgvEntityList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvEntityList_DataError);
            // 
            // dgvSelectButtonColumn
            // 
            this.dgvSelectButtonColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvSelectButtonColumn.HeaderText = "";
            this.dgvSelectButtonColumn.Name = "dgvSelectButtonColumn";
            this.dgvSelectButtonColumn.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbValue);
            this.groupBox1.Controls.Add(this.cmbEntityProperty);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtValue);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(803, 65);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // cmbEntityProperty
            // 
            this.cmbEntityProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEntityProperty.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbEntityProperty.FormattingEnabled = true;
            this.cmbEntityProperty.Location = new System.Drawing.Point(12, 21);
            this.cmbEntityProperty.Name = "cmbEntityProperty";
            this.cmbEntityProperty.Size = new System.Drawing.Size(138, 31);
            this.cmbEntityProperty.TabIndex = 5;
            this.cmbEntityProperty.SelectedValueChanged += new System.EventHandler(this.cmbEntityProperty_SelectedValueChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 15F);
            this.btnSearch.Location = new System.Drawing.Point(365, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 40);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtValue
            // 
            this.txtValue.Font = new System.Drawing.Font("Arial", 15F);
            this.txtValue.Location = new System.Drawing.Point(156, 22);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(136, 30);
            this.txtValue.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 15F);
            this.btnClose.Location = new System.Drawing.Point(365, 422);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chbValue
            // 
            this.chbValue.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbValue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chbValue.FlatAppearance.BorderSize = 0;
            this.chbValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chbValue.ImageIndex = 1;
            this.chbValue.Location = new System.Drawing.Point(156, 22);
            this.chbValue.Name = "chbValue";
            this.chbValue.Size = new System.Drawing.Size(30, 30);
            this.chbValue.TabIndex = 6;
            this.chbValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbValue.UseVisualStyleBackColor = true;
            this.chbValue.Visible = false;
            // 
            // genericEntityverticalScrollBarView
            // 
            this.genericEntityverticalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.genericEntityverticalScrollBarView.AutoHide = false;
            this.genericEntityverticalScrollBarView.BackColor = System.Drawing.Color.Azure;
            this.genericEntityverticalScrollBarView.DataGridView = this.dgvEntityList;
            this.genericEntityverticalScrollBarView.DownButtonBackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Down_Button;
            this.genericEntityverticalScrollBarView.DownButtonDisabledBackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Down_Button_Disabled;
            this.genericEntityverticalScrollBarView.Location = new System.Drawing.Point(762, 68);
            this.genericEntityverticalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.genericEntityverticalScrollBarView.Name = "genericEntityverticalScrollBarView";
            this.genericEntityverticalScrollBarView.ScrollableControl = null;
            this.genericEntityverticalScrollBarView.ScrollViewer = null;
            this.genericEntityverticalScrollBarView.Size = new System.Drawing.Size(40, 348);
            this.genericEntityverticalScrollBarView.TabIndex = 4;
            this.genericEntityverticalScrollBarView.UpButtonBackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Up_Button;
            this.genericEntityverticalScrollBarView.UpButtonDisabledBackgroundImage = global::Semnox.Core.GenericUtilities.Properties.Resources.Up_Button_Disabled;
            // 
            // GenericEntitySelectionUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(803, 467);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.genericEntityverticalScrollBarView);
            this.Controls.Add(this.dgvEntityList);
            this.Name = "GenericEntitySelectionUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generic Entity Selection";
            this.Resize += new System.EventHandler(this.GenericEntitySelectionUI_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntityList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvEntityList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbEntityProperty;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtValue;
        private CustomCheckBox chbValue;
        private System.Windows.Forms.DataGridViewButtonColumn dgvSelectButtonColumn;
        private VerticalScrollBarView genericEntityverticalScrollBarView;
        private System.Windows.Forms.Button btnClose;
    }
}