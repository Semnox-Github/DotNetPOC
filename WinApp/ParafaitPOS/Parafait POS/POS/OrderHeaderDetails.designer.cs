namespace Parafait_POS
{
    partial class OrderHeaderDetails
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTableNumber = new System.Windows.Forms.TextBox();
            this.txtWaiterName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCustomername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Table Number:";
            // 
            // txtTableNumber
            // 
            this.txtTableNumber.Location = new System.Drawing.Point(145, 48);
            this.txtTableNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTableNumber.Name = "txtTableNumber";
            this.txtTableNumber.Size = new System.Drawing.Size(181, 22);
            this.txtTableNumber.TabIndex = 1;
            this.txtTableNumber.Enter += new System.EventHandler(this.txtTableNumber_Enter);
            // 
            // txtWaiterName
            // 
            this.txtWaiterName.Location = new System.Drawing.Point(145, 80);
            this.txtWaiterName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWaiterName.Name = "txtWaiterName";
            this.txtWaiterName.Size = new System.Drawing.Size(181, 22);
            this.txtWaiterName.TabIndex = 3;
            this.txtWaiterName.Enter += new System.EventHandler(this.txtWaiterName_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Waiter Name:";
            // 
            // txtCustomername
            // 
            this.txtCustomername.Location = new System.Drawing.Point(145, 112);
            this.txtCustomername.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCustomername.Name = "txtCustomername";
            this.txtCustomername.Size = new System.Drawing.Size(181, 22);
            this.txtCustomername.TabIndex = 5;
            this.txtCustomername.Enter += new System.EventHandler(this.txtCustomername_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Customer Name:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(145, 144);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(181, 22);
            this.txtRemarks.TabIndex = 7;
            this.txtRemarks.Enter += new System.EventHandler(this.txtRemarks_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Remarks:";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(145, 205);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 42);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(254, 205);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 42);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MintCream;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowNumPad.Location = new System.Drawing.Point(345, 213);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 21;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // OrderHeaderDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(382, 259);
            this.Controls.Add(this.btnShowNumPad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCustomername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWaiterName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTableNumber);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OrderHeaderDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Information";
            this.Load += new System.EventHandler(this.OrderHeaderDetails_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OrderHeaderDetails_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTableNumber;
        private System.Windows.Forms.TextBox txtWaiterName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCustomername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowNumPad;
    }
}