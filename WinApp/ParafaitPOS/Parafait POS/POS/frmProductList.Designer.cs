namespace Parafait_POS
{
    partial class frmProductList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductList));
            this.flpProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSample = new System.Windows.Forms.Button();
            this.nudSample = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblText = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.flpProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSample)).BeginInit();
            this.SuspendLayout();
            // 
            // flpProducts
            // 
            this.flpProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpProducts.AutoScroll = true;
            this.flpProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpProducts.Controls.Add(this.btnSample);
            this.flpProducts.Controls.Add(this.nudSample);
            this.flpProducts.Location = new System.Drawing.Point(16, 58);
            this.flpProducts.Name = "flpProducts";
            this.flpProducts.Size = new System.Drawing.Size(413, 576);
            this.flpProducts.TabIndex = 0;
            // 
            // btnSample
            // 
            this.btnSample.BackColor = System.Drawing.Color.Coral;
            this.btnSample.FlatAppearance.BorderColor = System.Drawing.Color.Coral;
            this.btnSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.OrangeRed;
            this.btnSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSalmon;
            this.btnSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSample.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSample.Location = new System.Drawing.Point(3, 3);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(301, 54);
            this.btnSample.TabIndex = 0;
            this.btnSample.Text = "button1";
            this.btnSample.UseVisualStyleBackColor = false;
            // 
            // nudSample
            // 
            this.nudSample.Font = new System.Drawing.Font("Tahoma", 29F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSample.Location = new System.Drawing.Point(310, 3);
            this.nudSample.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSample.Name = "nudSample";
            this.nudSample.Size = new System.Drawing.Size(72, 54);
            this.nudSample.TabIndex = 1;
            this.nudSample.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(16, 645);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(104, 50);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(325, 645);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 50);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // lblText
            // 
            this.lblText.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.Location = new System.Drawing.Point(16, 1);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(413, 25);
            this.lblText.TabIndex = 7;
            this.lblText.Text = "Select x y category products ";
            this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSelected
            // 
            this.lblSelected.BackColor = System.Drawing.Color.RosyBrown;
            this.lblSelected.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelected.ForeColor = System.Drawing.Color.White;
            this.lblSelected.Location = new System.Drawing.Point(16, 29);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(413, 25);
            this.lblSelected.TabIndex = 8;
            this.lblSelected.Text = "0 Selected";
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmProductList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 706);
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.flpProducts);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmProductList";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product List";
            this.Load += new System.EventHandler(this.frmProductList_Load);
            this.flpProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.NumericUpDown nudSample;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label lblSelected;
    }
}