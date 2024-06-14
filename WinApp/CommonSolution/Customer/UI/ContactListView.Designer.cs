namespace Semnox.Parafait.Customer
{
    partial class ContactListView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactListView));
            this.grbContactListView = new System.Windows.Forms.GroupBox();
            this.contactListVerticalScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.flpContactListView = new System.Windows.Forms.FlowLayoutPanel();
            this.chbShowActiveContactEntries = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.grbContactListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbContactListView
            // 
            this.grbContactListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbContactListView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grbContactListView.BackColor = System.Drawing.Color.Transparent;
            this.grbContactListView.Controls.Add(this.contactListVerticalScrollBarView);
            this.grbContactListView.Controls.Add(this.chbShowActiveContactEntries);
            this.grbContactListView.Controls.Add(this.flpContactListView);
            this.grbContactListView.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbContactListView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grbContactListView.Location = new System.Drawing.Point(6, 0);
            this.grbContactListView.Name = "grbContactListView";
            this.grbContactListView.Padding = new System.Windows.Forms.Padding(0);
            this.grbContactListView.Size = new System.Drawing.Size(408, 250);
            this.grbContactListView.TabIndex = 1;
            this.grbContactListView.TabStop = false;
            this.grbContactListView.Text = "Contact";
            // 
            // contactListVerticalScrollBarView
            // 
            this.contactListVerticalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contactListVerticalScrollBarView.AutoHide = false;
            this.contactListVerticalScrollBarView.BackColor = System.Drawing.Color.Azure;
            this.contactListVerticalScrollBarView.DataGridView = null;
            this.contactListVerticalScrollBarView.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("contactListVerticalScrollBarView.DownButtonBackgroundImage")));
            this.contactListVerticalScrollBarView.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("contactListVerticalScrollBarView.DownButtonDisabledBackgroundImage")));
            this.contactListVerticalScrollBarView.Location = new System.Drawing.Point(366, 7);
            this.contactListVerticalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.contactListVerticalScrollBarView.Name = "contactListVerticalScrollBarView";
            this.contactListVerticalScrollBarView.ScrollableControl = this.flpContactListView;
            this.contactListVerticalScrollBarView.ScrollViewer = null;
            this.contactListVerticalScrollBarView.Size = new System.Drawing.Size(44, 240);
            this.contactListVerticalScrollBarView.TabIndex = 19;
            this.contactListVerticalScrollBarView.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("contactListVerticalScrollBarView.UpButtonBackgroundImage")));
            this.contactListVerticalScrollBarView.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("contactListVerticalScrollBarView.UpButtonDisabledBackgroundImage")));
            // 
            // flpContactListView
            // 
            this.flpContactListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpContactListView.AutoScroll = true;
            this.flpContactListView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flpContactListView.Location = new System.Drawing.Point(2, 51);
            this.flpContactListView.Name = "flpContactListView";
            this.flpContactListView.Padding = new System.Windows.Forms.Padding(3);
            this.flpContactListView.Size = new System.Drawing.Size(385, 193);
            this.flpContactListView.TabIndex = 18;
            // 
            // chbShowActiveContactEntries
            // 
            this.chbShowActiveContactEntries.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbShowActiveContactEntries.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chbShowActiveContactEntries.Checked = true;
            this.chbShowActiveContactEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveContactEntries.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveContactEntries.FlatAppearance.BorderSize = 0;
            this.chbShowActiveContactEntries.FlatAppearance.CheckedBackColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveContactEntries.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveContactEntries.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveContactEntries.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShowActiveContactEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbShowActiveContactEntries.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chbShowActiveContactEntries.ImageIndex = 0;
            this.chbShowActiveContactEntries.Location = new System.Drawing.Point(3, 21);
            this.chbShowActiveContactEntries.Margin = new System.Windows.Forms.Padding(2);
            this.chbShowActiveContactEntries.Name = "chbShowActiveContactEntries";
            this.chbShowActiveContactEntries.Size = new System.Drawing.Size(183, 25);
            this.chbShowActiveContactEntries.TabIndex = 15;
            this.chbShowActiveContactEntries.Tag = "RightHanded";
            this.chbShowActiveContactEntries.Text = "Show Active Entries";
            this.chbShowActiveContactEntries.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chbShowActiveContactEntries.UseVisualStyleBackColor = true;
            this.chbShowActiveContactEntries.CheckedChanged += new System.EventHandler(this.chkShowActive_CheckedChanged);
            // 
            // ContactListView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.Controls.Add(this.grbContactListView);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ContactListView";
            this.Size = new System.Drawing.Size(416, 250);
            this.grbContactListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbContactListView;
        private Semnox.Core.GenericUtilities.CustomCheckBox chbShowActiveContactEntries;
        private System.Windows.Forms.FlowLayoutPanel flpContactListView;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView contactListVerticalScrollBarView;
    }
}
