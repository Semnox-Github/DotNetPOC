namespace Parafait_Kiosk
{
    partial class usrCtrlCart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flpCartProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.panelCartProducts = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblDeleteBtnExtender = new System.Windows.Forms.Label();
            this.lblTotalPrice = new System.Windows.Forms.Label();
            this.lblProductDescription = new System.Windows.Forms.Label();
            this.flpCartProducts.SuspendLayout();
            this.panelCartProducts.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpCartProducts
            // 
            this.flpCartProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpCartProducts.Controls.Add(this.panelCartProducts);
            this.flpCartProducts.Location = new System.Drawing.Point(0, 3);
            this.flpCartProducts.MinimumSize = new System.Drawing.Size(824, 50);
            this.flpCartProducts.Name = "flpCartProducts";
            this.flpCartProducts.Size = new System.Drawing.Size(830, 61);
            this.flpCartProducts.TabIndex = 5;
            // 
            // panelCartProducts
            // 
            this.panelCartProducts.BackColor = System.Drawing.Color.Transparent;
            this.panelCartProducts.Controls.Add(this.btnDelete);
            this.panelCartProducts.Controls.Add(this.lblDeleteBtnExtender);
            this.panelCartProducts.Controls.Add(this.lblTotalPrice);
            this.panelCartProducts.Controls.Add(this.lblProductDescription);
            this.panelCartProducts.Location = new System.Drawing.Point(3, 3);
            this.panelCartProducts.Name = "panelCartProducts";
            this.panelCartProducts.Size = new System.Drawing.Size(822, 56);
            this.panelCartProducts.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.NewDeleteButton;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDelete.Location = new System.Drawing.Point(770, 3);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(47, 47);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Clicked);
            // 
            // lblDeleteBtnExtender
            // 
            this.lblDeleteBtnExtender.BackColor = System.Drawing.Color.Transparent;
            this.lblDeleteBtnExtender.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeleteBtnExtender.Location = new System.Drawing.Point(742, 5);
            this.lblDeleteBtnExtender.Name = "lblDeleteBtnExtender";
            this.lblDeleteBtnExtender.Size = new System.Drawing.Size(80, 47);
            this.lblDeleteBtnExtender.TabIndex = 9;
            this.lblDeleteBtnExtender.Click += new System.EventHandler(this.btnDelete_Clicked);
            // 
            // lblTotalPrice
            // 
            this.lblTotalPrice.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalPrice.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPrice.Location = new System.Drawing.Point(555, 3);
            this.lblTotalPrice.Name = "lblTotalPrice";
            this.lblTotalPrice.Size = new System.Drawing.Size(214, 47);
            this.lblTotalPrice.TabIndex = 8;
            this.lblTotalPrice.Text = "$800000.00";
            // 
            // lblProductDescription
            // 
            this.lblProductDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblProductDescription.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductDescription.Location = new System.Drawing.Point(1, 3);
            this.lblProductDescription.Name = "lblProductDescription";
            this.lblProductDescription.Size = new System.Drawing.Size(554, 47);
            this.lblProductDescription.TabIndex = 5;
            this.lblProductDescription.Text = "1 x 1$ Card + 10 ";
            // 
            // usrCtrlCart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.WhiteBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.flpCartProducts);
            this.DoubleBuffered = true;
            this.Name = "usrCtrlCart";
            this.Size = new System.Drawing.Size(835, 67);
            this.flpCartProducts.ResumeLayout(false);
            this.panelCartProducts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpCartProducts;
        private System.Windows.Forms.Panel panelCartProducts;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblProductDescription;
        private System.Windows.Forms.Label lblTotalPrice;
        private System.Windows.Forms.Label lblDeleteBtnExtender;
    }
}
