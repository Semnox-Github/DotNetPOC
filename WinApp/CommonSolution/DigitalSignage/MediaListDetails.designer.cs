namespace Semnox.Parafait.DigitalSignage
{
    partial class MediaListDetails
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaListDetails));
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.bindLookupView = new System.Windows.Forms.BindingSource(this.components);
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblActive = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.rchDescription = new System.Windows.Forms.RichTextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.lblSizeXInPixels = new System.Windows.Forms.Label();
            this.txtSizeXInPixels = new System.Windows.Forms.TextBox();
            this.lblSizeYInPixels = new System.Windows.Forms.Label();
            this.txtSizeYInPixels = new System.Windows.Forms.TextBox();
            this.lnkPrevious = new System.Windows.Forms.LinkLabel();
            this.lnkNext = new System.Windows.Forms.LinkLabel();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.lblLink = new System.Windows.Forms.Label();
            this.lblPreviewVideo = new System.Windows.Forms.LinkLabel();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindLookupView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.grpPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(31, 28);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(88, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Media Name: *";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(130, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(129, 21);
            this.txtName.TabIndex = 1;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(31, 56);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(36, 15);
            this.lblType.TabIndex = 20;
            this.lblType.Text = "Type:";
            // 
            // cmbType
            // 
            this.cmbType.DataSource = this.bindLookupView;
            this.cmbType.DisplayMember = "LookupValue";
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(130, 53);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(129, 23);
            this.cmbType.TabIndex = 2;
            this.cmbType.ValueMember = "LookupValueId";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // bindLookupView
            // 
            this.bindLookupView.DataMember = "LookupView";
            this.bindLookupView.Filter = "lookupname =\'MEDIA_TYPE\' and LookupValue <> \'ALL\'";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.Location = new System.Drawing.Point(31, 89);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(70, 15);
            this.lblFileName.TabIndex = 40;
            this.lblFileName.Text = "File Name:*";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(130, 91);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(125, 21);
            this.txtFileName.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(282, 89);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(63, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActive.Location = new System.Drawing.Point(31, 122);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(45, 15);
            this.lblActive.TabIndex = 70;
            this.lblActive.Text = "Active:";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(130, 127);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(15, 14);
            this.chkActive.TabIndex = 5;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(31, 158);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(75, 15);
            this.lblDescription.TabIndex = 90;
            this.lblDescription.Text = "Description:";
            // 
            // rchDescription
            // 
            this.rchDescription.Location = new System.Drawing.Point(130, 155);
            this.rchDescription.Name = "rchDescription";
            this.rchDescription.Size = new System.Drawing.Size(129, 96);
            this.rchDescription.TabIndex = 5;
            this.rchDescription.Text = "";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(34, 402);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Save";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(151, 402);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.AutoSize = true;
            this.lblStatusMessage.Location = new System.Drawing.Point(31, 368);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(58, 15);
            this.lblStatusMessage.TabIndex = 16;
            this.lblStatusMessage.Text = "Message";
            this.lblStatusMessage.Visible = false;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblID.Location = new System.Drawing.Point(401, 29);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(21, 15);
            this.lblID.TabIndex = 17;
            this.lblID.Text = "ID:";
            // 
            // txtID
            // 
            this.txtID.Enabled = false;
            this.txtID.Location = new System.Drawing.Point(445, 25);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(100, 21);
            this.txtID.TabIndex = 0;
            // 
            // lblSizeXInPixels
            // 
            this.lblSizeXInPixels.AutoSize = true;
            this.lblSizeXInPixels.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSizeXInPixels.Location = new System.Drawing.Point(31, 270);
            this.lblSizeXInPixels.Name = "lblSizeXInPixels";
            this.lblSizeXInPixels.Size = new System.Drawing.Size(99, 15);
            this.lblSizeXInPixels.TabIndex = 91;
            this.lblSizeXInPixels.Text = "Size X in Pixels: ";
            this.lblSizeXInPixels.Visible = false;
            // 
            // txtSizeXInPixels
            // 
            this.txtSizeXInPixels.Location = new System.Drawing.Point(130, 267);
            this.txtSizeXInPixels.Name = "txtSizeXInPixels";
            this.txtSizeXInPixels.Size = new System.Drawing.Size(129, 21);
            this.txtSizeXInPixels.TabIndex = 6;
            this.txtSizeXInPixels.Visible = false;
            // 
            // lblSizeYInPixels
            // 
            this.lblSizeYInPixels.AutoSize = true;
            this.lblSizeYInPixels.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSizeYInPixels.Location = new System.Drawing.Point(31, 313);
            this.lblSizeYInPixels.Name = "lblSizeYInPixels";
            this.lblSizeYInPixels.Size = new System.Drawing.Size(95, 15);
            this.lblSizeYInPixels.TabIndex = 93;
            this.lblSizeYInPixels.Text = "Size Y in Pixels:";
            this.lblSizeYInPixels.Visible = false;
            // 
            // txtSizeYInPixels
            // 
            this.txtSizeYInPixels.Location = new System.Drawing.Point(130, 307);
            this.txtSizeYInPixels.Name = "txtSizeYInPixels";
            this.txtSizeYInPixels.Size = new System.Drawing.Size(129, 21);
            this.txtSizeYInPixels.TabIndex = 7;
            this.txtSizeYInPixels.Visible = false;
            // 
            // lnkPrevious
            // 
            this.lnkPrevious.AutoSize = true;
            this.lnkPrevious.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPrevious.Location = new System.Drawing.Point(260, 406);
            this.lnkPrevious.Name = "lnkPrevious";
            this.lnkPrevious.Size = new System.Drawing.Size(57, 15);
            this.lnkPrevious.TabIndex = 94;
            this.lnkPrevious.TabStop = true;
            this.lnkPrevious.Text = "Previous";
            this.lnkPrevious.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPrevious_LinkClicked);
            // 
            // lnkNext
            // 
            this.lnkNext.AutoSize = true;
            this.lnkNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkNext.Location = new System.Drawing.Point(333, 406);
            this.lnkNext.Name = "lnkNext";
            this.lnkNext.Size = new System.Drawing.Size(33, 15);
            this.lnkNext.TabIndex = 95;
            this.lnkNext.TabStop = true;
            this.lnkNext.Text = "Next";
            this.lnkNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNext_LinkClicked);
            // 
            // txtLink
            // 
            this.txtLink.Location = new System.Drawing.Point(372, 90);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(265, 21);
            this.txtLink.TabIndex = 96;
            this.txtLink.Visible = false;
            this.txtLink.TextChanged += new System.EventHandler(this.txtLink_TextChanged);
            // 
            // lblLink
            // 
            this.lblLink.AutoSize = true;
            this.lblLink.Location = new System.Drawing.Point(271, 93);
            this.lblLink.Name = "lblLink";
            this.lblLink.Size = new System.Drawing.Size(95, 15);
            this.lblLink.TabIndex = 97;
            this.lblLink.Text = "Enter URL here:";
            this.lblLink.Visible = false;
            // 
            // lblPreviewVideo
            // 
            this.lblPreviewVideo.AutoSize = true;
            this.lblPreviewVideo.Location = new System.Drawing.Point(404, 338);
            this.lblPreviewVideo.Name = "lblPreviewVideo";
            this.lblPreviewVideo.Size = new System.Drawing.Size(112, 15);
            this.lblPreviewVideo.TabIndex = 98;
            this.lblPreviewVideo.TabStop = true;
            this.lblPreviewVideo.Text = "Preview URL Video";
            this.lblPreviewVideo.Visible = false;
            this.lblPreviewVideo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPreviewVideo_LinkClicked);
            // 
            // picPreview
            // 
            this.picPreview.Location = new System.Drawing.Point(15, 20);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(179, 145);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            this.picPreview.MouseHover += new System.EventHandler(this.picPreview_MouseHover);
            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this.picPreview);
            this.grpPreview.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPreview.Location = new System.Drawing.Point(404, 155);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(200, 173);
            this.grpPreview.TabIndex = 8;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview";
            this.grpPreview.Visible = false;
            // 
            // MediaListDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 439);
            this.Controls.Add(this.lblPreviewVideo);
            this.Controls.Add(this.lblLink);
            this.Controls.Add(this.txtLink);
            this.Controls.Add(this.lnkNext);
            this.Controls.Add(this.lnkPrevious);
            this.Controls.Add(this.txtSizeYInPixels);
            this.Controls.Add(this.lblSizeYInPixels);
            this.Controls.Add(this.txtSizeXInPixels);
            this.Controls.Add(this.lblSizeXInPixels);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblStatusMessage);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.rchDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblActive);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MediaListDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Media Details";
            this.Load += new System.EventHandler(this.MediaListDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindLookupView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.grpPreview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.RichTextBox rchDescription;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource bindLookupView;
        //private parafaitDataSetSignage parafaitDataSetSignage;
        //private parafaitDataSetSignageTableAdapters.LookupViewTableAdapter lookupViewTableAdapter;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label lblSizeXInPixels;
        private System.Windows.Forms.TextBox txtSizeXInPixels;
        private System.Windows.Forms.Label lblSizeYInPixels;
        private System.Windows.Forms.TextBox txtSizeYInPixels;
        private System.Windows.Forms.LinkLabel lnkPrevious;
        private System.Windows.Forms.LinkLabel lnkNext;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.Label lblLink;
        private System.Windows.Forms.LinkLabel lblPreviewVideo;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.GroupBox grpPreview;
    }
}