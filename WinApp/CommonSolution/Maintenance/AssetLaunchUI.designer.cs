namespace Semnox.Parafait.Maintenance
{
    partial class AssetLaunchUI
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
            this.assetTypeLaunchBtn = new System.Windows.Forms.Button();
            this.assetGroupLaunchBtn = new System.Windows.Forms.Button();
            this.assetLaunchBtn = new System.Windows.Forms.Button();
            this.btnGroup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // assetTypeLaunchBtn
            // 
            this.assetTypeLaunchBtn.Location = new System.Drawing.Point(54, 45);
            this.assetTypeLaunchBtn.Name = "assetTypeLaunchBtn";
            this.assetTypeLaunchBtn.Size = new System.Drawing.Size(163, 23);
            this.assetTypeLaunchBtn.TabIndex = 0;
            this.assetTypeLaunchBtn.Text = "Launch Asset Type";
            this.assetTypeLaunchBtn.UseVisualStyleBackColor = true;
            this.assetTypeLaunchBtn.Click += new System.EventHandler(this.assetTypeLaunchBtn_Click);
            // 
            // assetGroupLaunchBtn
            // 
            this.assetGroupLaunchBtn.Location = new System.Drawing.Point(54, 83);
            this.assetGroupLaunchBtn.Name = "assetGroupLaunchBtn";
            this.assetGroupLaunchBtn.Size = new System.Drawing.Size(163, 23);
            this.assetGroupLaunchBtn.TabIndex = 1;
            this.assetGroupLaunchBtn.Text = "Launch Asset Group";
            this.assetGroupLaunchBtn.UseVisualStyleBackColor = true;
            this.assetGroupLaunchBtn.Click += new System.EventHandler(this.assetGroupLaunchBtn_Click);
            // 
            // assetLaunchBtn
            // 
            this.assetLaunchBtn.Location = new System.Drawing.Point(53, 121);
            this.assetLaunchBtn.Name = "assetLaunchBtn";
            this.assetLaunchBtn.Size = new System.Drawing.Size(163, 23);
            this.assetLaunchBtn.TabIndex = 2;
            this.assetLaunchBtn.Text = "Launch Asset";
            this.assetLaunchBtn.UseVisualStyleBackColor = true;
            this.assetLaunchBtn.Click += new System.EventHandler(this.assetLaunchBtn_Click);
            // 
            // btnGroup
            // 
            this.btnGroup.Location = new System.Drawing.Point(54, 165);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(163, 23);
            this.btnGroup.TabIndex = 3;
            this.btnGroup.Text = "Launch Map asset Group";
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // AssetLaunchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 227);
            this.Controls.Add(this.btnGroup);
            this.Controls.Add(this.assetLaunchBtn);
            this.Controls.Add(this.assetGroupLaunchBtn);
            this.Controls.Add(this.assetTypeLaunchBtn);
            this.Name = "AssetLaunchUI";
            this.Text = "AssetLaunchUI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button assetTypeLaunchBtn;
        private System.Windows.Forms.Button assetGroupLaunchBtn;
        private System.Windows.Forms.Button assetLaunchBtn;
        private System.Windows.Forms.Button btnGroup;
    }
}