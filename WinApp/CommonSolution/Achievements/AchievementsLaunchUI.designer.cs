namespace Semnox.Parafait.Achievements
{
    partial class AchievementsLaunchUI
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
            this.btnAchievementClass = new System.Windows.Forms.Button();
            this.btnAchievementProjects = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAchievementClass
            // 
            this.btnAchievementClass.Location = new System.Drawing.Point(27, 62);
            this.btnAchievementClass.Name = "btnAchievementClass";
            this.btnAchievementClass.Size = new System.Drawing.Size(234, 23);
            this.btnAchievementClass.TabIndex = 7;
            this.btnAchievementClass.Text = "Achievement Class";
            this.btnAchievementClass.UseVisualStyleBackColor = true;
            this.btnAchievementClass.Click += new System.EventHandler(this.btnAchievementClass_Click);
            // 
            // btnAchievementProjects
            // 
            this.btnAchievementProjects.Location = new System.Drawing.Point(27, 21);
            this.btnAchievementProjects.Name = "btnAchievementProjects";
            this.btnAchievementProjects.Size = new System.Drawing.Size(234, 23);
            this.btnAchievementProjects.TabIndex = 6;
            this.btnAchievementProjects.Text = "Achievement Projects";
            this.btnAchievementProjects.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAchievementProjects.UseVisualStyleBackColor = true;
            this.btnAchievementProjects.Click += new System.EventHandler(this.btnAchievementProjects_Click);
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(25, 119);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(234, 23);
            this.btn1.TabIndex = 8;
            this.btn1.Text = "AchievementDetailsPosUI";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // AchievementsLaunchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.btnAchievementClass);
            this.Controls.Add(this.btnAchievementProjects);
            this.Name = "AchievementsLaunchUI";
            this.Text = "AchievementsLaunchUI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAchievementClass;
        private System.Windows.Forms.Button btnAchievementProjects;
        private System.Windows.Forms.Button btn1;
    }
}