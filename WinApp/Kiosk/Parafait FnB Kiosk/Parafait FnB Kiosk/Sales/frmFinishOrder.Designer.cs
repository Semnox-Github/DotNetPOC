namespace Parafait_FnB_Kiosk
{
    partial class frmFinishOrder
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
            this.panelBG = new System.Windows.Forms.Panel();
            this.panelElements = new System.Windows.Forms.Panel();
            this.lblScreenTitle = new System.Windows.Forms.Label();
            this.lblOrderNumber = new System.Windows.Forms.Label();
            this.btnPicture = new System.Windows.Forms.Button();
            this.panelConfirm = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelBG.SuspendLayout();
            this.panelElements.SuspendLayout();
            this.panelConfirm.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackColor = System.Drawing.Color.Transparent;
            this.panelBG.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Popup_955x1845;
            this.panelBG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelBG.Controls.Add(this.panelElements);
            this.panelBG.Controls.Add(this.panelConfirm);
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(955, 1845);
            this.panelBG.TabIndex = 5;
            // 
            // panelElements
            // 
            this.panelElements.Controls.Add(this.lblScreenTitle);
            this.panelElements.Controls.Add(this.lblOrderNumber);
            this.panelElements.Controls.Add(this.btnPicture);
            this.panelElements.Location = new System.Drawing.Point(0, 133);
            this.panelElements.Margin = new System.Windows.Forms.Padding(0);
            this.panelElements.Name = "panelElements";
            this.panelElements.Size = new System.Drawing.Size(955, 1535);
            this.panelElements.TabIndex = 2;
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(3, 0);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Size = new System.Drawing.Size(952, 163);
            this.lblScreenTitle.TabIndex = 5;
            this.lblScreenTitle.Text = "Please proceed to counter for cups/plates/silverware and alcohol identification";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblOrderNumber
            // 
            this.lblOrderNumber.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderNumber.ForeColor = System.Drawing.Color.White;
            this.lblOrderNumber.Location = new System.Drawing.Point(3, 263);
            this.lblOrderNumber.Margin = new System.Windows.Forms.Padding(3, 100, 3, 0);
            this.lblOrderNumber.Name = "lblOrderNumber";
            this.lblOrderNumber.Size = new System.Drawing.Size(949, 129);
            this.lblOrderNumber.TabIndex = 7;
            this.lblOrderNumber.Text = "Look for this sign";
            this.lblOrderNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPicture
            // 
            this.btnPicture.FlatAppearance.BorderSize = 0;
            this.btnPicture.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPicture.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPicture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPicture.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Finished_Screen_Image_1216;
            this.btnPicture.Location = new System.Drawing.Point(3, 395);
            this.btnPicture.Name = "btnPicture";
            this.btnPicture.Size = new System.Drawing.Size(530, 1080);
            this.btnPicture.TabIndex = 6;
            this.btnPicture.TabStop = false;
            // 
            // panelConfirm
            // 
            this.panelConfirm.Controls.Add(this.btnClose);
            this.panelConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConfirm.Location = new System.Drawing.Point(0, 1680);
            this.panelConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelConfirm.Name = "panelConfirm";
            this.panelConfirm.Size = new System.Drawing.Size(955, 165);
            this.panelConfirm.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Green_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnClose.Location = new System.Drawing.Point(304, 40);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(346, 68);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmFinishOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Turquoise;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(955, 1845);
            this.Controls.Add(this.panelBG);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "frmFinishOrder";
            this.Text = "BaseFormProductSale";
            this.TransparencyKey = System.Drawing.Color.Turquoise;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.frmTentSelection_Load);
            this.panelBG.ResumeLayout(false);
            this.panelElements.ResumeLayout(false);
            this.panelConfirm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelBG;
        public System.Windows.Forms.Panel panelElements;
        public System.Windows.Forms.Panel panelConfirm;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblScreenTitle;
        private System.Windows.Forms.Button btnPicture;
        private System.Windows.Forms.Label lblOrderNumber;
    }
}