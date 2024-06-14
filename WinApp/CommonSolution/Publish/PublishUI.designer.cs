namespace Semnox.Parafait.Publish
{
    partial class PublishUI
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
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            this.panelTree = new System.Windows.Forms.Panel();
            this.tvOrganization = new System.Windows.Forms.TreeView();
            this.btnPublish = new System.Windows.Forms.Button();
            this.lblPublishObject = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.fpnlEntityPublish = new System.Windows.Forms.FlowLayoutPanel();
            this.panelTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTree
            // 
            this.panelTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTree.Controls.Add(this.tvOrganization);
            this.panelTree.Location = new System.Drawing.Point(259, 3);
            this.panelTree.Name = "panelTree";
            this.panelTree.Size = new System.Drawing.Size(479, 474);
            this.panelTree.TabIndex = 33;
            // 
            // tvOrganization
            // 
            this.tvOrganization.CheckBoxes = true;
            this.tvOrganization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrganization.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvOrganization.Location = new System.Drawing.Point(0, 0);
            this.tvOrganization.Name = "tvOrganization";
            treeNode4.Name = "Node1";
            treeNode4.Text = "Node1";
            treeNode5.Name = "Node2";
            treeNode5.Text = "Node2";
            treeNode6.Name = "Node0";
            treeNode6.Text = "Node0";
            this.tvOrganization.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.tvOrganization.Size = new System.Drawing.Size(479, 474);
            this.tvOrganization.TabIndex = 0;
            this.tvOrganization.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvOrganization_AfterCheck);
            // 
            // btnPublish
            // 
            this.btnPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublish.BackgroundImage = global::Semnox.Parafait.Publish.Properties.Resources.BtnBackground;
            this.btnPublish.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPublish.FlatAppearance.BorderSize = 0;
            this.btnPublish.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPublish.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPublish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPublish.Location = new System.Drawing.Point(12, 405);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(103, 62);
            this.btnPublish.TabIndex = 34;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // lblPublishObject
            // 
            this.lblPublishObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPublishObject.Location = new System.Drawing.Point(12, 9);
            this.lblPublishObject.Name = "lblPublishObject";
            this.lblPublishObject.Size = new System.Drawing.Size(241, 73);
            this.lblPublishObject.TabIndex = 35;
            this.lblPublishObject.Text = "Publishing Product:\r\n100 Rs Recharge";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackgroundImage = global::Semnox.Parafait.Publish.Properties.Resources.BtnBackground;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(141, 405);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 62);
            this.btnClose.TabIndex = 36;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // fpnlEntityPublish
            // 
            this.fpnlEntityPublish.AutoScroll = true;
            this.fpnlEntityPublish.Location = new System.Drawing.Point(12, 109);
            this.fpnlEntityPublish.Name = "fpnlEntityPublish";
            this.fpnlEntityPublish.Size = new System.Drawing.Size(232, 31);
            this.fpnlEntityPublish.TabIndex = 1;
            // 
            // PublishUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(741, 479);
            this.Controls.Add(this.fpnlEntityPublish);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblPublishObject);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.panelTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PublishUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HQPublish";
            this.Load += new System.EventHandler(this.HQPublish_Load);
            this.panelTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTree;
        private System.Windows.Forms.TreeView tvOrganization;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Label lblPublishObject;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel fpnlEntityPublish;
    }
}

