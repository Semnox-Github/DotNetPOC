namespace Semnox.Parafait.Game
{
    partial class MachineInputDevicesUI
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
            this.dgMachineInputDevices = new System.Windows.Forms.DataGridView();
            this.DeviceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.drpDeviceTypeId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.drpDeviceModelId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MachineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PortNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MacAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.drpFPTemplateFormat = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MasterEntityId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineInputDevicesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dgMachineInputDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineInputDevicesDTOBindingSource)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgMachineInputDevices
            // 
            this.dgMachineInputDevices.AllowUserToDeleteRows = false;
            this.dgMachineInputDevices.AutoGenerateColumns = false;
            this.dgMachineInputDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMachineInputDevices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeviceId,
            this.DeviceName,
            this.drpDeviceTypeId,
            this.drpDeviceModelId,
            this.MachineId,
            this.IsActive,
            this.IPAddress,
            this.PortNo,
            this.MacAddress,
            this.drpFPTemplateFormat,
            this.MasterEntityId});
            this.dgMachineInputDevices.DataSource = this.machineInputDevicesDTOBindingSource;
            this.dgMachineInputDevices.Location = new System.Drawing.Point(6, 6);
            this.dgMachineInputDevices.Name = "dgMachineInputDevices";
            this.dgMachineInputDevices.Size = new System.Drawing.Size(1090, 404);
            this.dgMachineInputDevices.TabIndex = 0;
            this.dgMachineInputDevices.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgMachineInputDevices_DataError);
            // 
            // DeviceId
            // 
            this.DeviceId.DataPropertyName = "DeviceId";
            this.DeviceId.Frozen = true;
            this.DeviceId.HeaderText = "Device Id";
            this.DeviceId.Name = "DeviceId";
            this.DeviceId.ReadOnly = true;
            // 
            // DeviceName
            // 
            this.DeviceName.DataPropertyName = "DeviceName";
            this.DeviceName.Frozen = true;
            this.DeviceName.HeaderText = "Device Name";
            this.DeviceName.Name = "DeviceName";
            // 
            // drpDeviceTypeId
            // 
            this.drpDeviceTypeId.DataPropertyName = "DeviceTypeId";
            this.drpDeviceTypeId.HeaderText = "Device Type ";
            this.drpDeviceTypeId.Name = "drpDeviceTypeId";
            this.drpDeviceTypeId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.drpDeviceTypeId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // drpDeviceModelId
            // 
            this.drpDeviceModelId.DataPropertyName = "DeviceModelId";
            this.drpDeviceModelId.HeaderText = "Device Model ";
            this.drpDeviceModelId.Name = "drpDeviceModelId";
            this.drpDeviceModelId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.drpDeviceModelId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MachineId
            // 
            this.MachineId.DataPropertyName = "MachineId";
            this.MachineId.HeaderText = "Machine ";
            this.MachineId.Name = "MachineId";
            this.MachineId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MachineId.Visible = false;
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "Active";
            this.IsActive.Name = "IsActive";
            // 
            // IPAddress
            // 
            this.IPAddress.DataPropertyName = "IPAddress";
            this.IPAddress.HeaderText = "IP Address";
            this.IPAddress.Name = "IPAddress";
            // 
            // PortNo
            // 
            this.PortNo.DataPropertyName = "PortNo";
            this.PortNo.HeaderText = "Port No";
            this.PortNo.Name = "PortNo";
            // 
            // MacAddress
            // 
            this.MacAddress.DataPropertyName = "MacAddress";
            this.MacAddress.HeaderText = "Mac Address";
            this.MacAddress.Name = "MacAddress";
            // 
            // drpFPTemplateFormat
            // 
            this.drpFPTemplateFormat.DataPropertyName = "FPTemplateFormat";
            this.drpFPTemplateFormat.HeaderText = "Template Format";
            this.drpFPTemplateFormat.Name = "drpFPTemplateFormat";
            this.drpFPTemplateFormat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.drpFPTemplateFormat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MasterEntityId
            // 
            this.MasterEntityId.DataPropertyName = "MasterEntityId";
            this.MasterEntityId.HeaderText = "Master Entity Id";
            this.MasterEntityId.Name = "MasterEntityId";
            this.MasterEntityId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MasterEntityId.Visible = false;
            // 
            // machineInputDevicesDTOBindingSource
            // 
            this.machineInputDevicesDTOBindingSource.DataSource = typeof(MachineInputDevicesDTO);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(388, 481);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 23);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(266, 481);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 23);
            this.btnClose.TabIndex = 31;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(144, 481);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(82, 23);
            this.btnRefresh.TabIndex = 30;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(22, 481);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 23);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1134, 442);
            this.tabControl1.TabIndex = 33;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.dgMachineInputDevices);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1126, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Devices";
            // 
            // MachineInputDevicesUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1158, 516);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnDelete);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MachineInputDevicesUI";
            this.Text = "Machine Input Devices";
            ((System.ComponentModel.ISupportInitialize)(this.dgMachineInputDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineInputDevicesDTOBindingSource)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgMachineInputDevices;
        private System.Windows.Forms.BindingSource machineInputDevicesDTOBindingSource;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName;
        private System.Windows.Forms.DataGridViewComboBoxColumn drpDeviceTypeId;
        private System.Windows.Forms.DataGridViewComboBoxColumn drpDeviceModelId;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn IPAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn MacAddress;
        private System.Windows.Forms.DataGridViewComboBoxColumn drpFPTemplateFormat;
        private System.Windows.Forms.DataGridViewTextBoxColumn MasterEntityId;
    }
}

