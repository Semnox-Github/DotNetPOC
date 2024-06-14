namespace Semnox.Parafait.Communication
{
    partial class SendEmailUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendEmailUI));
            this.txtToEmails = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCCEmails = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBody = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtreplyToEmail = new System.Windows.Forms.TextBox();
            this.lnkAttachFile = new System.Windows.Forms.LinkLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.flpAttachments = new System.Windows.Forms.FlowLayoutPanel();
            this.AttachmentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAttach = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.flpAttachments.SuspendLayout();
            this.AttachmentContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtToEmails
            // 
            this.txtToEmails.Location = new System.Drawing.Point(100, 8);
            this.txtToEmails.Name = "txtToEmails";
            this.txtToEmails.Size = new System.Drawing.Size(257, 20);
            this.txtToEmails.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "To:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "CC:";
            // 
            // txtCCEmails
            // 
            this.txtCCEmails.Location = new System.Drawing.Point(100, 34);
            this.txtCCEmails.Name = "txtCCEmails";
            this.txtCCEmails.Size = new System.Drawing.Size(257, 20);
            this.txtCCEmails.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Subject:";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(100, 87);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(534, 20);
            this.txtSubject.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Body:";
            // 
            // txtBody
            // 
            this.txtBody.ForeColor = System.Drawing.Color.Black;
            this.txtBody.Location = new System.Drawing.Point(100, 114);
            this.txtBody.Name = "txtBody";
            this.txtBody.Size = new System.Drawing.Size(534, 239);
            this.txtBody.TabIndex = 6;
            this.txtBody.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 360);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Attachment:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Reply To:";
            // 
            // txtreplyToEmail
            // 
            this.txtreplyToEmail.Location = new System.Drawing.Point(100, 60);
            this.txtreplyToEmail.Name = "txtreplyToEmail";
            this.txtreplyToEmail.Size = new System.Drawing.Size(257, 20);
            this.txtreplyToEmail.TabIndex = 12;
            // 
            // lnkAttachFile
            // 
            this.lnkAttachFile.AutoSize = true;
            this.lnkAttachFile.Location = new System.Drawing.Point(3, 0);
            this.lnkAttachFile.Name = "lnkAttachFile";
            this.lnkAttachFile.Size = new System.Drawing.Size(69, 13);
            this.lnkAttachFile.TabIndex = 14;
            this.lnkAttachFile.TabStop = true;
            this.lnkAttachFile.Text = "Attached File";
            this.lnkAttachFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAttachFile_LinkClicked);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLabel,
            this.tsProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(653, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatusLabel
            // 
            this.tsStatusLabel.Name = "tsStatusLabel";
            this.tsStatusLabel.Size = new System.Drawing.Size(536, 17);
            this.tsStatusLabel.Spring = true;
            this.tsStatusLabel.Text = "Message";
            this.tsStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsProgressBar
            // 
            this.tsProgressBar.Name = "tsProgressBar";
            this.tsProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // flpAttachments
            // 
            this.flpAttachments.AutoScroll = true;
            this.flpAttachments.Controls.Add(this.lnkAttachFile);
            this.flpAttachments.Location = new System.Drawing.Point(102, 359);
            this.flpAttachments.Name = "flpAttachments";
            this.flpAttachments.Size = new System.Drawing.Size(532, 54);
            this.flpAttachments.TabIndex = 19;
            // 
            // AttachmentContextMenu
            // 
            this.AttachmentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpen,
            this.menuRemove});
            this.AttachmentContextMenu.Name = "AttachmentContextMenu";
            this.AttachmentContextMenu.Size = new System.Drawing.Size(118, 48);
            this.AttachmentContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.AttachmentContextMenu_ItemClicked);
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.Size = new System.Drawing.Size(117, 22);
            this.menuOpen.Text = "Open";
            // 
            // menuRemove
            // 
            this.menuRemove.Name = "menuRemove";
            this.menuRemove.Size = new System.Drawing.Size(117, 22);
            this.menuRemove.Text = "Remove";
            // 
            // btnAttach
            // 
            this.btnAttach.Enabled = false;
            this.btnAttach.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnAttach.ForeColor = System.Drawing.Color.Black;
            this.btnAttach.Image = global::Semnox.Parafait.Communication.Properties.Resources.Attach;
            this.btnAttach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAttach.Location = new System.Drawing.Point(102, 419);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(80, 23);
            this.btnAttach.TabIndex = 18;
            this.btnAttach.Text = "Attach";
            this.btnAttach.UseVisualStyleBackColor = false;
            this.btnAttach.Visible = false;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = global::Semnox.Parafait.Communication.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(300, 419);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSend
            // 
            this.btnSend.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnSend.ForeColor = System.Drawing.Color.Black;
            this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
            this.btnSend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSend.Location = new System.Drawing.Point(201, 419);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 23);
            this.btnSend.TabIndex = 10;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // SendEmailUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 468);
            this.Controls.Add(this.flpAttachments);
            this.Controls.Add(this.btnAttach);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtreplyToEmail);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBody);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCCEmails);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtToEmails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SendEmailUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send Email";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.flpAttachments.ResumeLayout(false);
            this.flpAttachments.PerformLayout();
            this.AttachmentContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtToEmails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCCEmails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox txtBody;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtreplyToEmail;
        private System.Windows.Forms.LinkLabel lnkAttachFile;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar tsProgressBar;
        private System.Windows.Forms.Button btnAttach;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FlowLayoutPanel flpAttachments;
        private System.Windows.Forms.ContextMenuStrip AttachmentContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuRemove;
    }
}