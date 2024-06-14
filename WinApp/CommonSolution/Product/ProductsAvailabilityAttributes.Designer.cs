namespace Semnox.Parafait.Product
{
    partial class ProductsAvailabilityAttributes
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRemainingTill = new System.Windows.Forms.ComboBox();
            this.txtRemainingQuantity = new System.Windows.Forms.TextBox();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPreviousComments = new System.Windows.Forms.Label();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.txtUnavailableTill = new System.Windows.Forms.TextBox();
            this.btnKeyboard = new System.Windows.Forms.Button();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblProdName = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remaining Quantity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(72, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Unavailable Till";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(97, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Comments";
            // 
            // cbRemainingTill
            // 
            this.cbRemainingTill.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRemainingTill.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRemainingTill.FormattingEnabled = true;
            this.cbRemainingTill.Location = new System.Drawing.Point(330, 82);
            this.cbRemainingTill.Name = "cbRemainingTill";
            this.cbRemainingTill.Size = new System.Drawing.Size(135, 23);
            this.cbRemainingTill.TabIndex = 1;
            this.cbRemainingTill.SelectedIndexChanged += new System.EventHandler(this.cbRemainingTill_SelectedIndexChanged);
            this.cbRemainingTill.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // txtRemainingQuantity
            // 
            this.txtRemainingQuantity.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemainingQuantity.Location = new System.Drawing.Point(172, 47);
            this.txtRemainingQuantity.Name = "txtRemainingQuantity";
            this.txtRemainingQuantity.Size = new System.Drawing.Size(293, 21);
            this.txtRemainingQuantity.TabIndex = 0;
            this.txtRemainingQuantity.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // txtComments
            // 
            this.txtComments.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComments.Location = new System.Drawing.Point(172, 118);
            this.txtComments.MaxLength = 500;
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(293, 38);
            this.txtComments.TabIndex = 2;
            this.txtComments.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(172, 236);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(291, 236);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPreviousComments
            // 
            this.lblPreviousComments.AutoSize = true;
            this.lblPreviousComments.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviousComments.Location = new System.Drawing.Point(43, 165);
            this.lblPreviousComments.Name = "lblPreviousComments";
            this.lblPreviousComments.Size = new System.Drawing.Size(120, 15);
            this.lblPreviousComments.TabIndex = 8;
            this.lblPreviousComments.Text = "Previous Comments";
            // 
            // txtHistory
            // 
            this.txtHistory.Enabled = false;
            this.txtHistory.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistory.Location = new System.Drawing.Point(172, 163);
            this.txtHistory.MaxLength = 2000;
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.Size = new System.Drawing.Size(293, 67);
            this.txtHistory.TabIndex = 3;
            // 
            // txtUnavailableTill
            // 
            this.txtUnavailableTill.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnavailableTill.Location = new System.Drawing.Point(172, 82);
            this.txtUnavailableTill.Name = "txtUnavailableTill";
            this.txtUnavailableTill.ReadOnly = true;
            this.txtUnavailableTill.Size = new System.Drawing.Size(152, 21);
            this.txtUnavailableTill.TabIndex = 10;
            // 
            // btnKeyboard
            // 
            this.btnKeyboard.BackgroundImage = global::Semnox.Parafait.Product.Properties.Resources.keyboard;
            this.btnKeyboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnKeyboard.Location = new System.Drawing.Point(472, 230);
            this.btnKeyboard.Name = "btnKeyboard";
            this.btnKeyboard.Size = new System.Drawing.Size(35, 35);
            this.btnKeyboard.TabIndex = 11;
            this.btnKeyboard.UseVisualStyleBackColor = true;
            this.btnKeyboard.Click += new System.EventHandler(this.btnKeypad_Click);
            // 
            // txtProductName
            // 
            this.txtProductName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductName.Location = new System.Drawing.Point(172, 12);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.ReadOnly = true;
            this.txtProductName.Size = new System.Drawing.Size(293, 21);
            this.txtProductName.TabIndex = 12;
            // 
            // lblProdName
            // 
            this.lblProdName.AutoSize = true;
            this.lblProdName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdName.Location = new System.Drawing.Point(77, 14);
            this.lblProdName.Name = "lblProdName";
            this.lblProdName.Size = new System.Drawing.Size(86, 15);
            this.lblProdName.TabIndex = 13;
            this.lblProdName.Text = "Product Name";
            // 
            // ProductsAvailabilityAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 271);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProdName);
            this.Controls.Add(this.btnKeyboard);
            this.Controls.Add(this.txtUnavailableTill);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.lblPreviousComments);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.txtRemainingQuantity);
            this.Controls.Add(this.cbRemainingTill);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProductsAvailabilityAttributes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Availability Attributes";
            this.Load += new System.EventHandler(this.ProductsAvailabilityAttributes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRemainingTill;
        private System.Windows.Forms.TextBox txtRemainingQuantity;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPreviousComments;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.TextBox txtUnavailableTill;
        private System.Windows.Forms.Button btnKeyboard;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblProdName;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}