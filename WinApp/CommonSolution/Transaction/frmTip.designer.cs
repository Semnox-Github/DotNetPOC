namespace Semnox.Parafait.Transaction
{

    partial class frmTip
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
            this.flpTenders = new System.Windows.Forms.FlowLayoutPanel();
            this.gbCash = new System.Windows.Forms.GroupBox();
            this.btnCash = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gbCreditCard = new System.Windows.Forms.GroupBox();
            this.btnCreditCard = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gpUser = new System.Windows.Forms.GroupBox();
            this.cbxlUser = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.flpTenders.SuspendLayout();
            this.gbCash.SuspendLayout();
            this.gbCreditCard.SuspendLayout();
            this.gpUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpTenders
            // 
            this.flpTenders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpTenders.Controls.Add(this.gbCash);
            this.flpTenders.Controls.Add(this.gbCreditCard);
            this.flpTenders.Controls.Add(this.gpUser);
            this.flpTenders.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTenders.Location = new System.Drawing.Point(265, 0);
            this.flpTenders.Name = "flpTenders";
            this.flpTenders.Size = new System.Drawing.Size(186, 389);
            this.flpTenders.TabIndex = 0;
            // 
            // gbCash
            // 
            this.gbCash.BackColor = System.Drawing.Color.Transparent;
            this.gbCash.Controls.Add(this.btnCash);
            this.gbCash.Controls.Add(this.label1);
            this.gbCash.Location = new System.Drawing.Point(3, 3);
            this.gbCash.Name = "gbCash";
            this.gbCash.Size = new System.Drawing.Size(175, 60);
            this.gbCash.TabIndex = 6;
            this.gbCash.TabStop = false;
            // 
            // btnCash
            // 
            this.btnCash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCash.Location = new System.Drawing.Point(6, 15);
            this.btnCash.Name = "btnCash";
            this.btnCash.Size = new System.Drawing.Size(47, 39);
            this.btnCash.TabIndex = 4;
            this.btnCash.UseVisualStyleBackColor = true;
            this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(53, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 38);
            this.label1.TabIndex = 5;
            this.label1.Text = "Cash";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.btnCash_Click);
            // 
            // gbCreditCard
            // 
            this.gbCreditCard.BackColor = System.Drawing.Color.Transparent;
            this.gbCreditCard.Controls.Add(this.btnCreditCard);
            this.gbCreditCard.Controls.Add(this.label2);
            this.gbCreditCard.Location = new System.Drawing.Point(3, 69);
            this.gbCreditCard.Name = "gbCreditCard";
            this.gbCreditCard.Size = new System.Drawing.Size(175, 60);
            this.gbCreditCard.TabIndex = 7;
            this.gbCreditCard.TabStop = false;
            // 
            // btnCreditCard
            // 
            this.btnCreditCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCreditCard.Location = new System.Drawing.Point(6, 15);
            this.btnCreditCard.Name = "btnCreditCard";
            this.btnCreditCard.Size = new System.Drawing.Size(47, 39);
            this.btnCreditCard.TabIndex = 4;
            this.btnCreditCard.UseVisualStyleBackColor = true;
            this.btnCreditCard.Click += new System.EventHandler(this.btnCreditCard_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(50, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 38);
            this.label2.TabIndex = 5;
            this.label2.Text = "Credit Card";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.btnCreditCard_Click);
            // 
            // gpUser
            // 
            this.gpUser.BackColor = System.Drawing.Color.Transparent;
            this.gpUser.Controls.Add(this.cbxlUser);
            this.gpUser.Controls.Add(this.label3);
            this.gpUser.Location = new System.Drawing.Point(3, 135);
            this.gpUser.Name = "gpUser";
            this.gpUser.Size = new System.Drawing.Size(175, 210);
            this.gpUser.TabIndex = 8;
            this.gpUser.TabStop = false;
            // 
            // cbxlUser
            // 
            this.cbxlUser.BackColor = System.Drawing.Color.Gainsboro;
            this.cbxlUser.CheckOnClick = true;
            this.cbxlUser.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.cbxlUser.FormattingEnabled = true;
            this.cbxlUser.Location = new System.Drawing.Point(6, 55);
            this.cbxlUser.Name = "cbxlUser";
            this.cbxlUser.Size = new System.Drawing.Size(163, 148);
            this.cbxlUser.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(29, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 38);
            this.label3.TabIndex = 5;
            this.label3.Text = "Users";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = Properties.Resources.customer_button_normal;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(6, 318);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(258, 61);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseUp);
            // 
            // frmTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(455, 389);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.flpTenders);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmTip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tip Amount";
            this.flpTenders.ResumeLayout(false);
            this.gbCash.ResumeLayout(false);
            this.gbCreditCard.ResumeLayout(false);
            this.gpUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpTenders;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbCash;
        private System.Windows.Forms.Button btnCash;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbCreditCard;
        private System.Windows.Forms.Button btnCreditCard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gpUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox cbxlUser;
    }
}