namespace Semnox.Parafait.Inventory.Recipe
{
    partial class frmRecipeDailyEstimationDetails
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
            this.grpMonth = new System.Windows.Forms.GroupBox();
            this.Calendar = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblEstimate = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblMonth = new System.Windows.Forms.Label();
            this.grpMonth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Calendar)).BeginInit();
            this.SuspendLayout();
            // 
            // grpMonth
            // 
            this.grpMonth.Controls.Add(this.Calendar);
            this.grpMonth.Location = new System.Drawing.Point(4, 54);
            this.grpMonth.Name = "grpMonth";
            this.grpMonth.Size = new System.Drawing.Size(877, 384);
            this.grpMonth.TabIndex = 5;
            this.grpMonth.TabStop = false;
            this.grpMonth.Text = "Month";
            // 
            // Calendar
            // 
            this.Calendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Calendar.Location = new System.Drawing.Point(22, 19);
            this.Calendar.Name = "Calendar";
            this.Calendar.Size = new System.Drawing.Size(835, 361);
            this.Calendar.TabIndex = 3;
            this.Calendar.Paint += new System.Windows.Forms.PaintEventHandler(this.Calendar_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(336, 444);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(123, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblEstimate
            // 
            this.lblEstimate.Location = new System.Drawing.Point(12, 9);
            this.lblEstimate.Name = "lblEstimate";
            this.lblEstimate.Size = new System.Drawing.Size(410, 23);
            this.lblEstimate.TabIndex = 7;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.DarkKhaki;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrev.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(483, 12);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(33, 25);
            this.btnPrev.TabIndex = 11;
            this.btnPrev.Text = "<<";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.DarkKhaki;
            this.btnNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(589, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(33, 25);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(522, 18);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(61, 23);
            this.lblMonth.TabIndex = 12;
            // 
            // frmRecipeDailyEstimationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 484);
            this.Controls.Add(this.lblMonth);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblEstimate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpMonth);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRecipeDailyEstimationDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.grpMonth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Calendar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMonth;
        private System.Windows.Forms.DataGridView Calendar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblEstimate;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblMonth;
    }
}