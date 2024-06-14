namespace Semnox.Parafait.Maintenance
{
    partial class MaintenanceLaunchUI
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
            this.btnMaintTask = new System.Windows.Forms.Button();
            this.btnMaintGrp = new System.Windows.Forms.Button();
            this.btnAdhoc = new System.Windows.Forms.Button();
            this.mainSummary = new System.Windows.Forms.Button();
            this.btnJobDetails = new System.Windows.Forms.Button();
            this.MaintRequest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMaintTask
            // 
            this.btnMaintTask.Location = new System.Drawing.Point(35, 31);
            this.btnMaintTask.Name = "btnMaintTask";
            this.btnMaintTask.Size = new System.Drawing.Size(156, 23);
            this.btnMaintTask.TabIndex = 0;
            this.btnMaintTask.Text = "Maintenance Task";
            this.btnMaintTask.UseVisualStyleBackColor = true;
            this.btnMaintTask.Click += new System.EventHandler(this.btnMaintTask_Click);
            // 
            // btnMaintGrp
            // 
            this.btnMaintGrp.Location = new System.Drawing.Point(35, 79);
            this.btnMaintGrp.Name = "btnMaintGrp";
            this.btnMaintGrp.Size = new System.Drawing.Size(156, 23);
            this.btnMaintGrp.TabIndex = 1;
            this.btnMaintGrp.Text = "Maintenace Group";
            this.btnMaintGrp.UseVisualStyleBackColor = true;
            this.btnMaintGrp.Click += new System.EventHandler(this.btnMaintGrp_Click);
            // 
            // btnAdhoc
            // 
            this.btnAdhoc.Location = new System.Drawing.Point(35, 119);
            this.btnAdhoc.Name = "btnAdhoc";
            this.btnAdhoc.Size = new System.Drawing.Size(156, 23);
            this.btnAdhoc.TabIndex = 2;
            this.btnAdhoc.Text = "Maintenance Adhoc";
            this.btnAdhoc.UseVisualStyleBackColor = true;
            this.btnAdhoc.Click += new System.EventHandler(this.btnAdhoc_Click);
            // 
            // mainSummary
            // 
            this.mainSummary.Location = new System.Drawing.Point(35, 226);
            this.mainSummary.Name = "mainSummary";
            this.mainSummary.Size = new System.Drawing.Size(156, 23);
            this.mainSummary.TabIndex = 3;
            this.mainSummary.Text = "Maintenance Summary";
            this.mainSummary.UseVisualStyleBackColor = true;
            this.mainSummary.Visible = false;
            // 
            // btnJobDetails
            // 
            this.btnJobDetails.Location = new System.Drawing.Point(35, 159);
            this.btnJobDetails.Name = "btnJobDetails";
            this.btnJobDetails.Size = new System.Drawing.Size(156, 23);
            this.btnJobDetails.TabIndex = 4;
            this.btnJobDetails.Text = "Maintenance Details";
            this.btnJobDetails.UseVisualStyleBackColor = true;
            this.btnJobDetails.Click += new System.EventHandler(this.btnJobDetails_Click);
            // 
            // MaintRequest
            // 
            this.MaintRequest.Location = new System.Drawing.Point(35, 194);
            this.MaintRequest.Name = "MaintRequest";
            this.MaintRequest.Size = new System.Drawing.Size(156, 23);
            this.MaintRequest.TabIndex = 5;
            this.MaintRequest.Text = "Maintenance Request";
            this.MaintRequest.UseVisualStyleBackColor = true;
            this.MaintRequest.Click += new System.EventHandler(this.MaintRequest_Click);
            // 
            // MaintenanceLaunchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 261);
            this.Controls.Add(this.MaintRequest);
            this.Controls.Add(this.btnJobDetails);
            this.Controls.Add(this.mainSummary);
            this.Controls.Add(this.btnAdhoc);
            this.Controls.Add(this.btnMaintGrp);
            this.Controls.Add(this.btnMaintTask);
            this.Name = "MaintenanceLaunchUI";
            this.Text = "MaintenanceLaunchUI";            
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMaintTask;
        private System.Windows.Forms.Button btnMaintGrp;
        private System.Windows.Forms.Button btnAdhoc;
        private System.Windows.Forms.Button mainSummary;
        private System.Windows.Forms.Button btnJobDetails;
        private System.Windows.Forms.Button MaintRequest;
    }
}