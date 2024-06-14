 namespace Semnox.Parafait.Deployment
{
    partial class AutomatedPatchAssetLaunchUI
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
            this.btnApplicationType = new System.Windows.Forms.Button();
            this.btnApplicationAsset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnApplicationType
            // 
            this.btnApplicationType.Location = new System.Drawing.Point(44, 41);
            this.btnApplicationType.Name = "btnApplicationType";
            this.btnApplicationType.Size = new System.Drawing.Size(197, 33);
            this.btnApplicationType.TabIndex = 1;
            this.btnApplicationType.Text = "Application Type";
            this.btnApplicationType.UseVisualStyleBackColor = true;
            this.btnApplicationType.Click += new System.EventHandler(this.btnApplicationType_Click);
            // 
            // btnApplicationAsset
            // 
            this.btnApplicationAsset.Location = new System.Drawing.Point(44, 97);
            this.btnApplicationAsset.Name = "btnApplicationAsset";
            this.btnApplicationAsset.Size = new System.Drawing.Size(197, 33);
            this.btnApplicationAsset.TabIndex = 2;
            this.btnApplicationAsset.Text = "Application Asset";
            this.btnApplicationAsset.UseVisualStyleBackColor = true;
            this.btnApplicationAsset.Click += new System.EventHandler(this.btnApplicationAsset_Click);
            // 
            // AutomatedPatchAssetLaunchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 160);
            this.Controls.Add(this.btnApplicationAsset);
            this.Controls.Add(this.btnApplicationType);
            this.Name = "AutomatedPatchAssetLaunchUI";
            this.Text = "AutomatedPatchAssetLaunchUI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnApplicationType;
        private System.Windows.Forms.Button btnApplicationAsset;
    }
}

