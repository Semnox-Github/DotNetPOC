namespace Semnox.Core.GenericUtilities
{
    partial class CustomerPhoto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerPhoto));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbCapture = new System.Windows.Forms.PictureBox();
            this.pbCurrentImage = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.pbCapture);
            this.groupBox1.Controls.Add(this.pbCurrentImage);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.pbPhoto);
            this.groupBox1.Location = new System.Drawing.Point(2, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(932, 426);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Semnox.Core.GenericUtilities.Resources.Delete;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(844, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 60);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // pbCapture
            // 
            this.pbCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbCapture.Image = ((System.Drawing.Image)(resources.GetObject("pbCapture.Image")));
            this.pbCapture.Location = new System.Drawing.Point(775, 225);
            this.pbCapture.Name = "pbCapture";
            this.pbCapture.Size = new System.Drawing.Size(120, 120);
            this.pbCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCapture.TabIndex = 15;
            this.pbCapture.TabStop = false;
            this.pbCapture.Click += new System.EventHandler(this.pbCapture_Click);
            // 
            // pbCurrentImage
            // 
            this.pbCurrentImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbCurrentImage.Image = global::Semnox.Core.GenericUtilities.Resources.profile_picture_placeholder;
            this.pbCurrentImage.Location = new System.Drawing.Point(746, 9);
            this.pbCurrentImage.Name = "pbCurrentImage";
            this.pbCurrentImage.Size = new System.Drawing.Size(180, 180);
            this.pbCurrentImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCurrentImage.TabIndex = 16;
            this.pbCurrentImage.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Semnox.Core.GenericUtilities.Resources.CheckMark;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(751, 359);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 60);
            this.btnOK.TabIndex = 16;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pbPhoto
            // 
            this.pbPhoto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbPhoto.Image = global::Semnox.Core.GenericUtilities.Resources.profile_picture_placeholder;
            this.pbPhoto.Location = new System.Drawing.Point(8, 9);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(730, 410);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPhoto.TabIndex = 0;
            this.pbPhoto.TabStop = false;
            this.pbPhoto.Paint += new System.Windows.Forms.PaintEventHandler(this.pbPhoto_Paint);
            this.pbPhoto.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbPhoto_MouseDown);
            this.pbPhoto.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbPhoto_MouseMove);
            this.pbPhoto.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbPhoto_MouseUp);
            // 
            // CustomerPhoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(936, 426);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CustomerPhoto";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Capture Customer Photo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CustomerPhoto_FormClosed);
            this.Load += new System.EventHandler(this.CustomerPhoto_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPhoto;
        private System.Windows.Forms.PictureBox pbCapture;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbCurrentImage;
    }
}