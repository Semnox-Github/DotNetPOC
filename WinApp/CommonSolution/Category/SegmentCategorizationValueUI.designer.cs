namespace Semnox.Parafait.Category
{
    partial class SegmentCategorizationValueUI
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
            this.grpBoxSegments = new System.Windows.Forms.GroupBox();
            this.flpCategorizationValues = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSegmentCategoryValueLable = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSegmentIdLabel = new System.Windows.Forms.Label();
            this.lblSegmentId = new System.Windows.Forms.Label();
            this.lblSegmentCategoryValue = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.lblSaveSuccessful = new System.Windows.Forms.Label();
            this.dgvFilter = new System.Windows.Forms.DataGridView();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.grpBoxSegments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBoxSegments
            // 
            this.grpBoxSegments.Controls.Add(this.flpCategorizationValues);
            this.grpBoxSegments.Location = new System.Drawing.Point(16, 97);
            this.grpBoxSegments.Name = "grpBoxSegments";
            this.grpBoxSegments.Size = new System.Drawing.Size(573, 375);
            this.grpBoxSegments.TabIndex = 38;
            this.grpBoxSegments.TabStop = false;
            // 
            // flpCategorizationValues
            // 
            this.flpCategorizationValues.AutoScroll = true;
            this.flpCategorizationValues.Location = new System.Drawing.Point(11, 19);
            this.flpCategorizationValues.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flpCategorizationValues.Name = "flpCategorizationValues";
            this.flpCategorizationValues.Size = new System.Drawing.Size(551, 344);
            this.flpCategorizationValues.TabIndex = 31;
            // 
            // lblSegmentCategoryValueLable
            // 
            this.lblSegmentCategoryValueLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSegmentCategoryValueLable.Location = new System.Drawing.Point(5, 44);
            this.lblSegmentCategoryValueLable.Name = "lblSegmentCategoryValueLable";
            this.lblSegmentCategoryValueLable.Size = new System.Drawing.Size(172, 15);
            this.lblSegmentCategoryValueLable.TabIndex = 37;
            this.lblSegmentCategoryValueLable.Text = "SKU Values: ";
            this.lblSegmentCategoryValueLable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(242, 480);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 28);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(129, 480);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 28);
            this.btnRefresh.TabIndex = 34;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(16, 480);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 28);
            this.btnSave.TabIndex = 35;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSegmentIdLabel
            // 
            this.lblSegmentIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSegmentIdLabel.Location = new System.Drawing.Point(12, 74);
            this.lblSegmentIdLabel.Name = "lblSegmentIdLabel";
            this.lblSegmentIdLabel.Size = new System.Drawing.Size(165, 15);
            this.lblSegmentIdLabel.TabIndex = 39;
            this.lblSegmentIdLabel.Text = "SKU ID: ";
            this.lblSegmentIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSegmentId
            // 
            this.lblSegmentId.AutoSize = true;
            this.lblSegmentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSegmentId.Location = new System.Drawing.Point(177, 74);
            this.lblSegmentId.Name = "lblSegmentId";
            this.lblSegmentId.Size = new System.Drawing.Size(47, 15);
            this.lblSegmentId.TabIndex = 41;
            this.lblSegmentId.Text = "label2";
            // 
            // lblSegmentCategoryValue
            // 
            this.lblSegmentCategoryValue.AutoSize = true;
            this.lblSegmentCategoryValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSegmentCategoryValue.Location = new System.Drawing.Point(177, 44);
            this.lblSegmentCategoryValue.Name = "lblSegmentCategoryValue";
            this.lblSegmentCategoryValue.Size = new System.Drawing.Size(47, 15);
            this.lblSegmentCategoryValue.TabIndex = 40;
            this.lblSegmentCategoryValue.Text = "label1";
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(177, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(51, 16);
            this.lblHeading.TabIndex = 42;
            this.lblHeading.Text = "label1";
            // 
            // lblSaveSuccessful
            // 
            this.lblSaveSuccessful.AutoSize = true;
            this.lblSaveSuccessful.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaveSuccessful.Location = new System.Drawing.Point(359, 486);
            this.lblSaveSuccessful.Name = "lblSaveSuccessful";
            this.lblSaveSuccessful.Size = new System.Drawing.Size(111, 15);
            this.lblSaveSuccessful.TabIndex = 43;
            this.lblSaveSuccessful.Text = "Save Successful";
            this.lblSaveSuccessful.Visible = false;
            // 
            // dgvFilter
            // 
            this.dgvFilter.AllowUserToAddRows = false;
            this.dgvFilter.AllowUserToDeleteRows = false;
            this.dgvFilter.BackgroundColor = System.Drawing.Color.White;
            this.dgvFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFilter.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilter.ColumnHeadersVisible = false;
            this.dgvFilter.Location = new System.Drawing.Point(611, 106);
            this.dgvFilter.Name = "dgvFilter";
            this.dgvFilter.ReadOnly = true;
            this.dgvFilter.RowHeadersVisible = false;
            this.dgvFilter.RowTemplate.Height = 35;
            this.dgvFilter.Size = new System.Drawing.Size(139, 181);
            this.dgvFilter.TabIndex = 62;
            this.dgvFilter.Visible = false;
            this.dgvFilter.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilter_CellClick);
            this.dgvFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFilter_KeyDown);
            // 
            // txtFilter
            // 
            this.txtFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtFilter.Location = new System.Drawing.Point(621, 145);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(119, 21);
            this.txtFilter.TabIndex = 61;
            this.txtFilter.Visible = false;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            this.txtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyDown);
            // 
            // SegmentCategorizationValueUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 524);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.dgvFilter);
            this.Controls.Add(this.lblSaveSuccessful);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.lblSegmentId);
            this.Controls.Add(this.lblSegmentCategoryValue);
            this.Controls.Add(this.lblSegmentIdLabel);
            this.Controls.Add(this.grpBoxSegments);
            this.Controls.Add(this.lblSegmentCategoryValueLable);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SegmentCategorizationValueUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SKU Data";
            this.Load += new System.EventHandler(this.SegmentCategorizationValueUI_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SegmentCategorizationValueUI_MouseClick);
            this.grpBoxSegments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxSegments;
        private System.Windows.Forms.FlowLayoutPanel flpCategorizationValues;
        private System.Windows.Forms.Label lblSegmentCategoryValueLable;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSegmentIdLabel;
        private System.Windows.Forms.Label lblSegmentId;
        private System.Windows.Forms.Label lblSegmentCategoryValue;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblSaveSuccessful;
        private System.Windows.Forms.DataGridView dgvFilter;
        private System.Windows.Forms.TextBox txtFilter;
    }
}

