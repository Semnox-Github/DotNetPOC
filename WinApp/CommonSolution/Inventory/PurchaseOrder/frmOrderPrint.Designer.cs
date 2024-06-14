namespace Semnox.Parafait.Inventory
{
    partial class frmOrderPrint
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbFinal = new System.Windows.Forms.RadioButton();
            this.rbDraft = new System.Windows.Forms.RadioButton();
            this.cb_print = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbFinal);
            this.groupBox1.Controls.Add(this.rbDraft);
            this.groupBox1.Location = new System.Drawing.Point(28, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Document Type";
            // 
            // rbFinal
            // 
            this.rbFinal.AutoSize = true;
            this.rbFinal.Location = new System.Drawing.Point(116, 43);
            this.rbFinal.Name = "rbFinal";
            this.rbFinal.Size = new System.Drawing.Size(47, 17);
            this.rbFinal.TabIndex = 1;
            this.rbFinal.TabStop = true;
            this.rbFinal.Text = "Final";
            this.rbFinal.UseVisualStyleBackColor = true;
            // 
            // rbDraft
            // 
            this.rbDraft.AutoSize = true;
            this.rbDraft.Location = new System.Drawing.Point(40, 43);
            this.rbDraft.Name = "rbDraft";
            this.rbDraft.Size = new System.Drawing.Size(48, 17);
            this.rbDraft.TabIndex = 0;
            this.rbDraft.TabStop = true;
            this.rbDraft.Text = "Draft";
            this.rbDraft.UseVisualStyleBackColor = true;
            // 
            // cb_print
            // 
            this.cb_print.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_print.Image = global::Semnox.Parafait.Inventory.Properties.Resources.printer;
            this.cb_print.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_print.Location = new System.Drawing.Point(32, 125);
            this.cb_print.Name = "cb_print";
            this.cb_print.Size = new System.Drawing.Size(90, 23);
            this.cb_print.TabIndex = 40;
            this.cb_print.Text = "Print";
            this.cb_print.UseVisualStyleBackColor = true;
            this.cb_print.Click += new System.EventHandler(this.cb_print_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(163, 126);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 41;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 169);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cb_print);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmPrint";
            this.Text = "Print";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbFinal;
        private System.Windows.Forms.RadioButton rbDraft;
        private System.Windows.Forms.Button cb_print;
        private System.Windows.Forms.Button btnClose;
    }
}