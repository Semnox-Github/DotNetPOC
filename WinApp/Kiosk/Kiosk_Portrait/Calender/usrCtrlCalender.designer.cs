using System;

namespace Parafait_Kiosk
{
    partial class usrCtrlCalender
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.flpCalender = new System.Windows.Forms.FlowLayoutPanel();
            this.dgvCalender = new System.Windows.Forms.DataGridView();
            this.vScrollBarCalender = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.CalenderCol1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CalenderCol2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CalenderCol3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpCalender.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalender)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // flpCalender
            // 
            this.flpCalender.AutoSize = true;
            this.flpCalender.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCalender.BackColor = System.Drawing.Color.Transparent;
            this.flpCalender.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.flpCalender.Controls.Add(this.dgvCalender);
            this.flpCalender.Controls.Add(this.vScrollBarCalender);
            this.flpCalender.Location = new System.Drawing.Point(0, 0);
            this.flpCalender.Margin = new System.Windows.Forms.Padding(0);
            this.flpCalender.Name = "flpCalender";
            this.flpCalender.Size = new System.Drawing.Size(423, 240);
            this.flpCalender.TabIndex = 20026;
            // 
            // dgvCalender
            // 
            this.dgvCalender.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCalender.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCalender.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvCalender.ColumnHeadersVisible = false;
            this.dgvCalender.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CalenderCol1,
            this.CalenderCol2,
            this.CalenderCol3});
            this.dgvCalender.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvCalender.Location = new System.Drawing.Point(0, 0);
            this.dgvCalender.Margin = new System.Windows.Forms.Padding(0);
            this.dgvCalender.MinimumSize = new System.Drawing.Size(0, 240);
            this.dgvCalender.MultiSelect = false;
            this.dgvCalender.Name = "dgvCalender";
            this.dgvCalender.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvCalender.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCalender.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvCalender.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvCalender.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCalender.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvCalender.RowTemplate.Height = 60;
            this.dgvCalender.RowTemplate.ReadOnly = true;
            this.dgvCalender.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCalender.Size = new System.Drawing.Size(360, 240);
            this.dgvCalender.TabIndex = 132;
            this.dgvCalender.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCalender_CellClick);
            // 
            // vScrollBarCalender
            // 
            this.vScrollBarCalender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarCalender.AutoHide = true;
            this.vScrollBarCalender.BackColor = System.Drawing.SystemColors.Control;
            this.vScrollBarCalender.DataGridView = this.dgvCalender;
            this.vScrollBarCalender.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.vScrollBarCalender.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.vScrollBarCalender.Location = new System.Drawing.Point(360, 0);
            this.vScrollBarCalender.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollBarCalender.MinimumSize = new System.Drawing.Size(63, 240);
            this.vScrollBarCalender.Name = "vScrollBarCalender";
            this.vScrollBarCalender.ScrollableControl = null;
            this.vScrollBarCalender.ScrollViewer = null;
            this.vScrollBarCalender.Size = new System.Drawing.Size(63, 240);
            this.vScrollBarCalender.TabIndex = 20022;
            this.vScrollBarCalender.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.vScrollBarCalender.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            // 
            // CalenderCol1
            // 
            this.CalenderCol1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CalenderCol1.DefaultCellStyle = dataGridViewCellStyle1;
            this.CalenderCol1.HeaderText = "CalenderCol1";
            this.CalenderCol1.Name = "CalenderCol1";
            this.CalenderCol1.ReadOnly = true;
            this.CalenderCol1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CalenderCol1.Width = 120;
            // 
            // CalenderCol2
            // 
            this.CalenderCol2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CalenderCol2.DefaultCellStyle = dataGridViewCellStyle2;
            this.CalenderCol2.HeaderText = "CalenderCol2";
            this.CalenderCol2.Name = "CalenderCol2";
            this.CalenderCol2.ReadOnly = true;
            this.CalenderCol2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CalenderCol2.Width = 120;
            // 
            // CalenderCol3
            // 
            this.CalenderCol3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CalenderCol3.DefaultCellStyle = dataGridViewCellStyle3;
            this.CalenderCol3.HeaderText = "CalenderCol3";
            this.CalenderCol3.Name = "CalenderCol3";
            this.CalenderCol3.ReadOnly = true;
            this.CalenderCol3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CalenderCol3.Width = 120;
            // 
            // usrCtrlCalender
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.flpCalender);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "usrCtrlCalender";
            this.Size = new System.Drawing.Size(423, 240);
            this.flpCalender.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView vScrollBarCalender;
        private System.Windows.Forms.DataGridView dgvCalender;
        private System.Windows.Forms.FlowLayoutPanel flpCalender;
        private System.Windows.Forms.DataGridViewTextBoxColumn CalenderCol1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CalenderCol2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CalenderCol3;
    }
}
