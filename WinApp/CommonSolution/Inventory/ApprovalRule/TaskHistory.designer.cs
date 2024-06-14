namespace Semnox.Parafait.Inventory
{
    partial class TaskHistory
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
            this.lnkHistoryView = new System.Windows.Forms.LinkLabel();
            this.lblHistoryDate = new System.Windows.Forms.Label();
            this.lblHistoryMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lnkHistoryView
            // 
            this.lnkHistoryView.AutoSize = true;
            this.lnkHistoryView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkHistoryView.Location = new System.Drawing.Point(9, 90);
            this.lnkHistoryView.Name = "lnkHistoryView";
            this.lnkHistoryView.Size = new System.Drawing.Size(34, 13);
            this.lnkHistoryView.TabIndex = 7;
            this.lnkHistoryView.TabStop = true;
            this.lnkHistoryView.Text = "View";
            this.lnkHistoryView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHistoryView_LinkClicked);
            // 
            // lblHistoryDate
            // 
            this.lblHistoryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHistoryDate.Location = new System.Drawing.Point(9, 60);
            this.lblHistoryDate.Name = "lblHistoryDate";
            this.lblHistoryDate.Size = new System.Drawing.Size(137, 19);
            this.lblHistoryDate.TabIndex = 6;
            this.lblHistoryDate.Text = "dd-MMM-yyyy";
            this.lblHistoryDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHistoryMessage
            // 
            this.lblHistoryMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHistoryMessage.Location = new System.Drawing.Point(9, 11);
            this.lblHistoryMessage.Name = "lblHistoryMessage";
            this.lblHistoryMessage.Size = new System.Drawing.Size(217, 41);
            this.lblHistoryMessage.TabIndex = 5;
            this.lblHistoryMessage.Text = "Name of Pending approval";
            // 
            // TaskHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lnkHistoryView);
            this.Controls.Add(this.lblHistoryDate);
            this.Controls.Add(this.lblHistoryMessage);
            this.Name = "TaskHistory";
            this.Size = new System.Drawing.Size(235, 114);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.LinkLabel lnkHistoryView;
        public System.Windows.Forms.Label lblHistoryDate;
        public System.Windows.Forms.Label lblHistoryMessage;
    }
}
