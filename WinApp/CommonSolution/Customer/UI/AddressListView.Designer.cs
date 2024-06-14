using System.Drawing;

namespace Semnox.Parafait.Customer
{
    partial class AddressListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddressListView));
            this.flpAddressListView = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelbtn = new System.Windows.Forms.Panel();
            this.lblAddressHeader = new System.Windows.Forms.Label();
            this.btnAddControl = new System.Windows.Forms.Button();
            this.addressListVerticalScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.chbShowActiveAddressEntries = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.groupBox1.SuspendLayout();
            this.panelbtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpAddressListView
            // 
            this.flpAddressListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpAddressListView.AutoScroll = true;
            this.flpAddressListView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flpAddressListView.Location = new System.Drawing.Point(3, 43);
            this.flpAddressListView.Name = "flpAddressListView";
            this.flpAddressListView.Size = new System.Drawing.Size(386, 165);
            this.flpAddressListView.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panelbtn);
            this.groupBox1.Controls.Add(this.addressListVerticalScrollBarView);
            this.groupBox1.Controls.Add(this.chbShowActiveAddressEntries);
            this.groupBox1.Controls.Add(this.flpAddressListView);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(408, 250);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Address";
            // 
            // panelbtn
            // 
            this.panelbtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelbtn.Controls.Add(this.lblAddressHeader);
            this.panelbtn.Controls.Add(this.btnAddControl);
            this.panelbtn.Location = new System.Drawing.Point(4, 216);
            this.panelbtn.Name = "panelbtn";
            this.panelbtn.Size = new System.Drawing.Size(360, 35);
            this.panelbtn.TabIndex = 20;
            // 
            // lblAddressHeader
            // 
            this.lblAddressHeader.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblAddressHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAddressHeader.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblAddressHeader.Location = new System.Drawing.Point(-1, 0);
            this.lblAddressHeader.Name = "lblAddressHeader";
            this.lblAddressHeader.Size = new System.Drawing.Size(316, 32);
            this.lblAddressHeader.TabIndex = 19;
            this.lblAddressHeader.Text = "ADDRESS";
            this.lblAddressHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddControl
            // 
            this.btnAddControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddControl.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.Plus_Btn_Green;
            this.btnAddControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAddControl.FlatAppearance.BorderSize = 0;
            this.btnAddControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddControl.Location = new System.Drawing.Point(315, -4);
            this.btnAddControl.Name = "btnAddControl";
            this.btnAddControl.Size = new System.Drawing.Size(40, 40);
            this.btnAddControl.TabIndex = 0;
            this.btnAddControl.UseVisualStyleBackColor = true;
            this.btnAddControl.Click += new System.EventHandler(this.btnAddControl_Click);
            // 
            // addressListVerticalScrollBarView
            // 
            this.addressListVerticalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressListVerticalScrollBarView.AutoHide = false;
            this.addressListVerticalScrollBarView.BackColor = System.Drawing.Color.Azure;
            this.addressListVerticalScrollBarView.DataGridView = null;
            this.addressListVerticalScrollBarView.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("addressListVerticalScrollBarView.DownButtonBackgroundImage")));
            this.addressListVerticalScrollBarView.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("addressListVerticalScrollBarView.DownButtonDisabledBackgroundImage")));
            this.addressListVerticalScrollBarView.Location = new System.Drawing.Point(366, 7);
            this.addressListVerticalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.addressListVerticalScrollBarView.Name = "addressListVerticalScrollBarView";
            this.addressListVerticalScrollBarView.ScrollableControl = this.flpAddressListView;
            this.addressListVerticalScrollBarView.ScrollViewer = null;
            this.addressListVerticalScrollBarView.Size = new System.Drawing.Size(44, 241);
            this.addressListVerticalScrollBarView.TabIndex = 1;
            this.addressListVerticalScrollBarView.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("addressListVerticalScrollBarView.UpButtonBackgroundImage")));
            this.addressListVerticalScrollBarView.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("addressListVerticalScrollBarView.UpButtonDisabledBackgroundImage")));
            // 
            // chbShowActiveAddressEntries
            // 
            this.chbShowActiveAddressEntries.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbShowActiveAddressEntries.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chbShowActiveAddressEntries.Checked = true;
            this.chbShowActiveAddressEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveAddressEntries.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveAddressEntries.FlatAppearance.BorderSize = 0;
            this.chbShowActiveAddressEntries.FlatAppearance.CheckedBackColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveAddressEntries.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveAddressEntries.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveAddressEntries.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShowActiveAddressEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbShowActiveAddressEntries.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chbShowActiveAddressEntries.ImageIndex = 0;
            this.chbShowActiveAddressEntries.Location = new System.Drawing.Point(7, 17);
            this.chbShowActiveAddressEntries.Margin = new System.Windows.Forms.Padding(2);
            this.chbShowActiveAddressEntries.Name = "chbShowActiveAddressEntries";
            this.chbShowActiveAddressEntries.Size = new System.Drawing.Size(183, 25);
            this.chbShowActiveAddressEntries.TabIndex = 15;
            this.chbShowActiveAddressEntries.Tag = "RightHanded";
            this.chbShowActiveAddressEntries.Text = "   Show Active Entries";
            this.chbShowActiveAddressEntries.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chbShowActiveAddressEntries.UseVisualStyleBackColor = true;
            this.chbShowActiveAddressEntries.CheckedChanged += new System.EventHandler(this.chkShowActive_CheckedChanged);
            // 
            // AddressListView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AddressListView";
            this.Size = new System.Drawing.Size(418, 250);
            this.groupBox1.ResumeLayout(false);
            this.panelbtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private Core.GenericUtilities.VerticalScrollBarView addressListVerticalScrollBarView;
        private System.Windows.Forms.FlowLayoutPanel flpAddressListView;
        private System.Windows.Forms.Button btnAddControl;
        private Core.GenericUtilities.CustomCheckBox chbShowActiveAddressEntries;
        private System.Windows.Forms.Label lblAddressHeader;
        private System.Windows.Forms.Panel panelbtn;
    }
}
