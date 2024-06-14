using System;
using System.Drawing;

namespace Parafait_POS.Attraction
{
    partial class AttractionSchedule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttractionSchedule));
            this.flpProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpAttractionDate = new System.Windows.Forms.DateTimePicker();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.chkShowPast = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAttractionFacility = new System.Windows.Forms.ComboBox();
            this.flpSummary = new System.Windows.Forms.FlowLayoutPanel();
            this.flpSchedules = new System.Windows.Controls.WrapPanel();
            this.ehSchedules = new System.Windows.Forms.Integration.ElementHost();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.flpScheduleHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.vsbSchedule = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.flpScreenHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHeaderAttractionProduct = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.flpLegends = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLegend = new System.Windows.Forms.Label();
            this.lblStatusBlocked = new System.Windows.Forms.Label();
            this.lblStatusUnavailable = new System.Windows.Forms.Label();
            this.lblStatusSoldOut = new System.Windows.Forms.Label();
            this.lblStatusInUse = new System.Windows.Forms.Label();
            this.lblStatusPartyReservation = new System.Windows.Forms.Label();
            this.lblStatusRescheduleInProgress = new System.Windows.Forms.Label();
            this.flpReschedule = new System.Windows.Forms.FlowLayoutPanel();
            this.flpReschduleInProgress = new System.Windows.Forms.FlowLayoutPanel();
            this.lblReschduleInProgress = new System.Windows.Forms.Label();
            this.picWorkInProgress = new System.Windows.Forms.PictureBox();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picWorkInProgress)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.flpScreenHeader.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.flpLegends.SuspendLayout();
            this.flpReschduleInProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpProducts
            // 
            this.flpProducts.AutoScroll = true;
            this.flpProducts.BackColor = System.Drawing.Color.White;
            this.flpProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpProducts.Location = new System.Drawing.Point(0, 0);
            this.flpProducts.Margin = new System.Windows.Forms.Padding(0);
            this.flpProducts.Name = "flpProducts";
            this.flpProducts.Size = new System.Drawing.Size(209, 529);
            this.flpProducts.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "For Date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpAttractionDate
            // 
            this.dtpAttractionDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpAttractionDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpAttractionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttractionDate.Location = new System.Drawing.Point(340, 7);
            this.dtpAttractionDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.dtpAttractionDate.Name = "dtpAttractionDate";
            this.dtpAttractionDate.Size = new System.Drawing.Size(142, 21);
            this.dtpAttractionDate.TabIndex = 1;
            this.dtpAttractionDate.ValueChanged += new System.EventHandler(this.dtpAttractionDate_ValueChanged);
            // 
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Backward_Btn;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(488, 2);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(3, 2, 1, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 30);
            this.btnPrev.TabIndex = 7;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseUp);
            // 
            // btnNext
            // 
            this.btnNext.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Forward_Btn;
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(522, 2);
            this.btnNext.Margin = new System.Windows.Forms.Padding(1, 2, 3, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 30);
            this.btnNext.TabIndex = 8;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            this.btnNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNext_MouseDown);
            this.btnNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNext_MouseUp);
            // 
            // chkShowPast
            // 
            this.chkShowPast.AutoSize = true;
            this.chkShowPast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkShowPast.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowPast.Location = new System.Drawing.Point(567, 9);
            this.chkShowPast.Margin = new System.Windows.Forms.Padding(10, 9, 3, 3);
            this.chkShowPast.Name = "chkShowPast";
            this.chkShowPast.Size = new System.Drawing.Size(144, 19);
            this.chkShowPast.TabIndex = 3;
            this.chkShowPast.Text = "Show Past Schedules";
            this.chkShowPast.UseVisualStyleBackColor = true;
            this.chkShowPast.CheckedChanged += new System.EventHandler(this.chkShowPast_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(485, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Attraction:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbAttractionFacility
            // 
            this.cmbAttractionFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttractionFacility.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAttractionFacility.FormattingEnabled = true;
            this.cmbAttractionFacility.Location = new System.Drawing.Point(823, 7);
            this.cmbAttractionFacility.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.cmbAttractionFacility.Name = "cmbAttractionFacility";
            this.cmbAttractionFacility.Size = new System.Drawing.Size(149, 23);
            this.cmbAttractionFacility.TabIndex = 0;
            this.cmbAttractionFacility.SelectedIndexChanged += new System.EventHandler(this.cmbAttractionFacility_SelectedIndexChanged);
            // 
            // flpSummary
            // 
            this.flpSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSummary.AutoScroll = true;
            this.flpSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpSummary.Location = new System.Drawing.Point(0, 0);
            this.flpSummary.Margin = new System.Windows.Forms.Padding(0);
            this.flpSummary.Name = "flpSummary";
            this.flpSummary.Size = new System.Drawing.Size(225, 291);
            this.flpSummary.TabIndex = 2;
            this.flpSummary.WrapContents = false;
            // 
            // flpSchedules
            // 
            //this.flpSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            ////this.flpSchedules.AutoScroll = true;
            //this.flpSchedules.BackColor = System.Drawing.Color.White;
            ////this.flpSchedules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //this.flpSchedules.Location = new System.Drawing.Point(212, 86);
            //this.flpSchedules.Margin = new System.Windows.Forms.Padding(0);
            //this.flpSchedules.Name = "flpSchedules";
            //this.flpSchedules.Size = new System.Drawing.Size(885, 443);
            //this.flpSchedules.TabIndex = 3;
            this.flpSchedules.Background = System.Windows.Media.Brushes.White;
            //this.flpSchedules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpSchedules.Name = "flpSchedules";
            this.flpSchedules.Height = 443;
            this.flpSchedules.Width = 885;
            this.flpSchedules.Orientation = System.Windows.Controls.Orientation.Vertical;
            // 
            // flpSchedules
            // 
            this.ehSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            //this.ehSchedules.AutoScroll = true;
            this.ehSchedules.BackColor = System.Drawing.Color.White;
            //this.ehSchedules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ehSchedules.Location = new System.Drawing.Point(212, 86);
            this.ehSchedules.Margin = new System.Windows.Forms.Padding(0);
            this.ehSchedules.Name = "ehSchedules";
            this.ehSchedules.Size = new System.Drawing.Size(885, 443);
            this.ehSchedules.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(0, 0);
            this.btnOk.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 45);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Done";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnOk.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOk_MouseDown);
            this.btnOk.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnOk_MouseUp);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(110, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 45);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseUp);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnOk);
            this.panelButtons.Location = new System.Drawing.Point(6, 467);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(225, 45);
            this.panelButtons.TabIndex = 4;
            // 
            // flpScheduleHeader
            // 
            this.flpScheduleHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpScheduleHeader.BackColor = System.Drawing.Color.Transparent;
            this.flpScheduleHeader.Location = new System.Drawing.Point(212, 38);
            this.flpScheduleHeader.Margin = new System.Windows.Forms.Padding(0);
            this.flpScheduleHeader.Name = "flpScheduleHeader";
            this.flpScheduleHeader.Size = new System.Drawing.Size(885, 48);
            this.flpScheduleHeader.TabIndex = 9;
            // 
            // vsbSchedule
            // 
            this.vsbSchedule.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.vsbSchedule.AutoHide = false;
            this.vsbSchedule.DataGridView = null;
            this.vsbSchedule.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vsbSchedule.DownButtonBackgroundImage")));
            this.vsbSchedule.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vsbSchedule.DownButtonDisabledBackgroundImage")));
            this.vsbSchedule.Location = new System.Drawing.Point(1099, 49);
            this.vsbSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.vsbSchedule.Name = "vsbSchedule";
            this.vsbSchedule.ScrollableControl = null;
            this.vsbSchedule.ScrollViewer = null;
            this.vsbSchedule.Size = new System.Drawing.Size(40, 480);
            this.vsbSchedule.TabIndex = 5;
            this.vsbSchedule.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vsbSchedule.UpButtonBackgroundImage")));
            this.vsbSchedule.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vsbSchedule.UpButtonDisabledBackgroundImage")));
            // 
            // flpScreenHeader
            // 
            this.flpScreenHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpScreenHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpScreenHeader.Controls.Add(this.lblHeaderAttractionProduct);
            this.flpScreenHeader.Controls.Add(this.label3);
            this.flpScreenHeader.Controls.Add(this.dtpAttractionDate);
            this.flpScreenHeader.Controls.Add(this.btnPrev);
            this.flpScreenHeader.Controls.Add(this.btnNext);
            this.flpScreenHeader.Controls.Add(this.chkShowPast);
            this.flpScreenHeader.Controls.Add(this.label4);
            this.flpScreenHeader.Controls.Add(this.cmbAttractionFacility);
            this.flpScreenHeader.Location = new System.Drawing.Point(212, 0);
            this.flpScreenHeader.Name = "flpScreenHeader";
            this.flpScreenHeader.Size = new System.Drawing.Size(1155, 38);
            this.flpScreenHeader.TabIndex = 1;
            // 
            // lblHeaderAttractionProduct
            // 
            this.lblHeaderAttractionProduct.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderAttractionProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblHeaderAttractionProduct.Location = new System.Drawing.Point(3, 5);
            this.lblHeaderAttractionProduct.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblHeaderAttractionProduct.Name = "lblHeaderAttractionProduct";
            this.lblHeaderAttractionProduct.Size = new System.Drawing.Size(250, 25);
            this.lblHeaderAttractionProduct.TabIndex = 10;
            this.lblHeaderAttractionProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(259, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "For Date:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(717, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Facility Map:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelRight
            // 
            this.panelRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panelRight.Controls.Add(this.flpLegends);
            this.panelRight.Controls.Add(this.flpSummary);
            this.panelRight.Controls.Add(this.panelButtons);
            this.panelRight.Location = new System.Drawing.Point(1142, 49);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(225, 515);
            this.panelRight.TabIndex = 10;
            // 
            // flpLegends
            // 
            this.flpLegends.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flpLegends.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpLegends.Controls.Add(this.lblLegend);
            this.flpLegends.Controls.Add(this.lblStatusBlocked);
            this.flpLegends.Controls.Add(this.lblStatusUnavailable);
            this.flpLegends.Controls.Add(this.lblStatusSoldOut);
            this.flpLegends.Controls.Add(this.lblStatusInUse);
            this.flpLegends.Controls.Add(this.lblStatusPartyReservation);
            this.flpLegends.Controls.Add(this.lblStatusRescheduleInProgress);
            this.flpLegends.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpLegends.Location = new System.Drawing.Point(0, 291);
            this.flpLegends.Margin = new System.Windows.Forms.Padding(0);
            this.flpLegends.Name = "flpLegends";
            this.flpLegends.Size = new System.Drawing.Size(225, 172);
            this.flpLegends.TabIndex = 3;
            this.flpLegends.WrapContents = false;
            // 
            // lblLegend
            // 
            this.lblLegend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLegend.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLegend.Location = new System.Drawing.Point(0, 0);
            this.lblLegend.Margin = new System.Windows.Forms.Padding(0);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(225, 25);
            this.lblLegend.TabIndex = 0;
            this.lblLegend.Text = "Legend";
            this.lblLegend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusBlocked
            // 
            this.lblStatusBlocked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusBlocked.BackColor = System.Drawing.Color.DarkGray;
            this.lblStatusBlocked.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusBlocked.Location = new System.Drawing.Point(0, 25);
            this.lblStatusBlocked.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatusBlocked.Name = "lblStatusBlocked";
            this.lblStatusBlocked.Size = new System.Drawing.Size(225, 20);
            this.lblStatusBlocked.TabIndex = 1;
            this.lblStatusBlocked.Text = "Blocked";
            this.lblStatusBlocked.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusUnavailable
            // 
            this.lblStatusUnavailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusUnavailable.BackColor = System.Drawing.Color.DarkRed;
            this.lblStatusUnavailable.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusUnavailable.Location = new System.Drawing.Point(0, 45);
            this.lblStatusUnavailable.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatusUnavailable.Name = "lblStatusUnavailable";
            this.lblStatusUnavailable.Size = new System.Drawing.Size(225, 20);
            this.lblStatusUnavailable.TabIndex = 2;
            this.lblStatusUnavailable.Text = "Not Available";
            this.lblStatusUnavailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusSoldOut
            // 
            this.lblStatusSoldOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusSoldOut.BackColor = System.Drawing.Color.Gainsboro;
            this.lblStatusSoldOut.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusSoldOut.Location = new System.Drawing.Point(0, 65);
            this.lblStatusSoldOut.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatusSoldOut.Name = "lblStatusSoldOut";
            this.lblStatusSoldOut.Size = new System.Drawing.Size(225, 20);
            this.lblStatusSoldOut.TabIndex = 3;
            this.lblStatusSoldOut.Text = "Sold Out";
            this.lblStatusSoldOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusInUse
            // 
            this.lblStatusInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusInUse.BackColor = System.Drawing.Color.Chocolate;
            this.lblStatusInUse.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusInUse.Location = new System.Drawing.Point(0, 85);
            this.lblStatusInUse.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatusInUse.Name = "lblStatusInUse";
            this.lblStatusInUse.Size = new System.Drawing.Size(225, 20);
            this.lblStatusInUse.TabIndex = 4;
            this.lblStatusInUse.Text = "Race in progress";
            this.lblStatusInUse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusPartyReservation
            // 
            this.lblStatusPartyReservation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusPartyReservation.BackColor = System.Drawing.Color.Gold;
            this.lblStatusPartyReservation.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusPartyReservation.Location = new System.Drawing.Point(0, 105);
            this.lblStatusPartyReservation.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatusPartyReservation.Name = "lblStatusPartyReservation";
            this.lblStatusPartyReservation.Size = new System.Drawing.Size(225, 20);
            this.lblStatusPartyReservation.TabIndex = 5;
            this.lblStatusPartyReservation.Text = "Party Reservation";
            this.lblStatusPartyReservation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusRescheduleInProgress
            // 
            this.lblStatusRescheduleInProgress.BackColor = System.Drawing.Color.PaleGreen;
            this.lblStatusRescheduleInProgress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusRescheduleInProgress.Location = new System.Drawing.Point(0, 125);
            this.lblStatusRescheduleInProgress.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatusRescheduleInProgress.Name = "lblStatusRescheduleInProgress";
            this.lblStatusRescheduleInProgress.Size = new System.Drawing.Size(225, 20);
            this.lblStatusRescheduleInProgress.TabIndex = 6;
            this.lblStatusRescheduleInProgress.Text = "Reschedule in progress";
            this.lblStatusRescheduleInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpReschedule
            // 
            this.flpReschedule.BackColor = System.Drawing.Color.White;
            this.flpReschedule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpReschedule.Location = new System.Drawing.Point(0, 0);
            this.flpReschedule.Margin = new System.Windows.Forms.Padding(0);
            this.flpReschedule.Name = "flpReschedule";
            this.flpReschedule.Size = new System.Drawing.Size(400, 350);
            this.flpReschedule.TabIndex = 10;
            this.flpReschedule.Visible = false;
            // 
            // flpReschduleInProgress
            // 
            this.flpReschduleInProgress.BackColor = System.Drawing.Color.White;
            this.flpReschduleInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpReschduleInProgress.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpReschduleInProgress.WrapContents = false;
            this.flpReschduleInProgress.Controls.Add(this.picWorkInProgress);
            this.flpReschduleInProgress.Controls.Add(this.lblReschduleInProgress);
            this.flpReschduleInProgress.Location = new System.Drawing.Point(0, 0);
            this.flpReschduleInProgress.Margin = new System.Windows.Forms.Padding(0);
            this.flpReschduleInProgress.Name = "flpReschduleInProgress";
            this.flpReschduleInProgress.Size = new System.Drawing.Size(400, 350);
            this.flpReschduleInProgress.TabIndex = 10;
            this.flpReschduleInProgress.Visible = false;
            // 
            // lblReschduleInProgress
            // 
            this.lblReschduleInProgress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReschduleInProgress.Location = new System.Drawing.Point(0, 0);
            this.lblReschduleInProgress.Margin = new System.Windows.Forms.Padding(0,0,0,1);
            this.lblReschduleInProgress.Name = "lblReschduleInProgress";
            this.lblReschduleInProgress.Size = new System.Drawing.Size(350, 50);
            this.lblReschduleInProgress.TabIndex = 2;
            this.lblReschduleInProgress.Text = "Reschduling in progress. Please wait.";
            this.lblReschduleInProgress.ForeColor = System.Drawing.Color.Black;
            this.lblReschduleInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picWorkInProgress
            // 
            this.picWorkInProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left))));
            this.picWorkInProgress.Margin = new System.Windows.Forms.Padding(135,85,0,85);
            this.picWorkInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picWorkInProgress.Name = "picWorkInProgress";
            this.picWorkInProgress.Size = new System.Drawing.Size(130, 130);
            this.picWorkInProgress.TabIndex = 0;
            this.picWorkInProgress.Image = Properties.Resources.Processing_Icon;
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblErrorMessage.BackColor = System.Drawing.Color.FloralWhite;
            this.lblErrorMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorMessage.ForeColor = System.Drawing.Color.DarkRed;
            this.lblErrorMessage.Location = new System.Drawing.Point(4, 534);
            this.lblErrorMessage.Margin = new System.Windows.Forms.Padding(0);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(1000, 25);
            this.lblErrorMessage.TabIndex = 0;
            this.lblErrorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AttractionSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 564);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.flpScheduleHeader);
            this.Controls.Add(this.vsbSchedule);
            this.Controls.Add(this.ehSchedules);
            this.Controls.Add(this.flpScreenHeader);
            this.Controls.Add(this.flpProducts);
            this.Controls.Add(this.flpReschedule);
            this.Controls.Add(this.flpReschduleInProgress);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttractionSchedule";
            this.Text = "Schedule Attractions";
            this.Load += new System.EventHandler(this.AttractionScheduleNew_Load);
            this.Resize += new System.EventHandler(this.AttractionScheduleNew_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picWorkInProgress)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.flpScreenHeader.ResumeLayout(false);
            this.flpScreenHeader.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.flpLegends.ResumeLayout(false);
            this.flpReschduleInProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Controls.WrapPanel flpSchedules;
        private System.Windows.Forms.Integration.ElementHost ehSchedules;
        private System.Windows.Forms.FlowLayoutPanel flpReschedule;
        private System.Windows.Forms.FlowLayoutPanel flpReschduleInProgress;
        private System.Windows.Forms.Label lblReschduleInProgress;
        private System.Windows.Forms.PictureBox picWorkInProgress;
        private System.Windows.Forms.ComboBox cmbAttractionFacility;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpAttractionDate;
        private System.Windows.Forms.CheckBox chkShowPast;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelButtons;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vsbSchedule;
        private System.Windows.Forms.FlowLayoutPanel flpScheduleHeader;
        private System.Windows.Forms.FlowLayoutPanel flpScreenHeader;
        private System.Windows.Forms.Label lblHeaderAttractionProduct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblErrorMessage;
        private System.Windows.Forms.FlowLayoutPanel flpLegends;
        private System.Windows.Forms.Label lblLegend;
        private System.Windows.Forms.Label lblStatusBlocked;
        private System.Windows.Forms.Label lblStatusUnavailable;
        private System.Windows.Forms.Label lblStatusSoldOut;
        private System.Windows.Forms.Label lblStatusInUse;
        private System.Windows.Forms.Label lblStatusPartyReservation;
        private System.Windows.Forms.Label lblStatusRescheduleInProgress;
    }
}