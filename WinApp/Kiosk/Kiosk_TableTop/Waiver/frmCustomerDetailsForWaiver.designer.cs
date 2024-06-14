using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    partial class frmCustomerDetailsForWaiver
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCustomer = new System.Windows.Forms.DataGridView();
            this.customerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerRelationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hasSignedWaiverSet = new System.Windows.Forms.DataGridViewImageColumn();
            this.signFor = new System.Windows.Forms.DataGridViewImageColumn();
            this.RelationshipTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtMessage = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSelection = new System.Windows.Forms.Label();
            this.lblWaiverSignedStatus = new System.Windows.Forms.Label();
            this.lblRelation = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.bigVerticalScrollCustomer = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.lblSignatoryCustomerName = new System.Windows.Forms.Label();
            this.btnAddNewRelations = new System.Windows.Forms.Button();
            this.btnProceed = new System.Windows.Forms.Button();
            this.lblWaiverSet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(670, 871);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(31, 524);
            this.btnCancel.TabIndex = 1027;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(190, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1529, 117);
            this.label2.TabIndex = 9;
            this.label2.Text = "Please include minor children, if you want to sign for them";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvCustomer
            // 
            this.dgvCustomer.AllowUserToAddRows = false;
            this.dgvCustomer.BackgroundColor = System.Drawing.Color.White;
            this.dgvCustomer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCustomer.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvCustomer.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Gotham Rounded Bold", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomer.ColumnHeadersVisible = false;
            this.dgvCustomer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customerName,
            this.customerRelationType,
            this.hasSignedWaiverSet,
            this.signFor,
            this.RelationshipTypeId});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCustomer.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCustomer.EnableHeadersVisualStyles = false;
            this.dgvCustomer.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvCustomer.Location = new System.Drawing.Point(55, 69);
            this.dgvCustomer.MultiSelect = false;
            this.dgvCustomer.Name = "dgvCustomer";
            this.dgvCustomer.ReadOnly = true;
            this.dgvCustomer.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvCustomer.RowHeadersVisible = false;
            this.dgvCustomer.RowTemplate.Height = 80;
            this.dgvCustomer.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCustomer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCustomer.Size = new System.Drawing.Size(950, 393);
            this.dgvCustomer.TabIndex = 13;
            this.dgvCustomer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWaiverSet_CellContentClick);
            // 
            // customerName
            // 
            this.customerName.HeaderText = "customer Name";
            this.customerName.MinimumWidth = 350;
            this.customerName.Name = "customerName";
            this.customerName.ReadOnly = true;
            this.customerName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.customerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.customerName.Width = 400;
            // 
            // customerRelationType
            // 
            this.customerRelationType.HeaderText = "Relation";
            this.customerRelationType.MinimumWidth = 155;
            this.customerRelationType.Name = "customerRelationType";
            this.customerRelationType.ReadOnly = true;
            this.customerRelationType.Width = 210;
            // 
            // hasSignedWaiverSet
            // 
            this.hasSignedWaiverSet.HeaderText = "Signed?";
            this.hasSignedWaiverSet.MinimumWidth = 165;
            this.hasSignedWaiverSet.Name = "hasSignedWaiverSet";
            this.hasSignedWaiverSet.ReadOnly = true;
            this.hasSignedWaiverSet.Width = 165;
            // 
            // signFor
            // 
            this.signFor.HeaderText = "sign For";
            this.signFor.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            this.signFor.MinimumWidth = 175;
            this.signFor.Name = "signFor";
            this.signFor.ReadOnly = true;
            this.signFor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.signFor.Width = 175;
            // 
            // RelationshipTypeId
            // 
            this.RelationshipTypeId.HeaderText = "RelationshipTypeId";
            this.RelationshipTypeId.Name = "RelationshipTypeId";
            this.RelationshipTypeId.ReadOnly = true;
            this.RelationshipTypeId.Visible = false;
            this.RelationshipTypeId.Width = 50;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1031);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.lblSelection);
            this.panel1.Controls.Add(this.lblWaiverSignedStatus);
            this.panel1.Controls.Add(this.lblRelation);
            this.panel1.Controls.Add(this.lblCustomerName);
            this.panel1.Controls.Add(this.bigVerticalScrollCustomer);
            this.panel1.Controls.Add(this.dgvCustomer);
            this.panel1.Location = new System.Drawing.Point(400, 318);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1150, 507);
            this.panel1.TabIndex = 163;
            // 
            // lblSelection
            // 
            this.lblSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelection.AutoEllipsis = true;
            this.lblSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblSelection.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelection.ForeColor = System.Drawing.Color.Thistle;
            this.lblSelection.Location = new System.Drawing.Point(815, 4);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(250, 52);
            this.lblSelection.TabIndex = 164;
            this.lblSelection.Text = "Sign For?";
            this.lblSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWaiverSignedStatus
            // 
            this.lblWaiverSignedStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWaiverSignedStatus.AutoEllipsis = true;
            this.lblWaiverSignedStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiverSignedStatus.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverSignedStatus.ForeColor = System.Drawing.Color.Thistle;
            this.lblWaiverSignedStatus.Location = new System.Drawing.Point(615, 4);
            this.lblWaiverSignedStatus.Name = "lblWaiverSignedStatus";
            this.lblWaiverSignedStatus.Size = new System.Drawing.Size(278, 52);
            this.lblWaiverSignedStatus.TabIndex = 167;
            this.lblWaiverSignedStatus.Text = "Signed?";
            this.lblWaiverSignedStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRelation
            // 
            this.lblRelation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRelation.AutoEllipsis = true;
            this.lblRelation.BackColor = System.Drawing.Color.Transparent;
            this.lblRelation.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelation.ForeColor = System.Drawing.Color.Thistle;
            this.lblRelation.Location = new System.Drawing.Point(420, 4);
            this.lblRelation.Name = "lblRelation";
            this.lblRelation.Size = new System.Drawing.Size(256, 52);
            this.lblRelation.TabIndex = 166;
            this.lblRelation.Text = "Relation";
            this.lblRelation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerName.AutoEllipsis = true;
            this.lblCustomerName.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerName.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.ForeColor = System.Drawing.Color.Thistle;
            this.lblCustomerName.Location = new System.Drawing.Point(29, 4);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(362, 52);
            this.lblCustomerName.TabIndex = 163;
            this.lblCustomerName.Text = "Name";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bigVerticalScrollCustomer
            // 
            this.bigVerticalScrollCustomer.AutoHide = false;
            this.bigVerticalScrollCustomer.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollCustomer.DataGridView = this.dgvCustomer;
            this.bigVerticalScrollCustomer.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollCustomer.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollCustomer.Location = new System.Drawing.Point(1005, 69);
            this.bigVerticalScrollCustomer.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollCustomer.Name = "bigVerticalScrollCustomer";
            this.bigVerticalScrollCustomer.ScrollableControl = null;
            this.bigVerticalScrollCustomer.ScrollViewer = null;
            this.bigVerticalScrollCustomer.Size = new System.Drawing.Size(63, 393);
            this.bigVerticalScrollCustomer.TabIndex = 165;
            this.bigVerticalScrollCustomer.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCustomer.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollCustomer.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollCustomer.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            this.bigVerticalScrollCustomer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarGp_Scroll);
            // 
            // lblSignatoryCustomerName
            // 
            this.lblSignatoryCustomerName.BackColor = System.Drawing.Color.Transparent;
            this.lblSignatoryCustomerName.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignatoryCustomerName.ForeColor = System.Drawing.Color.White;
            this.lblSignatoryCustomerName.Location = new System.Drawing.Point(300, 138);
            this.lblSignatoryCustomerName.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblSignatoryCustomerName.Name = "lblSignatoryCustomerName";
            this.lblSignatoryCustomerName.Size = new System.Drawing.Size(1505, 57);
            this.lblSignatoryCustomerName.TabIndex = 20003;
            this.lblSignatoryCustomerName.Text = "Customer :";
            this.lblSignatoryCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddNewRelations
            // 
            this.btnAddNewRelations.BackColor = System.Drawing.Color.Transparent;
            this.btnAddNewRelations.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnAddNewRelations.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddNewRelations.FlatAppearance.BorderSize = 0;
            this.btnAddNewRelations.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewRelations.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewRelations.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewRelations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewRelations.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNewRelations.ForeColor = System.Drawing.Color.White;
            this.btnAddNewRelations.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddNewRelations.Location = new System.Drawing.Point(1580, 332);
            this.btnAddNewRelations.Name = "btnAddNewRelations";
            this.btnAddNewRelations.Size = new System.Drawing.Size(250, 125);
            this.btnAddNewRelations.TabIndex = 20004;
            this.btnAddNewRelations.Text = "Add New Relations";
            this.btnAddNewRelations.UseVisualStyleBackColor = false;
            this.btnAddNewRelations.Click += new System.EventHandler(this.btnAddNewRelations_Click);
            // 
            // btnProceed
            // 
            this.btnProceed.BackColor = System.Drawing.Color.Transparent;
            this.btnProceed.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnProceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(1000, 871);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(250, 125);
            this.btnProceed.TabIndex = 1028;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // lblWaiverSet
            // 
            this.lblWaiverSet.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiverSet.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverSet.ForeColor = System.Drawing.Color.White;
            this.lblWaiverSet.Location = new System.Drawing.Point(300, 211);
            this.lblWaiverSet.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblWaiverSet.Name = "lblWaiverSet";
            this.lblWaiverSet.Size = new System.Drawing.Size(1508, 50);
            this.lblWaiverSet.TabIndex = 20005;
            this.lblWaiverSet.Text = "Waiver Set :";
            this.lblWaiverSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmCustomerDetailsForWaiver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.lblWaiverSet);
            this.Controls.Add(this.btnAddNewRelations);
            this.Controls.Add(this.lblSignatoryCustomerName);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmCustomerDetailsForWaiver";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Details";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelectWaiverOption_Closing);
            this.Load += new System.EventHandler(this.frmSelectWaiverOption_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.Controls.SetChildIndex(this.lblSignatoryCustomerName, 0);
            this.Controls.SetChildIndex(this.btnAddNewRelations, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.lblWaiverSet, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCustomer;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSelection;
        private System.Windows.Forms.Label lblCustomerName;
        //private System.Windows.Forms.Button btnCancel;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCustomer;
        private Label lblSignatoryCustomerName;
        private Button btnAddNewRelations;
        private Label lblRelation;
        private Button btnProceed;
        private Label lblWaiverSignedStatus;
        private Label lblWaiverSet;
        private DataGridViewTextBoxColumn customerName;
        private DataGridViewTextBoxColumn customerRelationType;
        private DataGridViewImageColumn hasSignedWaiverSet;
        private DataGridViewImageColumn signFor;
        private DataGridViewTextBoxColumn RelationshipTypeId;
    }
}