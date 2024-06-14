using System.Windows.Forms;

namespace Semnox.Parafait.KioskCore
{
    partial class frmPreLoader
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
            this.btnWait = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWait
            // 
            this.btnWait.BackColor = System.Drawing.Color.Transparent;
            this.btnWait.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnWait.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWait.FlatAppearance.BorderSize = 0;
            this.btnWait.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnWait.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnWait.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWait.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWait.ForeColor = System.Drawing.Color.White;
            this.btnWait.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWait.Location = new System.Drawing.Point(0, 0);
            this.btnWait.Name = "btnWait";
            this.btnWait.Size = new System.Drawing.Size(584, 201);
            this.btnWait.TabIndex = 2;
            this.btnWait.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWait.UseVisualStyleBackColor = false;
            // 
            // frmPreLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(584, 201);
            this.Controls.Add(this.btnWait);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPreLoader";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPreLoader";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.ResumeLayout(false);

        }

        #endregion
                   
            
        private Button btnWait;
    }
}