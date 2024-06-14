namespace Semnox.Parafait.Transaction
{
    partial class FrmProductModifier
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.flpModifierList = new System.Windows.Forms.FlowLayoutPanel();
            this.panelModifierProduct = new System.Windows.Forms.Panel();
            this.btnModifierProductCancel = new System.Windows.Forms.Button();
            this.lblModifierProduct = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.flpModifierSets = new System.Windows.Forms.FlowLayoutPanel();
            this.sampleModifierSet = new System.Windows.Forms.Button();
            this.flpParentModifierList = new System.Windows.Forms.FlowLayoutPanel();
            this.sampleParentModifier = new System.Windows.Forms.Button();
            this.flpProductList = new System.Windows.Forms.FlowLayoutPanel();
            this.tvProductList = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.flpModifierList.SuspendLayout();
            this.panelModifierProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.flpModifierSets.SuspendLayout();
            this.flpParentModifierList.SuspendLayout();
            this.flpProductList.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Azure;
            this.splitContainer1.Panel1.Controls.Add(this.panelStatus);
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.splitContainer1.Panel2.Controls.Add(this.flpProductList);
            this.splitContainer1.Size = new System.Drawing.Size(1310, 692);
            this.splitContainer1.SplitterDistance = 967;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.LightYellow;
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatus.Controls.Add(this.lblStatus);
            this.panelStatus.Location = new System.Drawing.Point(4, 647);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(963, 44);
            this.panelStatus.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(4, 6);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 18);
            this.lblStatus.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.splitContainer3.Location = new System.Drawing.Point(4, 215);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.flpModifierList);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.Color.Azure;
            this.splitContainer3.Panel2.Controls.Add(this.btnOK);
            this.splitContainer3.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer3.Size = new System.Drawing.Size(963, 426);
            this.splitContainer3.SplitterDistance = 353;
            this.splitContainer3.TabIndex = 7;
            // 
            // flpModifierList
            // 
            this.flpModifierList.AutoScroll = true;
            this.flpModifierList.BackColor = System.Drawing.Color.Azure;
            this.flpModifierList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.flpModifierList.Controls.Add(this.panelModifierProduct);
            this.flpModifierList.Location = new System.Drawing.Point(2, 2);
            this.flpModifierList.Name = "flpModifierList";
            this.flpModifierList.Size = new System.Drawing.Size(961, 348);
            this.flpModifierList.TabIndex = 0;
            // 
            // panelModifierProduct
            // 
            this.panelModifierProduct.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panelModifierProduct.BackColor = System.Drawing.Color.Azure;
            this.panelModifierProduct.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.ComboProduct;
            this.panelModifierProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelModifierProduct.Controls.Add(this.btnModifierProductCancel);
            this.panelModifierProduct.Controls.Add(this.lblModifierProduct);
            this.panelModifierProduct.Location = new System.Drawing.Point(3, 3);
            this.panelModifierProduct.Name = "panelModifierProduct";
            this.panelModifierProduct.Size = new System.Drawing.Size(100, 80);
            this.panelModifierProduct.TabIndex = 4;
            this.panelModifierProduct.Visible = false;
            // 
            // btnModifierProductCancel
            // 
            this.btnModifierProductCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnModifierProductCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnModifierProductCancel.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.cancel;
            this.btnModifierProductCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnModifierProductCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnModifierProductCancel.FlatAppearance.BorderSize = 0;
            this.btnModifierProductCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnModifierProductCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnModifierProductCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModifierProductCancel.ForeColor = System.Drawing.Color.Transparent;
            this.btnModifierProductCancel.Location = new System.Drawing.Point(76, 0);
            this.btnModifierProductCancel.Name = "btnModifierProductCancel";
            this.btnModifierProductCancel.Size = new System.Drawing.Size(24, 21);
            this.btnModifierProductCancel.TabIndex = 1;
            this.btnModifierProductCancel.UseVisualStyleBackColor = false;
            // 
            // lblModifierProduct
            // 
            this.lblModifierProduct.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblModifierProduct.BackColor = System.Drawing.Color.Transparent;
            this.lblModifierProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblModifierProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModifierProduct.Location = new System.Drawing.Point(2, 47);
            this.lblModifierProduct.Name = "lblModifierProduct";
            this.lblModifierProduct.Size = new System.Drawing.Size(94, 33);
            this.lblModifierProduct.TabIndex = 0;
            this.lblModifierProduct.Text = "Label";
            this.lblModifierProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.button_normal;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(6, 17);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(138, 42);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.button_normal;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(152, 17);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 42);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.splitContainer2.Panel1.Controls.Add(this.flpModifierSets);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.splitContainer2.Panel2.Controls.Add(this.flpParentModifierList);
            this.splitContainer2.Size = new System.Drawing.Size(964, 209);
            this.splitContainer2.SplitterDistance = 100;
            this.splitContainer2.TabIndex = 0;
            // 
            // flpModifierSets
            // 
            this.flpModifierSets.AutoScroll = true;
            this.flpModifierSets.BackColor = System.Drawing.Color.Azure;
            this.flpModifierSets.Controls.Add(this.sampleModifierSet);
            this.flpModifierSets.Location = new System.Drawing.Point(3, 3);
            this.flpModifierSets.Name = "flpModifierSets";
            this.flpModifierSets.Size = new System.Drawing.Size(961, 99);
            this.flpModifierSets.TabIndex = 0;
            this.flpModifierSets.WrapContents = false;
            // 
            // sampleModifierSet
            // 
            this.sampleModifierSet.BackColor = System.Drawing.Color.Transparent;
            this.sampleModifierSet.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.ComboProduct;
            this.sampleModifierSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleModifierSet.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sampleModifierSet.FlatAppearance.BorderSize = 0;
            this.sampleModifierSet.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleModifierSet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleModifierSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleModifierSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sampleModifierSet.ForeColor = System.Drawing.Color.White;
            this.sampleModifierSet.Location = new System.Drawing.Point(4, 4);
            this.sampleModifierSet.Margin = new System.Windows.Forms.Padding(4);
            this.sampleModifierSet.Name = "sampleModifierSet";
            this.sampleModifierSet.Size = new System.Drawing.Size(100, 70);
            this.sampleModifierSet.TabIndex = 4;
            this.sampleModifierSet.Text = "Sample";
            this.sampleModifierSet.UseVisualStyleBackColor = false;
            this.sampleModifierSet.Visible = false;
            // 
            // flpParentModifierList
            // 
            this.flpParentModifierList.AutoScroll = true;
            this.flpParentModifierList.BackColor = System.Drawing.Color.Azure;
            this.flpParentModifierList.Controls.Add(this.sampleParentModifier);
            this.flpParentModifierList.Location = new System.Drawing.Point(3, 1);
            this.flpParentModifierList.Name = "flpParentModifierList";
            this.flpParentModifierList.Size = new System.Drawing.Size(961, 103);
            this.flpParentModifierList.TabIndex = 4;
            this.flpParentModifierList.WrapContents = false;
            // 
            // sampleParentModifier
            // 
            this.sampleParentModifier.BackColor = System.Drawing.Color.Transparent;
            this.sampleParentModifier.BackgroundImage = global::Semnox.Parafait.Transaction.Properties.Resources.ComboProduct;
            this.sampleParentModifier.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleParentModifier.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sampleParentModifier.FlatAppearance.BorderSize = 0;
            this.sampleParentModifier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleParentModifier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleParentModifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleParentModifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sampleParentModifier.ForeColor = System.Drawing.Color.White;
            this.sampleParentModifier.Location = new System.Drawing.Point(4, 4);
            this.sampleParentModifier.Margin = new System.Windows.Forms.Padding(4);
            this.sampleParentModifier.Name = "sampleParentModifier";
            this.sampleParentModifier.Size = new System.Drawing.Size(100, 70);
            this.sampleParentModifier.TabIndex = 3;
            this.sampleParentModifier.Text = "Sample";
            this.sampleParentModifier.UseVisualStyleBackColor = false;
            this.sampleParentModifier.Visible = false;
            // 
            // flpProductList
            // 
            this.flpProductList.BackColor = System.Drawing.Color.Azure;
            this.flpProductList.Controls.Add(this.tvProductList);
            this.flpProductList.Location = new System.Drawing.Point(-1, 3);
            this.flpProductList.Name = "flpProductList";
            this.flpProductList.Size = new System.Drawing.Size(340, 689);
            this.flpProductList.TabIndex = 0;
            // 
            // tvProductList
            // 
            this.tvProductList.BackColor = System.Drawing.Color.Azure;
            this.tvProductList.Location = new System.Drawing.Point(3, 3);
            this.tvProductList.Name = "tvProductList";
            this.tvProductList.Size = new System.Drawing.Size(334, 683);
            this.tvProductList.TabIndex = 0;
            // 
            // FrmProductModifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1310, 692);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "FrmProductModifier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Modifiers";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.flpModifierList.ResumeLayout(false);
            this.panelModifierProduct.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.flpModifierSets.ResumeLayout(false);
            this.flpParentModifierList.ResumeLayout(false);
            this.flpProductList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flpProductList;
        private System.Windows.Forms.SplitContainer splitContainer2;
        internal System.Windows.Forms.FlowLayoutPanel flpModifierSets;
        private System.Windows.Forms.FlowLayoutPanel flpParentModifierList;
        private System.Windows.Forms.Button sampleModifierSet;
        private System.Windows.Forms.Button sampleParentModifier;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.FlowLayoutPanel flpModifierList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TreeView tvProductList;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelModifierProduct;
        private System.Windows.Forms.Label lblModifierProduct;
        private System.Windows.Forms.Button btnModifierProductCancel;
    }
}