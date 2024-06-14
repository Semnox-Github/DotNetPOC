namespace Parafait_Kiosk.Home
{
    partial class frmCardCountBasic
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
            this.btnOne = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Label();
            this.flpCardCount = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTwo = new System.Windows.Forms.Button();
            this.btnThree = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPoints = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtMessage = new System.Windows.Forms.Button();
            this.flpCardCount.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.TabIndex = 20010;
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(377, 1670);
            this.btnPrev.TabIndex = 1055;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnCart
            // 
            //this.btnCart.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            //this.btnCart.FlatAppearance.BorderSize = 0;
            //this.btnCart.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            //this.btnCart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnCart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnOne
            // 
            this.btnOne.BackColor = System.Drawing.Color.Transparent;
            this.btnOne.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.No_of_Card_Button;
            this.btnOne.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOne.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnOne.FlatAppearance.BorderSize = 0;
            this.btnOne.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOne.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOne.Font = new System.Drawing.Font("Gotham Rounded Medium", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOne.ForeColor = System.Drawing.Color.White;
            this.btnOne.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOne.Location = new System.Drawing.Point(0, 3);
            this.btnOne.Margin = new System.Windows.Forms.Padding(0, 3, 0, 30);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(882, 263);
            this.btnOne.TabIndex = 12;
            this.btnOne.Text = "All Value On 1 Card";
            this.btnOne.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOne.UseVisualStyleBackColor = false;
            this.btnOne.Visible = false;
            this.btnOne.Click += new System.EventHandler(this.btn_Click);
            this.btnOne.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnOne.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblHeading.Font = new System.Drawing.Font("Gotham Rounded Bold", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(3, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(1061, 75);
            this.lblHeading.TabIndex = 14;
            this.lblHeading.Text = "GIVES YOU";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpCardCount
            // 
            this.flpCardCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpCardCount.AutoScroll = true;
            this.flpCardCount.BackColor = System.Drawing.Color.Transparent;
            this.flpCardCount.Controls.Add(this.btnOne);
            this.flpCardCount.Controls.Add(this.btnTwo);
            this.flpCardCount.Controls.Add(this.btnThree);
            this.flpCardCount.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpCardCount.ForeColor = System.Drawing.Color.White;
            this.flpCardCount.Location = new System.Drawing.Point(99, 637);
            this.flpCardCount.Name = "flpCardCount";
            this.flpCardCount.Size = new System.Drawing.Size(882, 974);
            this.flpCardCount.TabIndex = 1052;
            // 
            // btnTwo
            // 
            this.btnTwo.BackColor = System.Drawing.Color.Transparent;
            this.btnTwo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.No_of_Card_Button;
            this.btnTwo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTwo.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnTwo.FlatAppearance.BorderSize = 0;
            this.btnTwo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTwo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTwo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTwo.Font = new System.Drawing.Font("Gotham Rounded Medium", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTwo.ForeColor = System.Drawing.Color.White;
            this.btnTwo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTwo.Location = new System.Drawing.Point(0, 299);
            this.btnTwo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 30);
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.Size = new System.Drawing.Size(882, 263);
            this.btnTwo.TabIndex = 13;
            this.btnTwo.Text = "Split Value Over 2 Cards";
            this.btnTwo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTwo.UseVisualStyleBackColor = false;
            this.btnTwo.Visible = false;
            this.btnTwo.Click += new System.EventHandler(this.btn_Click);
            this.btnTwo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnTwo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // btnThree
            // 
            this.btnThree.BackColor = System.Drawing.Color.Transparent;
            this.btnThree.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.No_of_Card_Button;
            this.btnThree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnThree.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnThree.FlatAppearance.BorderSize = 0;
            this.btnThree.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnThree.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnThree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThree.Font = new System.Drawing.Font("Gotham Rounded Medium", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThree.ForeColor = System.Drawing.Color.White;
            this.btnThree.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnThree.Location = new System.Drawing.Point(0, 595);
            this.btnThree.Margin = new System.Windows.Forms.Padding(0, 3, 0, 30);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(882, 263);
            this.btnThree.TabIndex = 14;
            this.btnThree.Text = "Split Value Over more than 2 Cards";
            this.btnThree.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThree.UseVisualStyleBackColor = false;
            this.btnThree.Visible = false;
            this.btnThree.Click += new System.EventHandler(this.btnThree_Click);
            this.btnThree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnThree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 10);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1056, 82);
            this.lblSiteName.TabIndex = 1054;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1061, 64);
            this.label1.TabIndex = 1056;
            this.label1.Text = "Please choose an option from below";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPoints
            // 
            this.lblPoints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPoints.BackColor = System.Drawing.Color.Transparent;
            this.lblPoints.Font = new System.Drawing.Font("Gotham Rounded Bold", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoints.ForeColor = System.Drawing.Color.White;
            this.lblPoints.Location = new System.Drawing.Point(3, 75);
            this.lblPoints.Name = "lblPoints";
            this.lblPoints.Size = new System.Drawing.Size(1061, 66);
            this.lblPoints.TabIndex = 1057;
            this.lblPoints.Text = "POINTS";
            this.lblPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.lblHeading);
            this.flowLayoutPanel1.Controls.Add(this.lblPoints);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(9, 259);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1061, 293);
            this.flowLayoutPanel1.TabIndex = 1058;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1870);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
            this.txtMessage.TabIndex = 20011;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // frmCardCountBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.flpCardCount);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCardCountBasic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCardCount";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCardCount_FormClosing);
            this.Load += new System.EventHandler(this.frmCardCount_Load);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.flpCardCount, 0);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.flowLayoutPanel1, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.flpCardCount.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOne;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.FlowLayoutPanel flpCardCount;
        private System.Windows.Forms.Button btnTwo;
        private System.Windows.Forms.Button btnThree;
        private System.Windows.Forms.Button lblSiteName;
        //private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPoints;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button txtMessage;
        //private System.Windows.Forms.Button btnHome;
    }
}
