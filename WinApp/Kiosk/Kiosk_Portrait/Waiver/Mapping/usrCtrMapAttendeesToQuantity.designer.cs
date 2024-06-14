using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlMapAttendeesToQuantity
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
            this.btnProductQty = new System.Windows.Forms.Button();
            this.pnlProductQty = new System.Windows.Forms.Panel();
            this.lblQty = new System.Windows.Forms.Label();
            this.btnParticipantQty = new System.Windows.Forms.Button();
            this.pbxMappingCompleted = new System.Windows.Forms.PictureBox();
            this.pnlProductQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMappingCompleted)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProductQty
            // 
            this.btnProductQty.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.PersonIcon;
            this.btnProductQty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProductQty.FlatAppearance.BorderSize = 0;
            this.btnProductQty.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProductQty.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProductQty.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProductQty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductQty.ForeColor = System.Drawing.Color.Black;
            this.btnProductQty.Location = new System.Drawing.Point(29, 14);
            this.btnProductQty.Margin = new System.Windows.Forms.Padding(20);
            this.btnProductQty.Name = "btnProductQty";
            this.btnProductQty.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnProductQty.Size = new System.Drawing.Size(87, 78);
            this.btnProductQty.TabIndex = 3;
            this.btnProductQty.Text = "01";
            this.btnProductQty.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnProductQty.UseVisualStyleBackColor = true;
            this.btnProductQty.Click += new System.EventHandler(this.UsrCtrlMapAttendees_Click);
            // 
            // pnlProductQty
            // 
            this.pnlProductQty.Controls.Add(this.lblQty);
            this.pnlProductQty.Controls.Add(this.btnProductQty);
            this.pnlProductQty.Location = new System.Drawing.Point(3, 3);
            this.pnlProductQty.Name = "pnlProductQty";
            this.pnlProductQty.Size = new System.Drawing.Size(144, 104);
            this.pnlProductQty.TabIndex = 20025;
            this.pnlProductQty.Click += new System.EventHandler(this.UsrCtrlMapAttendees_Click);
            // 
            // lblQty
            // 
            this.lblQty.BackColor = System.Drawing.Color.Transparent;
            this.lblQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQty.ForeColor = System.Drawing.Color.Black;
            this.lblQty.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblQty.Location = new System.Drawing.Point(47, 56);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(51, 25);
            this.lblQty.TabIndex = 20024;
            this.lblQty.Text = "01";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblQty.Visible = false;
            this.lblQty.Click += new System.EventHandler(this.UsrCtrlMapAttendees_Click);
            // 
            // btnParticipantQty
            // 
            this.btnParticipantQty.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Slot;
            this.btnParticipantQty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnParticipantQty.FlatAppearance.BorderSize = 0;
            this.btnParticipantQty.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnParticipantQty.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnParticipantQty.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnParticipantQty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParticipantQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParticipantQty.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnParticipantQty.Location = new System.Drawing.Point(159, 3);
            this.btnParticipantQty.Margin = new System.Windows.Forms.Padding(20);
            this.btnParticipantQty.Name = "btnParticipantQty";
            this.btnParticipantQty.Padding = new System.Windows.Forms.Padding(24, 0, 6, 0);
            this.btnParticipantQty.Size = new System.Drawing.Size(665, 104);
            this.btnParticipantQty.TabIndex = 20023;
            this.btnParticipantQty.Text = "Quantity 1";
            this.btnParticipantQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnParticipantQty.UseVisualStyleBackColor = true;
            this.btnParticipantQty.Click += new System.EventHandler(this.UsrCtrlMapAttendees_Click);
            // 
            // pbxMappingCompleted
            // 
            this.pbxMappingCompleted.BackColor = System.Drawing.Color.Transparent;
            this.pbxMappingCompleted.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.selected_tick;
            this.pbxMappingCompleted.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxMappingCompleted.Location = new System.Drawing.Point(748, 34);
            this.pbxMappingCompleted.Name = "pbxMappingCompleted";
            this.pbxMappingCompleted.Size = new System.Drawing.Size(50, 50);
            this.pbxMappingCompleted.TabIndex = 20026;
            this.pbxMappingCompleted.TabStop = false;
            // 
            // UsrCtrlMapAttendeesToQuantity
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.pbxMappingCompleted);
            this.Controls.Add(this.pnlProductQty);
            this.Controls.Add(this.btnParticipantQty);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.Name = "UsrCtrlMapAttendeesToQuantity";
            this.Size = new System.Drawing.Size(824, 110);
            this.pnlProductQty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxMappingCompleted)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnProductQty;
        private System.Windows.Forms.Panel pnlProductQty;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Button btnParticipantQty;
        private System.Windows.Forms.PictureBox pbxMappingCompleted;
    }
}
