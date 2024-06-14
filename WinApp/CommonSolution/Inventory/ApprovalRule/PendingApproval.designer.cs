namespace Semnox.Parafait.Inventory
{
    partial class PendingApproval
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
            this.lnkView = new System.Windows.Forms.LinkLabel();
            this.lnkReject = new System.Windows.Forms.LinkLabel();
            this.lnkApprove = new System.Windows.Forms.LinkLabel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblTypeOfApproval = new System.Windows.Forms.Label();
            this.lblInitiator = new System.Windows.Forms.Label();
            this.lblLevelOfApproval = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lnkView
            // 
            this.lnkView.AutoSize = true;
            this.lnkView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkView.Location = new System.Drawing.Point(240, 123);
            this.lnkView.Name = "lnkView";
            this.lnkView.Size = new System.Drawing.Size(34, 13);
            this.lnkView.TabIndex = 9;
            this.lnkView.TabStop = true;
            this.lnkView.Text = "View";
            // 
            // lnkReject
            // 
            this.lnkReject.AutoSize = true;
            this.lnkReject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkReject.Location = new System.Drawing.Point(138, 123);
            this.lnkReject.Name = "lnkReject";
            this.lnkReject.Size = new System.Drawing.Size(44, 13);
            this.lnkReject.TabIndex = 8;
            this.lnkReject.TabStop = true;
            this.lnkReject.Text = "Reject";
            // 
            // lnkApprove
            // 
            this.lnkApprove.AutoSize = true;
            this.lnkApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkApprove.Location = new System.Drawing.Point(26, 123);
            this.lnkApprove.Name = "lnkApprove";
            this.lnkApprove.Size = new System.Drawing.Size(54, 13);
            this.lnkApprove.TabIndex = 7;
            this.lnkApprove.TabStop = true;
            this.lnkApprove.Text = "Approve";
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(450, 6);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(196, 19);
            this.lblDate.TabIndex = 6;
            this.lblDate.Text = "dd-MMM-yyyy";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(16, 32);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(425, 45);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "Name of Pending approval";
            // 
            // lblTypeOfApproval
            // 
            this.lblTypeOfApproval.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTypeOfApproval.Location = new System.Drawing.Point(16, 6);
            this.lblTypeOfApproval.Name = "lblTypeOfApproval";
            this.lblTypeOfApproval.Size = new System.Drawing.Size(425, 19);
            this.lblTypeOfApproval.TabIndex = 10;
            this.lblTypeOfApproval.Text = "Name approval";
            // 
            // lblInitiator
            // 
            this.lblInitiator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitiator.Location = new System.Drawing.Point(16, 88);
            this.lblInitiator.Name = "lblInitiator";
            this.lblInitiator.Size = new System.Drawing.Size(500, 19);
            this.lblInitiator.TabIndex = 11;
            this.lblInitiator.Text = "Initiated By:";
            // 
            // lblLevelOfApproval
            // 
            this.lblLevelOfApproval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLevelOfApproval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevelOfApproval.Location = new System.Drawing.Point(447, 32);
            this.lblLevelOfApproval.Name = "lblLevelOfApproval";
            this.lblLevelOfApproval.Size = new System.Drawing.Size(196, 45);
            this.lblLevelOfApproval.TabIndex = 12;
            this.lblLevelOfApproval.Text = "Approval Level:";
            this.lblLevelOfApproval.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PendingApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblLevelOfApproval);
            this.Controls.Add(this.lblInitiator);
            this.Controls.Add(this.lblTypeOfApproval);
            this.Controls.Add(this.lnkView);
            this.Controls.Add(this.lnkReject);
            this.Controls.Add(this.lnkApprove);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblMessage);
            this.Name = "PendingApproval";
            this.Size = new System.Drawing.Size(646, 157);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.LinkLabel lnkView;
        public System.Windows.Forms.LinkLabel lnkReject;
        public System.Windows.Forms.LinkLabel lnkApprove;
        public System.Windows.Forms.Label lblDate;
        public System.Windows.Forms.Label lblMessage;
        public System.Windows.Forms.Label lblTypeOfApproval;
        public System.Windows.Forms.Label lblInitiator;
        public System.Windows.Forms.Label lblLevelOfApproval;
    }
}
