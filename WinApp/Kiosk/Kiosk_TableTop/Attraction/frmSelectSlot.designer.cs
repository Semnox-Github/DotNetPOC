namespace Parafait_Kiosk
{
    partial class frmSelectSlot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectSlot));
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnProceed = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.pnlYourSelections = new System.Windows.Forms.Panel();
            this.lblYourSelectionsTime = new System.Windows.Forms.Label();
            this.lblYourSelectionsDate = new System.Windows.Forms.Label();
            this.lblYourSelectionsQty = new System.Windows.Forms.Label();
            this.lblYourSelectionsDateHeader = new System.Windows.Forms.Label();
            this.lblYourSelectionsTimeHeader = new System.Windows.Forms.Label();
            this.lblYourSelectionsQtyHeader = new System.Windows.Forms.Label();
            this.lblYourSelections = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.flpDates = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDate1 = new System.Windows.Forms.Button();
            this.btnDate2 = new System.Windows.Forms.Button();
            this.btnDate3 = new System.Windows.Forms.Button();
            this.btnCalendar = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblSelectSlot = new System.Windows.Forms.Label();
            this.bigVerticalScrollView = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.flpSlots = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlTimeSection = new System.Windows.Forms.Panel();
            this.lblNoSchedules = new System.Windows.Forms.Label();
            this.lblBookingSlot = new System.Windows.Forms.Label();
            this.pnlDateSection = new System.Windows.Forms.Panel();
            this.pnlYourSelections.SuspendLayout();
            this.flpDates.SuspendLayout();
            this.pnlTimeSection.SuspendLayout();
            this.pnlDateSection.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.TabIndex = 20013;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(465, 882);
            this.btnPrev.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(834, 882);
            // 
            // txtMessage
            // 
            this.txtMessage.AutoEllipsis = true;
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1011);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 147;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
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
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(1203, 882);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(250, 125);
            this.btnProceed.TabIndex = 1078;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(333, 880);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1261, 131);
            this.panelButtons.TabIndex = 20005;
            // 
            // pnlYourSelections
            // 
            this.pnlYourSelections.BackColor = System.Drawing.Color.Transparent;
            this.pnlYourSelections.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlYourSelections.BackgroundImage")));
            this.pnlYourSelections.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlYourSelections.Controls.Add(this.lblYourSelectionsTime);
            this.pnlYourSelections.Controls.Add(this.lblYourSelectionsDate);
            this.pnlYourSelections.Controls.Add(this.lblYourSelectionsQty);
            this.pnlYourSelections.Controls.Add(this.lblYourSelectionsDateHeader);
            this.pnlYourSelections.Controls.Add(this.lblYourSelectionsTimeHeader);
            this.pnlYourSelections.Controls.Add(this.lblYourSelectionsQtyHeader);
            this.pnlYourSelections.Controls.Add(this.lblYourSelections);
            this.pnlYourSelections.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlYourSelections.Location = new System.Drawing.Point(465, 743);
            this.pnlYourSelections.Name = "pnlYourSelections";
            this.pnlYourSelections.Size = new System.Drawing.Size(988, 130);
            this.pnlYourSelections.TabIndex = 20027;
            // 
            // lblYourSelectionsTime
            // 
            this.lblYourSelectionsTime.BackColor = System.Drawing.Color.Transparent;
            this.lblYourSelectionsTime.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourSelectionsTime.ForeColor = System.Drawing.Color.Black;
            this.lblYourSelectionsTime.Location = new System.Drawing.Point(617, 80);
            this.lblYourSelectionsTime.Name = "lblYourSelectionsTime";
            this.lblYourSelectionsTime.Size = new System.Drawing.Size(280, 53);
            this.lblYourSelectionsTime.TabIndex = 15;
            this.lblYourSelectionsTime.Text = "--";
            this.lblYourSelectionsTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblYourSelectionsDate
            // 
            this.lblYourSelectionsDate.BackColor = System.Drawing.Color.Transparent;
            this.lblYourSelectionsDate.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourSelectionsDate.ForeColor = System.Drawing.Color.Black;
            this.lblYourSelectionsDate.Location = new System.Drawing.Point(353, 80);
            this.lblYourSelectionsDate.Name = "lblYourSelectionsDate";
            this.lblYourSelectionsDate.Size = new System.Drawing.Size(280, 53);
            this.lblYourSelectionsDate.TabIndex = 14;
            this.lblYourSelectionsDate.Text = "--";
            this.lblYourSelectionsDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblYourSelectionsQty
            // 
            this.lblYourSelectionsQty.BackColor = System.Drawing.Color.Transparent;
            this.lblYourSelectionsQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourSelectionsQty.ForeColor = System.Drawing.Color.Black;
            this.lblYourSelectionsQty.Location = new System.Drawing.Point(129, 80);
            this.lblYourSelectionsQty.Name = "lblYourSelectionsQty";
            this.lblYourSelectionsQty.Size = new System.Drawing.Size(154, 53);
            this.lblYourSelectionsQty.TabIndex = 13;
            this.lblYourSelectionsQty.Text = "1";
            this.lblYourSelectionsQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblYourSelectionsDateHeader
            // 
            this.lblYourSelectionsDateHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblYourSelectionsDateHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourSelectionsDateHeader.ForeColor = System.Drawing.Color.Black;
            this.lblYourSelectionsDateHeader.Location = new System.Drawing.Point(448, 40);
            this.lblYourSelectionsDateHeader.Name = "lblYourSelectionsDateHeader";
            this.lblYourSelectionsDateHeader.Size = new System.Drawing.Size(193, 53);
            this.lblYourSelectionsDateHeader.TabIndex = 12;
            this.lblYourSelectionsDateHeader.Text = "Date";
            this.lblYourSelectionsDateHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblYourSelectionsTimeHeader
            // 
            this.lblYourSelectionsTimeHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblYourSelectionsTimeHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourSelectionsTimeHeader.ForeColor = System.Drawing.Color.Black;
            this.lblYourSelectionsTimeHeader.Location = new System.Drawing.Point(712, 40);
            this.lblYourSelectionsTimeHeader.Name = "lblYourSelectionsTimeHeader";
            this.lblYourSelectionsTimeHeader.Size = new System.Drawing.Size(193, 53);
            this.lblYourSelectionsTimeHeader.TabIndex = 11;
            this.lblYourSelectionsTimeHeader.Text = "Time";
            this.lblYourSelectionsTimeHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblYourSelectionsQtyHeader
            // 
            this.lblYourSelectionsQtyHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblYourSelectionsQtyHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourSelectionsQtyHeader.ForeColor = System.Drawing.Color.Black;
            this.lblYourSelectionsQtyHeader.Location = new System.Drawing.Point(144, 40);
            this.lblYourSelectionsQtyHeader.Name = "lblYourSelectionsQtyHeader";
            this.lblYourSelectionsQtyHeader.Size = new System.Drawing.Size(193, 53);
            this.lblYourSelectionsQtyHeader.TabIndex = 10;
            this.lblYourSelectionsQtyHeader.Text = "Quantity";
            this.lblYourSelectionsQtyHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblYourSelections
            // 
            this.lblYourSelections.BackColor = System.Drawing.Color.Transparent;
            this.lblYourSelections.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourSelections.ForeColor = System.Drawing.Color.Black;
            this.lblYourSelections.Location = new System.Drawing.Point(303, 0);
            this.lblYourSelections.Name = "lblYourSelections";
            this.lblYourSelections.Size = new System.Drawing.Size(382, 53);
            this.lblYourSelections.TabIndex = 9;
            this.lblYourSelections.Text = "Your Selections...";
            this.lblYourSelections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductName
            // 
            this.lblProductName.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblProductName.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.ForeColor = System.Drawing.Color.White;
            this.lblProductName.Location = new System.Drawing.Point(436, 55);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(1051, 65);
            this.lblProductName.TabIndex = 132;
            this.lblProductName.Text = "&1";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpDates
            // 
            this.flpDates.BackColor = System.Drawing.Color.Transparent;
            this.flpDates.Controls.Add(this.btnDate1);
            this.flpDates.Controls.Add(this.btnDate2);
            this.flpDates.Controls.Add(this.btnDate3);
            this.flpDates.Controls.Add(this.btnCalendar);
            this.flpDates.Location = new System.Drawing.Point(0, 70);
            this.flpDates.Name = "flpDates";
            this.flpDates.Size = new System.Drawing.Size(146, 510);
            this.flpDates.TabIndex = 20020;
            // 
            // btnDate1
            // 
            this.btnDate1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.BigCircleUnSelected;
            this.btnDate1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDate1.FlatAppearance.BorderSize = 0;
            this.btnDate1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDate1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDate1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDate1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDate1.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDate1.ForeColor = System.Drawing.Color.Black;
            this.btnDate1.Location = new System.Drawing.Point(10, 7);
            this.btnDate1.Margin = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.btnDate1.Name = "btnDate1";
            this.btnDate1.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnDate1.Size = new System.Drawing.Size(126, 113);
            this.btnDate1.TabIndex = 2;
            this.btnDate1.Text = "Nov 16";
            this.btnDate1.UseVisualStyleBackColor = true;
            this.btnDate1.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnDate2
            // 
            this.btnDate2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.BigCircleUnSelected;
            this.btnDate2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDate2.FlatAppearance.BorderSize = 0;
            this.btnDate2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDate2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDate2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDate2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDate2.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDate2.ForeColor = System.Drawing.Color.Black;
            this.btnDate2.Location = new System.Drawing.Point(10, 134);
            this.btnDate2.Margin = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.btnDate2.Name = "btnDate2";
            this.btnDate2.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnDate2.Size = new System.Drawing.Size(126, 113);
            this.btnDate2.TabIndex = 2;
            this.btnDate2.Text = "Nov 17";
            this.btnDate2.UseVisualStyleBackColor = true;
            this.btnDate2.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnDate3
            // 
            this.btnDate3.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.BigCircleUnSelected;
            this.btnDate3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDate3.FlatAppearance.BorderSize = 0;
            this.btnDate3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDate3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDate3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDate3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDate3.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDate3.ForeColor = System.Drawing.Color.Black;
            this.btnDate3.Location = new System.Drawing.Point(10, 261);
            this.btnDate3.Margin = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.btnDate3.Name = "btnDate3";
            this.btnDate3.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnDate3.Size = new System.Drawing.Size(126, 113);
            this.btnDate3.TabIndex = 2;
            this.btnDate3.Text = "Nov 18";
            this.btnDate3.UseVisualStyleBackColor = true;
            this.btnDate3.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnCalendar
            // 
            this.btnCalendar.BackColor = System.Drawing.Color.Transparent;
            this.btnCalendar.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.pickDate;
            this.btnCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCalendar.FlatAppearance.BorderSize = 0;
            this.btnCalendar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCalendar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCalendar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendar.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalendar.ForeColor = System.Drawing.Color.White;
            this.btnCalendar.Location = new System.Drawing.Point(10, 388);
            this.btnCalendar.Margin = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(126, 113);
            this.btnCalendar.TabIndex = 2;
            this.btnCalendar.UseVisualStyleBackColor = false;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendarIcon_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoEllipsis = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(0, 5);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(146, 62);
            this.lblDate.TabIndex = 20021;
            this.lblDate.Text = "Date";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSelectSlot
            // 
            this.lblSelectSlot.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectSlot.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectSlot.ForeColor = System.Drawing.Color.White;
            this.lblSelectSlot.Location = new System.Drawing.Point(39, 5);
            this.lblSelectSlot.Name = "lblSelectSlot";
            this.lblSelectSlot.Size = new System.Drawing.Size(794, 62);
            this.lblSelectSlot.TabIndex = 20022;
            this.lblSelectSlot.Text = "Select a slot";
            this.lblSelectSlot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bigVerticalScrollView
            // 
            this.bigVerticalScrollView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollView.AutoHide = false;
            this.bigVerticalScrollView.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollView.DataGridView = null;
            this.bigVerticalScrollView.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollView.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollView.Location = new System.Drawing.Point(1373, 190);
            this.bigVerticalScrollView.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollView.Name = "bigVerticalScrollView";
            this.bigVerticalScrollView.ScrollableControl = this.flpSlots;
            this.bigVerticalScrollView.ScrollViewer = null;
            this.bigVerticalScrollView.Size = new System.Drawing.Size(60, 495);
            this.bigVerticalScrollView.TabIndex = 20019;
            this.bigVerticalScrollView.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollView.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollView.UpButtonClick += new System.EventHandler(this.bigVerticalScrollView_ButtonClick);
            this.bigVerticalScrollView.DownButtonClick += new System.EventHandler(this.bigVerticalScrollView_ButtonClick);
            // 
            // flpSlots
            // 
            this.flpSlots.AutoScroll = true;
            this.flpSlots.BackColor = System.Drawing.Color.Transparent;
            this.flpSlots.Location = new System.Drawing.Point(30, 70);
            this.flpSlots.Name = "flpSlots";
            this.flpSlots.Size = new System.Drawing.Size(754, 510);
            this.flpSlots.TabIndex = 20006;
            // 
            // pnlTimeSection
            // 
            this.pnlTimeSection.BackColor = System.Drawing.Color.Transparent;
            this.pnlTimeSection.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.PanelSelectTimeSection;
            this.pnlTimeSection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTimeSection.Controls.Add(this.lblNoSchedules);
            this.pnlTimeSection.Controls.Add(this.lblSelectSlot);
            this.pnlTimeSection.Controls.Add(this.flpSlots);
            this.pnlTimeSection.Location = new System.Drawing.Point(618, 121);
            this.pnlTimeSection.Name = "pnlTimeSection";
            this.pnlTimeSection.Size = new System.Drawing.Size(836, 610);
            this.pnlTimeSection.TabIndex = 20025;
            // 
            // lblNoSchedules
            // 
            this.lblNoSchedules.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoSchedules.ForeColor = System.Drawing.Color.White;
            this.lblNoSchedules.Location = new System.Drawing.Point(32, 128);
            this.lblNoSchedules.Name = "lblNoSchedules";
            this.lblNoSchedules.Size = new System.Drawing.Size(754, 400);
            this.lblNoSchedules.TabIndex = 0;
            this.lblNoSchedules.Text = "No schedules available for the day now. Please select another date";
            this.lblNoSchedules.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBookingSlot
            // 
            this.lblBookingSlot.BackColor = System.Drawing.Color.Transparent;
            this.lblBookingSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblBookingSlot.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookingSlot.ForeColor = System.Drawing.Color.White;
            this.lblBookingSlot.Location = new System.Drawing.Point(436, 11);
            this.lblBookingSlot.Name = "lblBookingSlot";
            this.lblBookingSlot.Size = new System.Drawing.Size(1051, 46);
            this.lblBookingSlot.TabIndex = 20024;
            this.lblBookingSlot.Text = "Booking slot for &1 of &2";
            this.lblBookingSlot.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlDateSection
            // 
            this.pnlDateSection.BackColor = System.Drawing.Color.Transparent;
            this.pnlDateSection.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.DatePanelBackground;
            this.pnlDateSection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDateSection.Controls.Add(this.lblDate);
            this.pnlDateSection.Controls.Add(this.flpDates);
            this.pnlDateSection.Location = new System.Drawing.Point(465, 121);
            this.pnlDateSection.Name = "pnlDateSection";
            this.pnlDateSection.Size = new System.Drawing.Size(146, 610);
            this.pnlDateSection.TabIndex = 20025;
            // 
            // frmSelectSlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1061);
            this.Controls.Add(this.bigVerticalScrollView);
            this.Controls.Add(this.lblBookingSlot);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.pnlDateSection);
            this.Controls.Add(this.pnlTimeSection);
            this.Controls.Add(this.pnlYourSelections);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.txtMessage);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmSelectSlot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSelectSlot_FormClosed);
            this.Load += new System.EventHandler(this.frmSelectSlot_Load);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.lblProductName, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.pnlYourSelections, 0);
            this.Controls.SetChildIndex(this.pnlTimeSection, 0);
            this.Controls.SetChildIndex(this.pnlDateSection, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.Controls.SetChildIndex(this.lblBookingSlot, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollView, 0);
            this.pnlYourSelections.ResumeLayout(false);
            this.flpDates.ResumeLayout(false);
            this.pnlTimeSection.ResumeLayout(false);
            this.pnlDateSection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Panel panelButtons;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollView;
        private System.Windows.Forms.Panel pnlYourSelections;
        private System.Windows.Forms.Label lblYourSelectionsTime;
        private System.Windows.Forms.Label lblYourSelectionsDate;
        private System.Windows.Forms.Label lblYourSelectionsQty;
        private System.Windows.Forms.Label lblYourSelectionsDateHeader;
        private System.Windows.Forms.Label lblYourSelectionsTimeHeader;
        private System.Windows.Forms.Label lblYourSelectionsQtyHeader;
        private System.Windows.Forms.Label lblYourSelections;
        internal System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.FlowLayoutPanel flpDates;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblSelectSlot;
        private System.Windows.Forms.Button btnDate1;
        private System.Windows.Forms.Button btnDate2;
        private System.Windows.Forms.Button btnDate3;
        private System.Windows.Forms.Button btnCalendar;
        internal System.Windows.Forms.Label lblBookingSlot;
        private System.Windows.Forms.Panel pnlDateSection;
        private System.Windows.Forms.Panel pnlTimeSection;
        private System.Windows.Forms.FlowLayoutPanel flpSlots;
        private System.Windows.Forms.Label lblNoSchedules;
    }
}