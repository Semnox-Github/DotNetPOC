namespace Semnox.Parafait.Schedule
{
    partial class ScheduleLaunchUI
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
            this.btnSchedule = new System.Windows.Forms.Button();
            this.btnScheduleExclusions = new System.Windows.Forms.Button();
            this.btnScheduleUI = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSchedule
            // 
            this.btnSchedule.Location = new System.Drawing.Point(65, 54);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(134, 23);
            this.btnSchedule.TabIndex = 0;
            this.btnSchedule.Text = "Schedule";
            this.btnSchedule.UseVisualStyleBackColor = true;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // btnScheduleExclusions
            // 
            this.btnScheduleExclusions.Location = new System.Drawing.Point(65, 94);
            this.btnScheduleExclusions.Name = "btnScheduleExclusions";
            this.btnScheduleExclusions.Size = new System.Drawing.Size(134, 23);
            this.btnScheduleExclusions.TabIndex = 1;
            this.btnScheduleExclusions.Text = "schedule Exclusion";
            this.btnScheduleExclusions.UseVisualStyleBackColor = true;
            this.btnScheduleExclusions.Click += new System.EventHandler(this.btnScheduleExclusions_Click);
            // 
            // btnScheduleUI
            // 
            this.btnScheduleUI.Location = new System.Drawing.Point(65, 136);
            this.btnScheduleUI.Name = "btnScheduleUI";
            this.btnScheduleUI.Size = new System.Drawing.Size(134, 23);
            this.btnScheduleUI.TabIndex = 2;
            this.btnScheduleUI.Text = "ScheduleUI";
            this.btnScheduleUI.UseVisualStyleBackColor = true;
            this.btnScheduleUI.Click += new System.EventHandler(this.btnScheduleUI_Click);
            // 
            // ScheduleLaunchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 261);
            this.Controls.Add(this.btnScheduleUI);
            this.Controls.Add(this.btnScheduleExclusions);
            this.Controls.Add(this.btnSchedule);
            this.Name = "ScheduleLaunchUI";
            this.Text = "ScheduleLaunchUI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnScheduleExclusions;
        private System.Windows.Forms.Button btnScheduleUI;
    }
}