using Semnox.Parafait.Game;

namespace Semnox.Parafait.Inventory
{
    partial class ImportMachines
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportMachines));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chkImportAllMachines = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvMachines = new System.Windows.Forms.DataGridView();
            this.SelectMachine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.machineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLocationType = new System.Windows.Forms.ComboBox();
            this.locationTypeDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMachines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationTypeDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // chkImportAllMachines
            // 
            this.chkImportAllMachines.AutoSize = true;
            this.chkImportAllMachines.Location = new System.Drawing.Point(38, 58);
            this.chkImportAllMachines.Name = "chkImportAllMachines";
            this.chkImportAllMachines.Size = new System.Drawing.Size(118, 17);
            this.chkImportAllMachines.TabIndex = 1;
            this.chkImportAllMachines.Text = "Import All Machines";
            this.chkImportAllMachines.UseVisualStyleBackColor = true;
            this.chkImportAllMachines.CheckedChanged += new System.EventHandler(this.chkImportAllMachines_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(147, 399);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(38, 399);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvMachines
            // 
            this.dgvMachines.AllowUserToAddRows = false;
            this.dgvMachines.AllowUserToDeleteRows = false;
            this.dgvMachines.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMachines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMachines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMachines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectMachine,
            this.machineIdDataGridViewTextBoxColumn,
            this.machineNameDataGridViewTextBoxColumn});
            this.dgvMachines.DataSource = this.machineDTOBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMachines.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMachines.Location = new System.Drawing.Point(36, 81);
            this.dgvMachines.Name = "dgvMachines";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMachines.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMachines.Size = new System.Drawing.Size(374, 306);
            this.dgvMachines.TabIndex = 22;
            // 
            // SelectMachine
            // 
            this.SelectMachine.FalseValue = "false";
            this.SelectMachine.HeaderText = "Select";
            this.SelectMachine.Name = "SelectMachine";
            this.SelectMachine.TrueValue = "true";
            // 
            // machineIdDataGridViewTextBoxColumn
            // 
            this.machineIdDataGridViewTextBoxColumn.DataPropertyName = "MachineId";
            this.machineIdDataGridViewTextBoxColumn.HeaderText = "MachineId";
            this.machineIdDataGridViewTextBoxColumn.Name = "machineIdDataGridViewTextBoxColumn";
            // 
            // machineNameDataGridViewTextBoxColumn
            // 
            this.machineNameDataGridViewTextBoxColumn.DataPropertyName = "MachineName";
            this.machineNameDataGridViewTextBoxColumn.HeaderText = "MachineName";
            this.machineNameDataGridViewTextBoxColumn.Name = "machineNameDataGridViewTextBoxColumn";
            // 
            // machineDTOBindingSource
            // 
            this.machineDTOBindingSource.DataSource = typeof(MachineDTO);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Select Location Type: ";
            // 
            // cmbLocationType
            // 
            this.cmbLocationType.DataSource = this.locationTypeDTOBindingSource;
            this.cmbLocationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocationType.FormattingEnabled = true;
            this.cmbLocationType.Location = new System.Drawing.Point(147, 15);
            this.cmbLocationType.Name = "cmbLocationType";
            this.cmbLocationType.Size = new System.Drawing.Size(121, 21);
            this.cmbLocationType.TabIndex = 24;
            // 
            // locationTypeDTOBindingSource
            // 
            this.locationTypeDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationTypeDTO);
            // 
            // ImportMachines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 433);
            this.Controls.Add(this.cmbLocationType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvMachines);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkImportAllMachines);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ImportMachines";
            this.Text = "Import Machines";
            this.Load += new System.EventHandler(this.ImportMachines_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMachines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationTypeDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkImportAllMachines;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvMachines;
        private System.Windows.Forms.BindingSource machineDTOBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectMachine;
        private System.Windows.Forms.DataGridViewTextBoxColumn machineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn machineNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbLocationType;
        private System.Windows.Forms.BindingSource locationTypeDTOBindingSource;
    }
}