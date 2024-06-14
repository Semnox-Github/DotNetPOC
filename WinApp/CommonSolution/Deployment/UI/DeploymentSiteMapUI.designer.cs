namespace Semnox.Parafait.Deployment
{
    partial class DeploymentSiteMapUI
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.btnClose = new System.Windows.Forms.Button();
            this.lblPublishObject = new System.Windows.Forms.Label();
            this.btnPublish = new System.Windows.Forms.Button();
            this.panelTree = new System.Windows.Forms.Panel();
            this.tvOrganization = new System.Windows.Forms.TreeView();
            this.panelTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(152, 254);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 40;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblPublishObject
            // 
            this.lblPublishObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPublishObject.Location = new System.Drawing.Point(12, 9);
            this.lblPublishObject.Name = "lblPublishObject";
            this.lblPublishObject.Size = new System.Drawing.Size(235, 227);
            this.lblPublishObject.TabIndex = 39;
            this.lblPublishObject.Text = "Deployment Plan:\r";
            this.lblPublishObject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnPublish
            // 
            this.btnPublish.Location = new System.Drawing.Point(23, 254);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 23);
            this.btnPublish.TabIndex = 38;
            this.btnPublish.Text = "Save";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // panelTree
            // 
            this.panelTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTree.Controls.Add(this.tvOrganization);
            this.panelTree.Location = new System.Drawing.Point(253, 3);
            this.panelTree.Name = "panelTree";
            this.panelTree.Size = new System.Drawing.Size(479, 517);
            this.panelTree.TabIndex = 37;
            // 
            // tvOrganization
            // 
            this.tvOrganization.CheckBoxes = true;
            this.tvOrganization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrganization.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvOrganization.Location = new System.Drawing.Point(0, 0);
            this.tvOrganization.Name = "tvOrganization";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Node2";
            treeNode3.Name = "Node0";
            treeNode3.Text = "Node0";
            this.tvOrganization.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.tvOrganization.Size = new System.Drawing.Size(479, 517);
            this.tvOrganization.TabIndex = 0;
            this.tvOrganization.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvOrganization_AfterSelect);
            // 
            // DeploymentSiteMapUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 522);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblPublishObject);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.panelTree);
            this.Name = "DeploymentSiteMapUI";
            this.Text = "Sites";
            this.Load += new System.EventHandler(this.DeploymentSiteMapUI_Load);
            this.panelTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblPublishObject;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Panel panelTree;
        private System.Windows.Forms.TreeView tvOrganization;

    }
}