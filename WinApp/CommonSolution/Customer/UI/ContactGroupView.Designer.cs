namespace Semnox.Parafait.Customer
{
    partial class ContactGroupView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactGroupView));
            this.pnlContactType = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblContactType = new System.Windows.Forms.Label();
            this.flpContactGroup = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlContactType.SuspendLayout();
            this.flpContactGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContactType
            // 
            this.pnlContactType.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlContactType.Controls.Add(this.btnAdd);
            this.pnlContactType.Controls.Add(this.lblContactType);
            this.pnlContactType.Location = new System.Drawing.Point(3, 3);
            this.pnlContactType.Name = "pnlContactType";
            this.pnlContactType.Size = new System.Drawing.Size(349, 35);
            this.pnlContactType.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(304, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(36, 34);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblContactType
            // 
            this.lblContactType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblContactType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblContactType.Location = new System.Drawing.Point(16, 3);
            this.lblContactType.Name = "lblContactType";
            this.lblContactType.Size = new System.Drawing.Size(280, 26);
            this.lblContactType.TabIndex = 1;
            this.lblContactType.Text = "Contact Type";
            this.lblContactType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpContactGroup
            // 
            this.flpContactGroup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flpContactGroup.AutoSize = true;
            this.flpContactGroup.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.flpContactGroup.Controls.Add(this.pnlContactType);
            this.flpContactGroup.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpContactGroup.Location = new System.Drawing.Point(0, 0);
            this.flpContactGroup.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.flpContactGroup.Name = "flpContactGroup";
            this.flpContactGroup.Size = new System.Drawing.Size(355, 41);
            this.flpContactGroup.TabIndex = 5;
            this.flpContactGroup.SizeChanged += new System.EventHandler(this.flpContactGroup_SizeChanged);
            this.flpContactGroup.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.flpContactGroup_ControlAdded);
            // 
            // ContactGroupView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.Controls.Add(this.flpContactGroup);
            this.Name = "ContactGroupView";
            this.Size = new System.Drawing.Size(354, 327);
            this.Load += new System.EventHandler(this.ContactGroupView_Load);
          //  this.Leave += new System.EventHandler(this.ContactGroupView_Leave);
            this.pnlContactType.ResumeLayout(false);
            this.flpContactGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlContactType;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblContactType;
        private System.Windows.Forms.FlowLayoutPanel flpContactGroup;
    }
}
