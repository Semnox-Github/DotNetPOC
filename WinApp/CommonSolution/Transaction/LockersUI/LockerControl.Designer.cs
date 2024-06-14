namespace Semnox.Parafait.Transaction
{
    partial class LockerControl
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblIdentifier = new System.Windows.Forms.Label();
            this.lblLockerId = new System.Windows.Forms.Label();
            this.txtIdentifier = new System.Windows.Forms.TextBox();
            this.txtLockerName = new System.Windows.Forms.TextBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.chkDisable = new System.Windows.Forms.CheckBox();
            this.txtPositionX = new System.Windows.Forms.TextBox();
            this.lblPositionX = new System.Windows.Forms.Label();
            this.txtPositionY = new System.Windows.Forms.TextBox();
            this.lblPositionY = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(5, 10);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 20);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIdentifier
            // 
            this.lblIdentifier.Location = new System.Drawing.Point(5, 30);
            this.lblIdentifier.Name = "lblIdentifier";
            this.lblIdentifier.Size = new System.Drawing.Size(80, 20);
            this.lblIdentifier.TabIndex = 1;
            this.lblIdentifier.Text = "Identifier:";
            this.lblIdentifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLockerId
            // 
            this.lblLockerId.Location = new System.Drawing.Point(165, 160);
            this.lblLockerId.Name = "lblLockerId";
            this.lblLockerId.Size = new System.Drawing.Size(43, 17);
            this.lblLockerId.TabIndex = 2;
            this.lblLockerId.Text = "LockerId";
            this.lblLockerId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLockerId.Visible = false;
            // 
            // txtIdentifier
            // 
            this.txtIdentifier.Location = new System.Drawing.Point(90, 31);
            this.txtIdentifier.Name = "txtIdentifier";
            this.txtIdentifier.Size = new System.Drawing.Size(40, 20);
            this.txtIdentifier.TabIndex = 1;
            this.txtIdentifier.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIdentifier_KeyPress);
            // 
            // txtLockerName
            // 
            this.txtLockerName.Location = new System.Drawing.Point(90, 10);
            this.txtLockerName.Name = "txtLockerName";
            this.txtLockerName.Size = new System.Drawing.Size(40, 20);
            this.txtLockerName.TabIndex = 0;
            // 
            // chkActive
            // 
            this.chkActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(5, 53);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(98, 18);
            this.chkActive.TabIndex = 2;
            this.chkActive.Text = "Active";
            this.chkActive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkActive.UseVisualStyleBackColor = false;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // chkDisable
            // 
            this.chkDisable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDisable.Location = new System.Drawing.Point(5, 73);
            this.chkDisable.Name = "chkDisable";
            this.chkDisable.Size = new System.Drawing.Size(98, 18);
            this.chkDisable.TabIndex = 3;
            this.chkDisable.Text = "Disable";
            this.chkDisable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDisable.UseVisualStyleBackColor = false;
            this.chkDisable.CheckedChanged += new System.EventHandler(this.chkDisable_CheckedChanged);
            // 
            // txtPositionX
            // 
            this.txtPositionX.Location = new System.Drawing.Point(90, 94);
            this.txtPositionX.Name = "txtPositionX";
            this.txtPositionX.Size = new System.Drawing.Size(40, 20);
            this.txtPositionX.TabIndex = 4;
            this.txtPositionX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIdentifier_KeyPress);
            // 
            // lblPositionX
            // 
            this.lblPositionX.Location = new System.Drawing.Point(5, 94);
            this.lblPositionX.Name = "lblPositionX";
            this.lblPositionX.Size = new System.Drawing.Size(80, 20);
            this.lblPositionX.TabIndex = 7;
            this.lblPositionX.Text = "Position X:";
            this.lblPositionX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPositionY
            // 
            this.txtPositionY.Location = new System.Drawing.Point(90, 115);
            this.txtPositionY.Name = "txtPositionY";
            this.txtPositionY.Size = new System.Drawing.Size(40, 20);
            this.txtPositionY.TabIndex = 5;
            this.txtPositionY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIdentifier_KeyPress);
            // 
            // lblPositionY
            // 
            this.lblPositionY.Location = new System.Drawing.Point(5, 114);
            this.lblPositionY.Name = "lblPositionY";
            this.lblPositionY.Size = new System.Drawing.Size(80, 20);
            this.lblPositionY.TabIndex = 9;
            this.lblPositionY.Text = "Position Y:";
            this.lblPositionY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LockerControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtPositionY);
            this.Controls.Add(this.lblPositionY);
            this.Controls.Add(this.txtPositionX);
            this.Controls.Add(this.lblPositionX);
            this.Controls.Add(this.chkDisable);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.txtLockerName);
            this.Controls.Add(this.txtIdentifier);
            this.Controls.Add(this.lblLockerId);
            this.Controls.Add(this.lblIdentifier);
            this.Controls.Add(this.lblName);
            this.Name = "LockerControl";
            this.Size = new System.Drawing.Size(196, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblIdentifier;
        public System.Windows.Forms.TextBox txtLockerName;
        private System.Windows.Forms.Label lblPositionX;
        private System.Windows.Forms.Label lblPositionY;
        public System.Windows.Forms.TextBox txtIdentifier;
        public System.Windows.Forms.CheckBox chkActive;
        public System.Windows.Forms.CheckBox chkDisable;
        public System.Windows.Forms.TextBox txtPositionX;
        public System.Windows.Forms.TextBox txtPositionY;
        public System.Windows.Forms.Label lblLockerId;
    }
}
