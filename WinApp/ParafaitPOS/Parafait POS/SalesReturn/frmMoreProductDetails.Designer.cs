namespace Parafait_POS.SalesReturn
{
    partial class frmMoreProductDetails
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblHeading = new System.Windows.Forms.Label();
            this.btnPLClose = new System.Windows.Forms.Button();
            this.dgvProductDetails = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblHeading.Location = new System.Drawing.Point(3, 3);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(104, 16);
            this.lblHeading.TabIndex = 75;
            this.lblHeading.Text = "Product Details";
            // 
            // btnPLClose
            // 
            this.btnPLClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPLClose.BackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPLClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPLClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPLClose.FlatAppearance.BorderSize = 0;
            this.btnPLClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPLClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPLClose.ForeColor = System.Drawing.Color.White;
            this.btnPLClose.Location = new System.Drawing.Point(145, 358);
            this.btnPLClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPLClose.Name = "btnPLClose";
            this.btnPLClose.Size = new System.Drawing.Size(155, 55);
            this.btnPLClose.TabIndex = 131;
            this.btnPLClose.Text = "Close";
            this.btnPLClose.UseVisualStyleBackColor = false;
            this.btnPLClose.Click += new System.EventHandler(this.btnPLClose_Click);
            // 
            // dgvProductDetails
            // 
            this.dgvProductDetails.AllowUserToAddRows = false;
            this.dgvProductDetails.AllowUserToDeleteRows = false;
            this.dgvProductDetails.AllowUserToResizeColumns = false;
            this.dgvProductDetails.AllowUserToResizeRows = false;
            this.dgvProductDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductDetails.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvProductDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvProductDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProductDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProductDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProductDetails.EnableHeadersVisualStyles = false;
            this.dgvProductDetails.Location = new System.Drawing.Point(33, 41);
            this.dgvProductDetails.Name = "dgvProductDetails";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProductDetails.RowHeadersVisible = false;
            this.dgvProductDetails.RowTemplate.Height = 40;
            this.dgvProductDetails.Size = new System.Drawing.Size(400, 300);
            this.dgvProductDetails.TabIndex = 132;
            // 
            // frmMoreProductDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(484, 431);
            this.ControlBox = false;
            this.Controls.Add(this.dgvProductDetails);
            this.Controls.Add(this.btnPLClose);
            this.Controls.Add(this.lblHeading);
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMoreProductDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Details";
            this.Load += new System.EventHandler(this.frmProductDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Button btnPLClose;
        private System.Windows.Forms.DataGridView dgvProductDetails;
    }
}