namespace Semnox.Parafait.Customer.Accounts
{
    partial class AdvancedSearchUI
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
            this.rdbAnd = new System.Windows.Forms.RadioButton();
            this.rdbOr = new System.Windows.Forms.RadioButton();
            this.rdbNot = new System.Windows.Forms.RadioButton();
            this.lblField = new System.Windows.Forms.Label();
            this.cmbColumns = new System.Windows.Forms.ComboBox();
            this.lblCondition = new System.Windows.Forms.Label();
            this.cmbOperator = new System.Windows.Forms.ComboBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.flpCriteria = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.dtpValue = new System.Windows.Forms.DateTimePicker();
            this.cmbValue = new System.Windows.Forms.ComboBox();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdbAnd
            // 
            this.rdbAnd.AutoSize = true;
            this.rdbAnd.Checked = true;
            this.rdbAnd.Location = new System.Drawing.Point(14, 14);
            this.rdbAnd.Name = "rdbAnd";
            this.rdbAnd.Size = new System.Drawing.Size(49, 19);
            this.rdbAnd.TabIndex = 0;
            this.rdbAnd.TabStop = true;
            this.rdbAnd.Text = "AND";
            this.rdbAnd.UseVisualStyleBackColor = true;
            // 
            // rdbOr
            // 
            this.rdbOr.AutoSize = true;
            this.rdbOr.Location = new System.Drawing.Point(14, 40);
            this.rdbOr.Name = "rdbOr";
            this.rdbOr.Size = new System.Drawing.Size(42, 19);
            this.rdbOr.TabIndex = 1;
            this.rdbOr.Text = "OR";
            this.rdbOr.UseVisualStyleBackColor = true;
            // 
            // rdbNot
            // 
            this.rdbNot.AutoSize = true;
            this.rdbNot.Location = new System.Drawing.Point(14, 67);
            this.rdbNot.Name = "rdbNot";
            this.rdbNot.Size = new System.Drawing.Size(49, 19);
            this.rdbNot.TabIndex = 2;
            this.rdbNot.Text = "NOT";
            this.rdbNot.UseVisualStyleBackColor = true;
            this.rdbNot.Visible = false;
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            this.lblField.Location = new System.Drawing.Point(100, 18);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(33, 15);
            this.lblField.TabIndex = 3;
            this.lblField.Text = "Field";
            // 
            // cmbColumns
            // 
            this.cmbColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColumns.FormattingEnabled = true;
            this.cmbColumns.Location = new System.Drawing.Point(104, 40);
            this.cmbColumns.Name = "cmbColumns";
            this.cmbColumns.Size = new System.Drawing.Size(140, 23);
            this.cmbColumns.TabIndex = 4;
            this.cmbColumns.SelectedValueChanged += new System.EventHandler(this.cmbColumns_SelectedValueChanged);
            // 
            // lblCondition
            // 
            this.lblCondition.AutoSize = true;
            this.lblCondition.Location = new System.Drawing.Point(262, 18);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(60, 15);
            this.lblCondition.TabIndex = 5;
            this.lblCondition.Text = "Condition";
            // 
            // cmbOperator
            // 
            this.cmbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperator.FormattingEnabled = true;
            this.cmbOperator.Location = new System.Drawing.Point(266, 40);
            this.cmbOperator.Name = "cmbOperator";
            this.cmbOperator.Size = new System.Drawing.Size(140, 23);
            this.cmbOperator.TabIndex = 6;
            this.cmbOperator.SelectedValueChanged += new System.EventHandler(this.cmbOperator_SelectedValueChanged);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(420, 18);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(38, 15);
            this.lblValue.TabIndex = 7;
            this.lblValue.Text = "Value";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(427, 40);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(203, 21);
            this.txtValue.TabIndex = 8;
            this.txtValue.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(636, 14);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // flpCriteria
            // 
            this.flpCriteria.AutoScroll = true;
            this.flpCriteria.Location = new System.Drawing.Point(103, 69);
            this.flpCriteria.Name = "flpCriteria";
            this.flpCriteria.Size = new System.Drawing.Size(623, 272);
            this.flpCriteria.TabIndex = 10;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(248, 347);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(363, 347);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(636, 43);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // dtpValue
            // 
            this.dtpValue.CustomFormat = "dd-MMM-yyyy h:mm tt";
            this.dtpValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpValue.Location = new System.Drawing.Point(427, 40);
            this.dtpValue.Name = "dtpValue";
            this.dtpValue.Size = new System.Drawing.Size(203, 21);
            this.dtpValue.TabIndex = 14;
            this.dtpValue.Visible = false;
            // 
            // cmbValue
            // 
            this.cmbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValue.FormattingEnabled = true;
            this.cmbValue.Location = new System.Drawing.Point(427, 39);
            this.cmbValue.Name = "cmbValue";
            this.cmbValue.Size = new System.Drawing.Size(121, 23);
            this.cmbValue.TabIndex = 15;
            this.cmbValue.Visible = false;
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Semnox.Parafait.Customer.Properties.Resources.keyboard;
            this.btnShowKeyPad.Location = new System.Drawing.Point(701, 346);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowKeyPad.TabIndex = 48;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // AdvancedSearchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 380);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.cmbValue);
            this.Controls.Add(this.dtpValue);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.flpCriteria);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.cmbOperator);
            this.Controls.Add(this.lblCondition);
            this.Controls.Add(this.cmbColumns);
            this.Controls.Add(this.lblField);
            this.Controls.Add(this.rdbNot);
            this.Controls.Add(this.rdbOr);
            this.Controls.Add(this.rdbAnd);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.KeyPreview = true;
            this.Name = "AdvancedSearchUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Search";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomerAdvancedSearchUI_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbAnd;
        private System.Windows.Forms.RadioButton rdbOr;
        private System.Windows.Forms.RadioButton rdbNot;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.ComboBox cmbColumns;
        private System.Windows.Forms.Label lblCondition;
        private System.Windows.Forms.ComboBox cmbOperator;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.FlowLayoutPanel flpCriteria;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.DateTimePicker dtpValue;
        private System.Windows.Forms.ComboBox cmbValue;
        private System.Windows.Forms.Button btnShowKeyPad;
    }
}