namespace Parafait_POS.Waivers
{
    partial class usrCtlWaiverSet
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
            this.rBtnSelectedWaiverSet = new System.Windows.Forms.RadioButton();
            this.lblWaiverSetName = new System.Windows.Forms.Label();
            this.tPnlWaiverSet = new System.Windows.Forms.TableLayoutPanel();
            this.tPnlWaivers = new System.Windows.Forms.TableLayoutPanel();
            this.tPnlWaiverSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // rBtnSelectedWaiverSet
            // 
            this.rBtnSelectedWaiverSet.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnSelectedWaiverSet.Font = new System.Drawing.Font("Arial", 15F);
            this.rBtnSelectedWaiverSet.Location = new System.Drawing.Point(3, 3);
            this.rBtnSelectedWaiverSet.Name = "rBtnSelectedWaiverSet";
            this.rBtnSelectedWaiverSet.Size = new System.Drawing.Size(29, 33);
            this.rBtnSelectedWaiverSet.TabIndex = 0;
            this.rBtnSelectedWaiverSet.TabStop = true;
            this.rBtnSelectedWaiverSet.UseVisualStyleBackColor = true;
            this.rBtnSelectedWaiverSet.CheckedChanged += new System.EventHandler(this.rBtnSelectedWaiverSet_CheckedChanged);
            // 
            // lblWaiverSetName
            // 
            this.lblWaiverSetName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverSetName.Location = new System.Drawing.Point(38, 0);
            this.lblWaiverSetName.Name = "lblWaiverSetName";
            this.lblWaiverSetName.Size = new System.Drawing.Size(234, 40);
            this.lblWaiverSetName.TabIndex = 2;
            this.lblWaiverSetName.Text = "label1";
            this.lblWaiverSetName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblWaiverSetName.AutoEllipsis = true;
            // 
            // tPnlWaiverSet
            // 
            this.tPnlWaiverSet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tPnlWaiverSet.ColumnCount = 3;
            this.tPnlWaiverSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tPnlWaiverSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tPnlWaiverSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 390F));
            this.tPnlWaiverSet.Controls.Add(this.rBtnSelectedWaiverSet, 0, 0);
            this.tPnlWaiverSet.Controls.Add(this.lblWaiverSetName, 1, 0);
            this.tPnlWaiverSet.Controls.Add(this.tPnlWaivers, 2, 0);
            this.tPnlWaiverSet.Font = new System.Drawing.Font("Arial", 9F);
            this.tPnlWaiverSet.Location = new System.Drawing.Point(3, 3);
            this.tPnlWaiverSet.Name = "tPnlWaiverSet";
            this.tPnlWaiverSet.RowCount = 1;
            this.tPnlWaiverSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 680F));
            this.tPnlWaiverSet.Size = new System.Drawing.Size(668, 45);
            this.tPnlWaiverSet.TabIndex = 3;
            // 
            // tPnlWaivers
            // 
            this.tPnlWaivers.ColumnCount = 4;
            this.tPnlWaivers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tPnlWaivers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tPnlWaivers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tPnlWaivers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tPnlWaivers.Font = new System.Drawing.Font("Arial", 15F);
            this.tPnlWaivers.Location = new System.Drawing.Point(278, 3);
            this.tPnlWaivers.Name = "tPnlWaivers";
            this.tPnlWaivers.RowCount = 1;
            this.tPnlWaivers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tPnlWaivers.Size = new System.Drawing.Size(375, 38);
            this.tPnlWaivers.TabIndex = 3;
            // 
            // usrCtlWaiverSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tPnlWaiverSet);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "usrCtlWaiverSet";
            this.Size = new System.Drawing.Size(672, 51);
            this.tPnlWaiverSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rBtnSelectedWaiverSet;
        private System.Windows.Forms.Label lblWaiverSetName;
        private System.Windows.Forms.TableLayoutPanel tPnlWaiverSet; 
        private System.Windows.Forms.TableLayoutPanel tPnlWaivers;
    }
}
