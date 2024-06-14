namespace Parafait_Kiosk
{
    partial class frmKioskPrintSummary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKioskPrintSummary));
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblFromText = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.pBFromCalender = new System.Windows.Forms.PictureBox();
            this.pBToCalender = new System.Windows.Forms.PictureBox();
            this.lblFromDateTime = new System.Windows.Forms.Label();
            this.lblToDateTime = new System.Windows.Forms.Label();
            this.lblToText = new System.Windows.Forms.Label();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnRePrintTab = new System.Windows.Forms.Button();
            this.pBPrintSummaryImage = new System.Windows.Forms.PictureBox();
            this.cmbReprintDateRange = new System.Windows.Forms.ComboBox();
            this.btnNewTab = new System.Windows.Forms.Button();
            this.pnlFromToDates = new System.Windows.Forms.Panel();
            this.lblOR = new System.Windows.Forms.Label();
            this.pnlReprint = new System.Windows.Forms.Panel();
            this.lblPickText = new System.Windows.Forms.Label();
            this.flpTabs = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTabMsg = new System.Windows.Forms.Label();
            this.panelKioskSummaryReport = new System.Windows.Forms.Panel();
            this.flpTabBars = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pBFromCalender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBToCalender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBPrintSummaryImage)).BeginInit();
            this.pnlFromToDates.SuspendLayout();
            this.pnlReprint.SuspendLayout();
            this.panelKioskSummaryReport.SuspendLayout();
            this.SuspendLayout();
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
            this.btnPrev.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F);
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(124, 1035);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(375, 192);
            this.btnPrev.TabIndex = 152;
            this.btnPrev.Text = "Back";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblGreeting
            // 
            this.lblGreeting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(3, 14);
            this.lblGreeting.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(2848, 105);
            this.lblGreeting.TabIndex = 143;
            this.lblGreeting.Text = "Kiosk Print Summary";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1569);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(2880, 77);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // lblFromText
            // 
            this.lblFromText.BackColor = System.Drawing.Color.Transparent;
            this.lblFromText.CausesValidation = false;
            this.lblFromText.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.lblFromText.ForeColor = System.Drawing.Color.White;
            this.lblFromText.Location = new System.Drawing.Point(-2, 49);
            this.lblFromText.Margin = new System.Windows.Forms.Padding(0);
            this.lblFromText.Name = "lblFromText";
            this.lblFromText.Size = new System.Drawing.Size(318, 122);
            this.lblFromText.TabIndex = 0;
            this.lblFromText.Text = "From :";
            this.lblFromText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(554, 1035);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(105, 5, 4, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(375, 192);
            this.btnPrint.TabIndex = 20036;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pBFromCalender
            // 
            this.pBFromCalender.BackColor = System.Drawing.Color.Transparent;
            this.pBFromCalender.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pBFromCalender.Location = new System.Drawing.Point(1060, 48);
            this.pBFromCalender.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pBFromCalender.Name = "pBFromCalender";
            this.pBFromCalender.Size = new System.Drawing.Size(122, 134);
            this.pBFromCalender.TabIndex = 20039;
            this.pBFromCalender.TabStop = false;
            this.pBFromCalender.Click += new System.EventHandler(this.FromDateTime_Click);
            // 
            // pBToCalender
            // 
            this.pBToCalender.BackColor = System.Drawing.Color.Transparent;
            this.pBToCalender.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pBToCalender.Location = new System.Drawing.Point(1060, 268);
            this.pBToCalender.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pBToCalender.Name = "pBToCalender";
            this.pBToCalender.Size = new System.Drawing.Size(122, 123);
            this.pBToCalender.TabIndex = 20040;
            this.pBToCalender.TabStop = false;
            this.pBToCalender.Click += new System.EventHandler(this.ToDateTime_Click);
            // 
            // lblFromDateTime
            // 
            this.lblFromDateTime.BackColor = System.Drawing.Color.White;
            this.lblFromDateTime.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F);
            this.lblFromDateTime.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblFromDateTime.Location = new System.Drawing.Point(318, 48);
            this.lblFromDateTime.Margin = new System.Windows.Forms.Padding(0);
            this.lblFromDateTime.Name = "lblFromDateTime";
            this.lblFromDateTime.Size = new System.Drawing.Size(864, 123);
            this.lblFromDateTime.TabIndex = 20041;
            this.lblFromDateTime.Text = "01 Jan 1901, 6:00 AM";
            this.lblFromDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFromDateTime.Click += new System.EventHandler(this.FromDateTime_Click);
            // 
            // lblToDateTime
            // 
            this.lblToDateTime.BackColor = System.Drawing.Color.White;
            this.lblToDateTime.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F);
            this.lblToDateTime.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblToDateTime.Location = new System.Drawing.Point(318, 268);
            this.lblToDateTime.Margin = new System.Windows.Forms.Padding(0);
            this.lblToDateTime.Name = "lblToDateTime";
            this.lblToDateTime.Size = new System.Drawing.Size(864, 123);
            this.lblToDateTime.TabIndex = 20042;
            this.lblToDateTime.Text = "01 Jan 1901, 6:00 AM";
            this.lblToDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblToDateTime.Click += new System.EventHandler(this.ToDateTime_Click);
            // 
            // lblToText
            // 
            this.lblToText.BackColor = System.Drawing.Color.Transparent;
            this.lblToText.CausesValidation = false;
            this.lblToText.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.lblToText.ForeColor = System.Drawing.Color.White;
            this.lblToText.Location = new System.Drawing.Point(-2, 269);
            this.lblToText.Margin = new System.Windows.Forms.Padding(0);
            this.lblToText.Name = "lblToText";
            this.lblToText.Size = new System.Drawing.Size(318, 122);
            this.lblToText.TabIndex = 20043;
            this.lblToText.Text = "To :";
            this.lblToText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEmail
            // 
            this.btnEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnEmail.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmail.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnEmail.FlatAppearance.BorderSize = 0;
            this.btnEmail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEmail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmail.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F);
            this.btnEmail.ForeColor = System.Drawing.Color.White;
            this.btnEmail.Location = new System.Drawing.Point(982, 1035);
            this.btnEmail.Margin = new System.Windows.Forms.Padding(105, 5, 4, 5);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(375, 192);
            this.btnEmail.TabIndex = 20049;
            this.btnEmail.Text = "Email";
            this.btnEmail.UseVisualStyleBackColor = false;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // btnRePrintTab
            // 
            this.btnRePrintTab.AutoSize = true;
            this.btnRePrintTab.BackColor = System.Drawing.Color.Transparent;
            this.btnRePrintTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRePrintTab.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnRePrintTab.FlatAppearance.BorderSize = 0;
            this.btnRePrintTab.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRePrintTab.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRePrintTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRePrintTab.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Underline);
            this.btnRePrintTab.ForeColor = System.Drawing.Color.White;
            this.btnRePrintTab.Location = new System.Drawing.Point(1413, 134);
            this.btnRePrintTab.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnRePrintTab.Name = "btnRePrintTab";
            this.btnRePrintTab.Size = new System.Drawing.Size(570, 91);
            this.btnRePrintTab.TabIndex = 20037;
            this.btnRePrintTab.Text = "Reprint Report";
            this.btnRePrintTab.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnRePrintTab.UseVisualStyleBackColor = false;
            this.btnRePrintTab.Click += new System.EventHandler(this.btnRePrintTab_Click);
            // 
            // pBPrintSummaryImage
            // 
            this.pBPrintSummaryImage.BackColor = System.Drawing.Color.White;
            this.pBPrintSummaryImage.Location = new System.Drawing.Point(1485, 225);
            this.pBPrintSummaryImage.Margin = new System.Windows.Forms.Padding(1350, 0, 4, 5);
            this.pBPrintSummaryImage.Name = "pBPrintSummaryImage";
            this.pBPrintSummaryImage.Size = new System.Drawing.Size(1150, 1003);
            this.pBPrintSummaryImage.TabIndex = 20045;
            this.pBPrintSummaryImage.TabStop = false;
            // 
            // cmbReprintDateRange
            // 
            this.cmbReprintDateRange.BackColor = System.Drawing.Color.White;
            this.cmbReprintDateRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReprintDateRange.Font = new System.Drawing.Font("Gotham Rounded Bold", 19.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReprintDateRange.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbReprintDateRange.FormattingEnabled = true;
            this.cmbReprintDateRange.ItemHeight = 46;
            this.cmbReprintDateRange.Location = new System.Drawing.Point(318, 45);
            this.cmbReprintDateRange.Margin = new System.Windows.Forms.Padding(0, 18, 0, 18);
            this.cmbReprintDateRange.MaxDropDownItems = 10;
            this.cmbReprintDateRange.Name = "cmbReprintDateRange";
            this.cmbReprintDateRange.Size = new System.Drawing.Size(1142, 54);
            this.cmbReprintDateRange.TabIndex = 20048;
            this.cmbReprintDateRange.SelectedValueChanged += new System.EventHandler(this.cmbReprintDateRange_SelectedValueChanged);
            // 
            // btnNewTab
            // 
            this.btnNewTab.AutoSize = true;
            this.btnNewTab.BackColor = System.Drawing.Color.Transparent;
            this.btnNewTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNewTab.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnNewTab.FlatAppearance.BorderSize = 0;
            this.btnNewTab.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewTab.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewTab.Font = new System.Drawing.Font("Gotham Rounded Bold", 30.25F, System.Drawing.FontStyle.Underline);
            this.btnNewTab.ForeColor = System.Drawing.Color.White;
            this.btnNewTab.Location = new System.Drawing.Point(936, 134);
            this.btnNewTab.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnNewTab.Name = "btnNewTab";
            this.btnNewTab.Size = new System.Drawing.Size(492, 91);
            this.btnNewTab.TabIndex = 20049;
            this.btnNewTab.Text = "New Report";
            this.btnNewTab.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnNewTab.UseVisualStyleBackColor = false;
            this.btnNewTab.Click += new System.EventHandler(this.btnNewTab_Click);
            // 
            // pnlFromToDates
            // 
            this.pnlFromToDates.BackColor = System.Drawing.Color.Transparent;
            this.pnlFromToDates.Controls.Add(this.pBToCalender);
            this.pnlFromToDates.Controls.Add(this.lblToDateTime);
            this.pnlFromToDates.Controls.Add(this.lblFromText);
            this.pnlFromToDates.Controls.Add(this.pBFromCalender);
            this.pnlFromToDates.Controls.Add(this.lblFromDateTime);
            this.pnlFromToDates.Controls.Add(this.lblToText);
            this.pnlFromToDates.Location = new System.Drawing.Point(4, 440);
            this.pnlFromToDates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlFromToDates.Name = "pnlFromToDates";
            this.pnlFromToDates.Size = new System.Drawing.Size(1462, 485);
            this.pnlFromToDates.TabIndex = 20050;
            // 
            // lblOR
            // 
            this.lblOR.AutoSize = true;
            this.lblOR.BackColor = System.Drawing.Color.Transparent;
            this.lblOR.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.lblOR.ForeColor = System.Drawing.Color.White;
            this.lblOR.Location = new System.Drawing.Point(650, 137);
            this.lblOR.Margin = new System.Windows.Forms.Padding(0);
            this.lblOR.Name = "lblOR";
            this.lblOR.Size = new System.Drawing.Size(124, 72);
            this.lblOR.TabIndex = 20051;
            this.lblOR.Text = "OR";
            this.lblOR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlReprint
            // 
            this.pnlReprint.BackColor = System.Drawing.Color.Transparent;
            this.pnlReprint.Controls.Add(this.lblPickText);
            this.pnlReprint.Controls.Add(this.lblOR);
            this.pnlReprint.Controls.Add(this.cmbReprintDateRange);
            this.pnlReprint.Location = new System.Drawing.Point(4, 186);
            this.pnlReprint.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlReprint.Name = "pnlReprint";
            this.pnlReprint.Size = new System.Drawing.Size(1462, 245);
            this.pnlReprint.TabIndex = 20052;
            // 
            // lblPickText
            // 
            this.lblPickText.BackColor = System.Drawing.Color.Transparent;
            this.lblPickText.CausesValidation = false;
            this.lblPickText.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.lblPickText.ForeColor = System.Drawing.Color.White;
            this.lblPickText.Location = new System.Drawing.Point(-2, 15);
            this.lblPickText.Margin = new System.Windows.Forms.Padding(0);
            this.lblPickText.Name = "lblPickText";
            this.lblPickText.Size = new System.Drawing.Size(318, 122);
            this.lblPickText.TabIndex = 20052;
            this.lblPickText.Text = "Pick :";
            this.lblPickText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flpTabs
            // 
            this.flpTabs.AutoSize = true;
            this.flpTabs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTabs.BackColor = System.Drawing.Color.Transparent;
            this.flpTabs.Location = new System.Drawing.Point(18, 429);
            this.flpTabs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flpTabs.Name = "flpTabs";
            this.flpTabs.Size = new System.Drawing.Size(0, 0);
            this.flpTabs.TabIndex = 20054;
            // 
            // lblTabMsg
            // 
            this.lblTabMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F);
            this.lblTabMsg.ForeColor = System.Drawing.Color.White;
            this.lblTabMsg.Location = new System.Drawing.Point(4, 0);
            this.lblTabMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTabMsg.Name = "lblTabMsg";
            this.lblTabMsg.Size = new System.Drawing.Size(2654, 182);
            this.lblTabMsg.TabIndex = 20053;
            this.lblTabMsg.Text = "Pick the available date range from drop down or manually select the From date and" +
    " the To date to Reprint the report.";
            this.lblTabMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelKioskSummaryReport
            // 
            this.panelKioskSummaryReport.BackColor = System.Drawing.Color.Transparent;
            this.panelKioskSummaryReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelKioskSummaryReport.Controls.Add(this.lblTabMsg);
            this.panelKioskSummaryReport.Controls.Add(this.btnEmail);
            this.panelKioskSummaryReport.Controls.Add(this.pnlReprint);
            this.panelKioskSummaryReport.Controls.Add(this.btnPrint);
            this.panelKioskSummaryReport.Controls.Add(this.pnlFromToDates);
            this.panelKioskSummaryReport.Controls.Add(this.btnPrev);
            this.panelKioskSummaryReport.Controls.Add(this.pBPrintSummaryImage);
            this.panelKioskSummaryReport.Location = new System.Drawing.Point(105, 245);
            this.panelKioskSummaryReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelKioskSummaryReport.Name = "panelKioskSummaryReport";
            this.panelKioskSummaryReport.Size = new System.Drawing.Size(2666, 1269);
            this.panelKioskSummaryReport.TabIndex = 20055;
            // 
            // flpTabBars
            // 
            this.flpTabBars.AutoSize = true;
            this.flpTabBars.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTabBars.BackColor = System.Drawing.Color.Transparent;
            this.flpTabBars.Location = new System.Drawing.Point(18, 472);
            this.flpTabBars.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flpTabBars.Name = "flpTabBars";
            this.flpTabBars.Size = new System.Drawing.Size(0, 0);
            this.flpTabBars.TabIndex = 20057;
            // 
            // frmKioskPrintSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(2880, 1646);
            this.Controls.Add(this.btnNewTab);
            this.Controls.Add(this.flpTabBars);
            this.Controls.Add(this.btnRePrintTab);
            this.Controls.Add(this.panelKioskSummaryReport);
            this.Controls.Add(this.flpTabs);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblGreeting);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "frmKioskPrintSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kiosk Transaction View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.KioskPrintSummary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBFromCalender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBToCalender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBPrintSummaryImage)).EndInit();
            this.pnlFromToDates.ResumeLayout(false);
            this.pnlReprint.ResumeLayout(false);
            this.pnlReprint.PerformLayout();
            this.panelKioskSummaryReport.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Label lblFromText;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.PictureBox pBFromCalender;
        private System.Windows.Forms.PictureBox pBToCalender;
        private System.Windows.Forms.Label lblFromDateTime;
        private System.Windows.Forms.Label lblToDateTime;
        private System.Windows.Forms.Label lblToText;
        private System.Windows.Forms.Button btnRePrintTab;
        private System.Windows.Forms.PictureBox pBPrintSummaryImage;
        private System.Windows.Forms.ComboBox cmbReprintDateRange;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnNewTab;
        private System.Windows.Forms.Panel pnlFromToDates;
        private System.Windows.Forms.Label lblOR;
        private System.Windows.Forms.Panel pnlReprint;
        private System.Windows.Forms.FlowLayoutPanel flpTabs;
        private System.Windows.Forms.Label lblPickText;
        private System.Windows.Forms.Label lblTabMsg;
        private System.Windows.Forms.Panel panelKioskSummaryReport;
        private System.Windows.Forms.FlowLayoutPanel flpTabBars;
    }
}