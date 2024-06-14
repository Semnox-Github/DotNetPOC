namespace Semnox.Parafait.Discounts
{
    partial class TrxLineDiscountsUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrxLineDiscountsUI));
            this.flpDiscounts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSample = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblDiscountSelected = new System.Windows.Forms.Label();
            this.flpDiscounts.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpDiscounts
            // 
            this.flpDiscounts.AutoScroll = true;
            this.flpDiscounts.Controls.Add(this.btnSample);
            this.flpDiscounts.Location = new System.Drawing.Point(30, 12);
            this.flpDiscounts.Name = "flpDiscounts";
            this.flpDiscounts.Size = new System.Drawing.Size(583, 228);
            this.flpDiscounts.TabIndex = 5;
            // 
            // btnSample
            // 
            this.btnSample.BackColor = System.Drawing.Color.Transparent;
            this.btnSample.BackgroundImage = global::Semnox.Parafait.Discounts.Properties.Resources.discount_button;
            this.btnSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSample.FlatAppearance.BorderSize = 0;
            this.btnSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSample.ForeColor = System.Drawing.Color.White;
            this.btnSample.Location = new System.Drawing.Point(6, 6);
            this.btnSample.Margin = new System.Windows.Forms.Padding(6, 6, 3, 3);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(103, 99);
            this.btnSample.TabIndex = 4;
            this.btnSample.Text = "Discount";
            this.btnSample.UseVisualStyleBackColor = false;
            this.btnSample.Visible = false;
            this.btnSample.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(329, 282);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(177, 59);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.BackColor = System.Drawing.Color.Transparent;
            this.btnApply.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnApply.BackgroundImage")));
            this.btnApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(110, 282);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(177, 59);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblDiscountSelected
            // 
            this.lblDiscountSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountSelected.ForeColor = System.Drawing.Color.Brown;
            this.lblDiscountSelected.Location = new System.Drawing.Point(35, 243);
            this.lblDiscountSelected.Name = "lblDiscountSelected";
            this.lblDiscountSelected.Size = new System.Drawing.Size(573, 35);
            this.lblDiscountSelected.TabIndex = 6;
            // 
            // TrxLineDiscountsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(645, 361);
            this.Controls.Add(this.lblDiscountSelected);
            this.Controls.Add(this.flpDiscounts);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TrxLineDiscountsUI";
            this.Text = "TrxLineDiscountsUI";
            this.Load += new System.EventHandler(this.TrxLineDiscountsUI_Load);
            this.flpDiscounts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.FlowLayoutPanel flpDiscounts;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Label lblDiscountSelected;
    }
}