namespace Parafait_POS.Reservation
{
    partial class frmAdditionalProductsDecomissioned
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.flpModifierSets = new System.Windows.Forms.FlowLayoutPanel();
            this.sampleModifierSetButton = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvSelected = new System.Windows.Forms.DataGridView();
            this.dcSelected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpModifierList = new System.Windows.Forms.FlowLayoutPanel();
            this.sampleModifierProduct = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.flpModifierSets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).BeginInit();
            this.flpModifierList.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.Color.Azure;
            this.splitContainer.Panel1.Controls.Add(this.flpModifierSets);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.Color.Azure;
            this.splitContainer.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer.Panel2.Controls.Add(this.dgvSelected);
            this.splitContainer.Panel2.Controls.Add(this.flpModifierList);
            this.splitContainer.Panel2.Controls.Add(this.btnClose);
            this.splitContainer.Size = new System.Drawing.Size(752, 534);
            this.splitContainer.SplitterDistance = 225;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 0;
            // 
            // flpModifierSets
            // 
            this.flpModifierSets.Controls.Add(this.sampleModifierSetButton);
            this.flpModifierSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpModifierSets.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpModifierSets.Location = new System.Drawing.Point(0, 0);
            this.flpModifierSets.Name = "flpModifierSets";
            this.flpModifierSets.Size = new System.Drawing.Size(225, 534);
            this.flpModifierSets.TabIndex = 2;
            // 
            // sampleModifierSetButton
            // 
            this.sampleModifierSetButton.BackColor = System.Drawing.Color.Transparent;
            this.sampleModifierSetButton.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.sampleModifierSetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleModifierSetButton.FlatAppearance.BorderSize = 0;
            this.sampleModifierSetButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleModifierSetButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleModifierSetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleModifierSetButton.ForeColor = System.Drawing.Color.White;
            this.sampleModifierSetButton.Location = new System.Drawing.Point(4, 4);
            this.sampleModifierSetButton.Margin = new System.Windows.Forms.Padding(4);
            this.sampleModifierSetButton.Name = "sampleModifierSetButton";
            this.sampleModifierSetButton.Size = new System.Drawing.Size(217, 62);
            this.sampleModifierSetButton.TabIndex = 1;
            this.sampleModifierSetButton.Text = "Sample";
            this.sampleModifierSetButton.UseVisualStyleBackColor = false;
            this.sampleModifierSetButton.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.button_normal;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(289, 484);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 42);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // dgvSelected
            // 
            this.dgvSelected.AllowUserToAddRows = false;
            this.dgvSelected.AllowUserToDeleteRows = false;
            this.dgvSelected.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelected.BackgroundColor = System.Drawing.Color.Azure;
            this.dgvSelected.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelected.ColumnHeadersVisible = false;
            this.dgvSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcSelected});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Azure;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Azure;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelected.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSelected.GridColor = System.Drawing.Color.Azure;
            this.dgvSelected.Location = new System.Drawing.Point(2, 373);
            this.dgvSelected.Name = "dgvSelected";
            this.dgvSelected.RowHeadersVisible = false;
            this.dgvSelected.Size = new System.Drawing.Size(518, 105);
            this.dgvSelected.TabIndex = 2;
            // 
            // dcSelected
            // 
            this.dcSelected.HeaderText = "Selected";
            this.dcSelected.Name = "dcSelected";
            this.dcSelected.ReadOnly = true;
            // 
            // flpModifierList
            // 
            this.flpModifierList.AutoScroll = true;
            this.flpModifierList.Controls.Add(this.sampleModifierProduct);
            this.flpModifierList.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpModifierList.Location = new System.Drawing.Point(0, 0);
            this.flpModifierList.Name = "flpModifierList";
            this.flpModifierList.Size = new System.Drawing.Size(522, 370);
            this.flpModifierList.TabIndex = 1;
            // 
            // sampleModifierProduct
            // 
            this.sampleModifierProduct.BackColor = System.Drawing.Color.Transparent;
            this.sampleModifierProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.ComboProduct;
            this.sampleModifierProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleModifierProduct.FlatAppearance.BorderSize = 0;
            this.sampleModifierProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleModifierProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleModifierProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleModifierProduct.ForeColor = System.Drawing.Color.White;
            this.sampleModifierProduct.Location = new System.Drawing.Point(4, 4);
            this.sampleModifierProduct.Margin = new System.Windows.Forms.Padding(4);
            this.sampleModifierProduct.Name = "sampleModifierProduct";
            this.sampleModifierProduct.Size = new System.Drawing.Size(170, 113);
            this.sampleModifierProduct.TabIndex = 2;
            this.sampleModifierProduct.Text = "Sample";
            this.sampleModifierProduct.UseVisualStyleBackColor = false;
            this.sampleModifierProduct.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.button_normal;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(84, 484);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(138, 42);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // frmAdditionalProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(752, 534);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAdditionalProducts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Modifiers";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.flpModifierSets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).EndInit();
            this.flpModifierList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.FlowLayoutPanel flpModifierSets;
        private System.Windows.Forms.FlowLayoutPanel flpModifierList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button sampleModifierSetButton;
        private System.Windows.Forms.Button sampleModifierProduct;
        private System.Windows.Forms.DataGridView dgvSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSelected;
        private System.Windows.Forms.Button btnCancel;
    }
}