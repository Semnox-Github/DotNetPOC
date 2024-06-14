namespace Semnox.Core.GenericUtilities.Excel
{
    partial class ImportToExcelUI
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
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lnkError = new System.Windows.Forms.LinkLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpNotes = new System.Windows.Forms.GroupBox();
            this.lblNote1 = new System.Windows.Forms.Label();
            this.grpNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTemplate
            // 
            this.btnTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTemplate.Location = new System.Drawing.Point(12, 235);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(87, 30);
            this.btnTemplate.TabIndex = 0;
            this.btnTemplate.Text = "Template";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpload.Location = new System.Drawing.Point(120, 235);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 30);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(219, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 151);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(386, 32);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "message";
            // 
            // lnkError
            // 
            this.lnkError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkError.AutoSize = true;
            this.lnkError.Location = new System.Drawing.Point(313, 243);
            this.lnkError.Name = "lnkError";
            this.lnkError.Size = new System.Drawing.Size(36, 15);
            this.lnkError.TabIndex = 4;
            this.lnkError.TabStop = true;
            this.lnkError.Text = "Error";
            this.lnkError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkError_LinkClicked);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 186);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(383, 23);
            this.progressBar.TabIndex = 5;
            // 
            // grpNotes
            // 
            this.grpNotes.Controls.Add(this.lblNote1);
            this.grpNotes.Location = new System.Drawing.Point(15, 12);
            this.grpNotes.Name = "grpNotes";
            this.grpNotes.Size = new System.Drawing.Size(380, 136);
            this.grpNotes.TabIndex = 7;
            this.grpNotes.TabStop = false;
            this.grpNotes.Text = "Notes";
            // 
            // lblNote1
            // 
            this.lblNote1.Location = new System.Drawing.Point(6, 17);
            this.lblNote1.Name = "lblNote1";
            this.lblNote1.Size = new System.Drawing.Size(369, 110);
            this.lblNote1.TabIndex = 0;
            // 
            // ImportToExcelUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 276);
            this.Controls.Add(this.grpNotes);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lnkError);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnTemplate);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportToExcelUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Customers";
            this.Load += new System.EventHandler(this.ImportToExcelUI_Load);
            this.grpNotes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button btnTemplate;
        protected System.Windows.Forms.Button btnUpload;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Label lblMessage;
        protected System.Windows.Forms.LinkLabel lnkError;
        protected System.Windows.Forms.ProgressBar progressBar;
        protected System.Windows.Forms.GroupBox grpNotes;
        protected System.Windows.Forms.Label lblNote1;
    }
}