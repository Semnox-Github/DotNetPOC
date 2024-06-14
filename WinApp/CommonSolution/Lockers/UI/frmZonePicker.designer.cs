namespace Semnox.Parafait.Device.Lockers
{
    partial class frmZonePicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmZonePicker));
            this.flpZoneList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSample = new System.Windows.Forms.Button();
            this.gpSelectZone = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.flpZoneList.SuspendLayout();
            this.gpSelectZone.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpZoneList
            // 
            this.flpZoneList.Controls.Add(this.btnSample);
            this.flpZoneList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpZoneList.Location = new System.Drawing.Point(3, 20);
            this.flpZoneList.Name = "flpZoneList";
            this.flpZoneList.Size = new System.Drawing.Size(923, 288);
            this.flpZoneList.TabIndex = 0;
            // 
            // btnSample
            // 
            this.btnSample.BackgroundImage = global::Semnox.Parafait.Device.Lockers.Properties.Resources.DiplayGroupButton;
            this.btnSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSample.FlatAppearance.BorderSize = 0;
            this.btnSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSample.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSample.Location = new System.Drawing.Point(10, 10);
            this.btnSample.Margin = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(167, 131);
            this.btnSample.TabIndex = 0;
            this.btnSample.Text = "Sample";
            this.btnSample.UseVisualStyleBackColor = true;
            this.btnSample.Visible = false;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // gpSelectZone
            // 
            this.gpSelectZone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpSelectZone.Controls.Add(this.flpZoneList);
            this.gpSelectZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpSelectZone.Location = new System.Drawing.Point(7, 7);
            this.gpSelectZone.Name = "gpSelectZone";
            this.gpSelectZone.Size = new System.Drawing.Size(929, 311);
            this.gpSelectZone.TabIndex = 1;
            this.gpSelectZone.TabStop = false;
            this.gpSelectZone.Text = "Select Zone";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(485, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(181, 54);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSelectAll.BackgroundImage")));
            this.btnSelectAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSelectAll.FlatAppearance.BorderSize = 0;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAll.ForeColor = System.Drawing.Color.White;
            this.btnSelectAll.Location = new System.Drawing.Point(281, 330);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(181, 54);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "All Zones";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // frmZonePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(942, 394);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gpSelectZone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmZonePicker";
            this.Text = "Select Zone";
            this.flpZoneList.ResumeLayout(false);
            this.gpSelectZone.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpZoneList;
        private System.Windows.Forms.GroupBox gpSelectZone;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Button btnSelectAll;
    }
}