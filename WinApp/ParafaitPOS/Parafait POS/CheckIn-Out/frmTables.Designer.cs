namespace Parafait_POS.CheckIn_Out
{
    partial class frmTables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTables));
            this.btnCancel = new System.Windows.Forms.Button();
            this.flpFacilities = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSelectedTable = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutVerticalScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.tableLayoutHorizontalScrollBarView = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.tblPanelTables = new System.Windows.Forms.Integration.ElementHost();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(351, 506);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 39);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // flpFacilities
            // 
            this.flpFacilities.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flpFacilities.Location = new System.Drawing.Point(13, 13);
            this.flpFacilities.Name = "flpFacilities";
            this.flpFacilities.Size = new System.Drawing.Size(806, 32);
            this.flpFacilities.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selected:";
            // 
            // lblSelectedTable
            // 
            this.lblSelectedTable.AutoSize = true;
            this.lblSelectedTable.BackColor = System.Drawing.Color.Yellow;
            this.lblSelectedTable.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTable.Location = new System.Drawing.Point(104, 50);
            this.lblSelectedTable.Name = "lblSelectedTable";
            this.lblSelectedTable.Size = new System.Drawing.Size(86, 22);
            this.lblSelectedTable.TabIndex = 4;
            this.lblSelectedTable.Text = "Selected";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tableLayoutVerticalScrollBarView);
            this.panel1.Controls.Add(this.tableLayoutHorizontalScrollBarView);
            this.panel1.Controls.Add(this.tblPanelTables);
            this.panel1.Location = new System.Drawing.Point(13, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 427);
            this.panel1.TabIndex = 5;
            // 
            // tableLayoutVerticalScrollBarView
            // 
            this.tableLayoutVerticalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutVerticalScrollBarView.AutoHide = false;
            this.tableLayoutVerticalScrollBarView.DataGridView = null;
            this.tableLayoutVerticalScrollBarView.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.DownButtonBackgroundImage")));
            this.tableLayoutVerticalScrollBarView.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.DownButtonDisabledBackgroundImage")));
            this.tableLayoutVerticalScrollBarView.Location = new System.Drawing.Point(762, 5);
            this.tableLayoutVerticalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutVerticalScrollBarView.Name = "tableLayoutVerticalScrollBarView";
            this.tableLayoutVerticalScrollBarView.ScrollableControl = null;
            this.tableLayoutVerticalScrollBarView.ScrollViewer = null;
            this.tableLayoutVerticalScrollBarView.Size = new System.Drawing.Size(40, 377);
            this.tableLayoutVerticalScrollBarView.TabIndex = 2;
            this.tableLayoutVerticalScrollBarView.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.UpButtonBackgroundImage")));
            this.tableLayoutVerticalScrollBarView.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.UpButtonDisabledBackgroundImage")));
            // 
            // tableLayoutHorizontalScrollBarView
            // 
            this.tableLayoutHorizontalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutHorizontalScrollBarView.AutoHide = false;
            this.tableLayoutHorizontalScrollBarView.DataGridView = null;
            this.tableLayoutHorizontalScrollBarView.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.LeftButtonBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.LeftButtonDisabledBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.Location = new System.Drawing.Point(3, 383);
            this.tableLayoutHorizontalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutHorizontalScrollBarView.Name = "tableLayoutHorizontalScrollBarView";
            this.tableLayoutHorizontalScrollBarView.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.RightButtonBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.RightButtonDisabledBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.ScrollableControl = null;
            this.tableLayoutHorizontalScrollBarView.ScrollViewer = null;
            this.tableLayoutHorizontalScrollBarView.Size = new System.Drawing.Size(760, 40);
            this.tableLayoutHorizontalScrollBarView.TabIndex = 1;
            // 
            // tblPanelTables
            // 
            this.tblPanelTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblPanelTables.Location = new System.Drawing.Point(3, 5);
            this.tblPanelTables.Name = "tblPanelTables";
            this.tblPanelTables.Size = new System.Drawing.Size(756, 375);
            this.tblPanelTables.TabIndex = 0;
            this.tblPanelTables.Text = "elementHost1";
            this.tblPanelTables.Child = null;
            // 
            // frmTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(831, 546);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblSelectedTable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flpFacilities);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmTables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Table";
            this.Load += new System.EventHandler(this.frmTables_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FlowLayoutPanel flpFacilities;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSelectedTable;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Integration.ElementHost tblPanelTables;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView tableLayoutVerticalScrollBarView;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView tableLayoutHorizontalScrollBarView;
    }
}