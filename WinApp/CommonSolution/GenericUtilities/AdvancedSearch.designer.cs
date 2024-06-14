namespace Semnox.Core.GenericUtilities
{
    partial class AdvancedSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rbAnd = new System.Windows.Forms.RadioButton();
            this.rbOr = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvCriteria = new System.Windows.Forms.DataGridView();
            this.criteria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datatype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operatOr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AndOr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.lnkDateLookup = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cmbCriteria = new System.Windows.Forms.ComboBox();
            this.cmbSegments = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCriteria)).BeginInit();
            this.SuspendLayout();
            // 
            // rbAnd
            // 
            this.rbAnd.AutoSize = true;
            this.rbAnd.Checked = true;
            this.rbAnd.Location = new System.Drawing.Point(12, 9);
            this.rbAnd.Name = "rbAnd";
            this.rbAnd.Size = new System.Drawing.Size(48, 17);
            this.rbAnd.TabIndex = 3;
            this.rbAnd.TabStop = true;
            this.rbAnd.Text = "AND";
            this.rbAnd.UseVisualStyleBackColor = true;
            // 
            // rbOr
            // 
            this.rbOr.AutoSize = true;
            this.rbOr.Location = new System.Drawing.Point(12, 31);
            this.rbOr.Name = "rbOr";
            this.rbOr.Size = new System.Drawing.Size(41, 17);
            this.rbOr.TabIndex = 4;
            this.rbOr.Text = "OR";
            this.rbOr.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Criteria";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(145, 281);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(377, 281);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvCriteria
            // 
            this.dgvCriteria.AllowUserToAddRows = false;
            this.dgvCriteria.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCriteria.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvCriteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCriteria.ColumnHeadersVisible = false;
            this.dgvCriteria.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.criteria,
            this.remove,
            this.field,
            this.datatype,
            this.operatOr,
            this.value,
            this.AndOr});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCriteria.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvCriteria.GridColor = System.Drawing.Color.White;
            this.dgvCriteria.Location = new System.Drawing.Point(76, 70);
            this.dgvCriteria.Name = "dgvCriteria";
            this.dgvCriteria.ReadOnly = true;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCriteria.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvCriteria.RowHeadersVisible = false;
            this.dgvCriteria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCriteria.Size = new System.Drawing.Size(483, 198);
            this.dgvCriteria.TabIndex = 12;
            this.dgvCriteria.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCriteria_CellClick);
            // 
            // criteria
            // 
            this.criteria.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.criteria.HeaderText = "criteria";
            this.criteria.Name = "criteria";
            this.criteria.ReadOnly = true;
            // 
            // remove
            // 
            this.remove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.White;
            this.remove.DefaultCellStyle = dataGridViewCellStyle14;
            this.remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.remove.HeaderText = "Remove";
            this.remove.Name = "remove";
            this.remove.ReadOnly = true;
            this.remove.Text = "X";
            this.remove.UseColumnTextForButtonValue = true;
            this.remove.Width = 5;
            // 
            // field
            // 
            this.field.HeaderText = "filed";
            this.field.Name = "field";
            this.field.ReadOnly = true;
            this.field.Visible = false;
            // 
            // datatype
            // 
            this.datatype.HeaderText = "datatype";
            this.datatype.Name = "datatype";
            this.datatype.ReadOnly = true;
            this.datatype.Visible = false;
            // 
            // operatOr
            // 
            this.operatOr.HeaderText = "operator";
            this.operatOr.Name = "operatOr";
            this.operatOr.ReadOnly = true;
            this.operatOr.Visible = false;
            // 
            // value
            // 
            this.value.HeaderText = "value";
            this.value.Name = "value";
            this.value.ReadOnly = true;
            this.value.Visible = false;
            // 
            // AndOr
            // 
            this.AndOr.HeaderText = "AndOr";
            this.AndOr.Name = "AndOr";
            this.AndOr.ReadOnly = true;
            this.AndOr.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(193, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Condition";
            // 
            // lnkDateLookup
            // 
            this.lnkDateLookup.AutoSize = true;
            this.lnkDateLookup.Location = new System.Drawing.Point(427, 51);
            this.lnkDateLookup.Name = "lnkDateLookup";
            this.lnkDateLookup.Size = new System.Drawing.Size(69, 13);
            this.lnkDateLookup.TabIndex = 21;
            this.lnkDateLookup.TabStop = true;
            this.lnkDateLookup.Text = "Date Lookup";
            this.lnkDateLookup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDateLookup_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "SKU Segment";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(500, 26);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 23);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(295, 28);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(199, 20);
            this.txtValue.TabIndex = 17;
            // 
            // cmbCriteria
            // 
            this.cmbCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCriteria.FormattingEnabled = true;
            this.cmbCriteria.Items.AddRange(new object[] {
            "Contains",
            "Does not contain",
            "=",
            ">",
            ">=",
            "<",
            "<=",
            "is null",
            "is not null"});
            this.cmbCriteria.Location = new System.Drawing.Point(196, 28);
            this.cmbCriteria.Name = "cmbCriteria";
            this.cmbCriteria.Size = new System.Drawing.Size(93, 21);
            this.cmbCriteria.TabIndex = 16;
            // 
            // cmbSegments
            // 
            this.cmbSegments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSegments.FormattingEnabled = true;
            this.cmbSegments.Location = new System.Drawing.Point(77, 28);
            this.cmbSegments.Name = "cmbSegments";
            this.cmbSegments.Size = new System.Drawing.Size(112, 21);
            this.cmbSegments.TabIndex = 15;
            // 
            // AdvancedSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(593, 314);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lnkDateLookup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.cmbCriteria);
            this.Controls.Add(this.cmbSegments);
            this.Controls.Add(this.dgvCriteria);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbOr);
            this.Controls.Add(this.rbAnd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AdvancedSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Search";
            this.Load += new System.EventHandler(this.AdvancedSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCriteria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbAnd;
        private System.Windows.Forms.RadioButton rbOr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvCriteria;
        private System.Windows.Forms.DataGridViewTextBoxColumn criteria;
        private System.Windows.Forms.DataGridViewButtonColumn remove;
        private System.Windows.Forms.DataGridViewTextBoxColumn field;
        private System.Windows.Forms.DataGridViewTextBoxColumn datatype;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatOr;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn AndOr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lnkDateLookup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.ComboBox cmbCriteria;
        private System.Windows.Forms.ComboBox cmbSegments;
    }
}