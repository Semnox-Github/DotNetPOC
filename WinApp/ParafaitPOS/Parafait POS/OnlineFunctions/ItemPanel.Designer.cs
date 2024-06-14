namespace Parafait_POS
{
    partial class ItemPanel
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
            this.lblName = new System.Windows.Forms.Label();
            this.nuQuantity = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nuQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(7, 8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(195, 41);
            this.lblName.TabIndex = 0;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nuQuantity
            // 
            this.nuQuantity.Font = new System.Drawing.Font("Tahoma", 29F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nuQuantity.Location = new System.Drawing.Point(212, 2);
            this.nuQuantity.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nuQuantity.Name = "nuQuantity";
            this.nuQuantity.Size = new System.Drawing.Size(104, 54);
            this.nuQuantity.TabIndex = 2;
            this.nuQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nuQuantity.ValueChanged += new System.EventHandler(this.nuQuantity_ValueChanged);
            // 
            // ItemPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.nuQuantity);
            this.Controls.Add(this.lblName);
            this.Name = "ItemPanel";
            this.Size = new System.Drawing.Size(325, 59);
            ((System.ComponentModel.ISupportInitialize)(this.nuQuantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown nuQuantity;
    }
}
