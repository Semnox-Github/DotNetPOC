﻿namespace Parafait_Kiosk
{
    partial class frmFAQ
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flpTopics = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSampleTopic = new System.Windows.Forms.Button();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.wbFAQ = new System.Windows.Forms.WebBrowser();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flpTopics.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(5, 111);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flpTopics);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.vScrollBar);
            this.splitContainer1.Panel2.Controls.Add(this.wbFAQ);
            this.splitContainer1.Size = new System.Drawing.Size(728, 403);
            this.splitContainer1.SplitterDistance = 275;
            this.splitContainer1.TabIndex = 0;
            // 
            // flpTopics
            // 
            this.flpTopics.AutoScroll = true;
            this.flpTopics.Controls.Add(this.btnSampleTopic);
            this.flpTopics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTopics.Location = new System.Drawing.Point(0, 0);
            this.flpTopics.Name = "flpTopics";
            this.flpTopics.Size = new System.Drawing.Size(275, 403);
            this.flpTopics.TabIndex = 0;
            // 
            // btnSampleTopic
            // 
            this.btnSampleTopic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSampleTopic.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleTopic.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.terms_button;
            this.btnSampleTopic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSampleTopic.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnSampleTopic.FlatAppearance.BorderSize = 0;
            this.btnSampleTopic.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleTopic.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleTopic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleTopic.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleTopic.ForeColor = System.Drawing.Color.White;
            this.btnSampleTopic.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSampleTopic.Location = new System.Drawing.Point(30, 3);
            this.btnSampleTopic.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.btnSampleTopic.Name = "btnSampleTopic";
            this.btnSampleTopic.Size = new System.Drawing.Size(199, 52);
            this.btnSampleTopic.TabIndex = 12;
            this.btnSampleTopic.Text = "Buy New Card";
            this.btnSampleTopic.UseVisualStyleBackColor = false;
            this.btnSampleTopic.Visible = false;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.LargeChange = 100;
            this.vScrollBar.Location = new System.Drawing.Point(394, 0);
            this.vScrollBar.Maximum = 3200;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(55, 403);
            this.vScrollBar.SmallChange = 10;
            this.vScrollBar.TabIndex = 155;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // wbFAQ
            // 
            this.wbFAQ.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbFAQ.Location = new System.Drawing.Point(2, 4);
            this.wbFAQ.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbFAQ.Name = "wbFAQ";
            this.wbFAQ.ScrollBarsEnabled = false;
            this.wbFAQ.Size = new System.Drawing.Size(392, 399);
            this.wbFAQ.TabIndex = 0;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(12, 12);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(153, 80);
            this.btnPrev.TabIndex = 153;
            this.btnPrev.Text = "BACK";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(12, 29);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(716, 51);
            this.lblHeading.TabIndex = 154;
            this.lblHeading.Text = "Frequently Asked Questions";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(740, 100);
            this.panel1.TabIndex = 155;
            // 
            // frmFAQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(750, 555);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmFAQ";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FAQ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFAQ_FormClosed);
            this.Load += new System.EventHandler(this.frmFAQ_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.flpTopics.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flpTopics;
        private System.Windows.Forms.WebBrowser wbFAQ;
        private System.Windows.Forms.Button btnSampleTopic;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.Panel panel1;
    }
}