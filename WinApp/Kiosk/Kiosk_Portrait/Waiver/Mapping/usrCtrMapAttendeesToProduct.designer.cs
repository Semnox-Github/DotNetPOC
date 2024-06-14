using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlMapAttendeesToProduct
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
            this.btnProduct = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnProduct
            // 
            this.btnProduct.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.SmallCircleUnselected;
            this.btnProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProduct.FlatAppearance.BorderSize = 0;
            this.btnProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProduct.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProduct.ForeColor = System.Drawing.Color.Black;
            this.btnProduct.Location = new System.Drawing.Point(0, 0);
            this.btnProduct.Margin = new System.Windows.Forms.Padding(0);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(68, 60);
            this.btnProduct.TabIndex = 4;
            this.btnProduct.Text = "1";
            this.btnProduct.UseVisualStyleBackColor = true;
            // 
            // UsrCtrlMapAttendeesToProduct
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btnProduct);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UsrCtrlMapAttendeesToProduct";
            this.Size = new System.Drawing.Size(68, 60);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnProduct;
    }
}
