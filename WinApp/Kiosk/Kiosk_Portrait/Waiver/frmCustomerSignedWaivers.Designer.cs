namespace Parafait_Kiosk
{
    partial class frmCustomerSignedWaivers
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvCustomerSignedWaiver = new System.Windows.Forms.DataGridView();
            this.selectRecord = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.waiverSetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedWaiverFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSignedWaiverIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WaiverName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedByNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverSetDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedForDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedForNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.deactivatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deactivationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deactivationApprovedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverSetDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSignedWaiverDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.bigVerticalScrollCustomerSignedWaiver = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.bigHorizontalScrollCustomerSignedWaiver = new Semnox.Core.GenericUtilities.BigHorizontalScrollBarView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.lblCustomer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerSignedWaiver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerSignedWaiverDTOBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.btnHome.TabIndex = 146;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(140, 1670);
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
            this.btnCancel.Location = new System.Drawing.Point(605, 1670);
            this.btnCancel.TabIndex = 1027;
            // 
            // dgvCustomerSignedWaiver
            // 
            this.dgvCustomerSignedWaiver.AllowUserToAddRows = false;
            this.dgvCustomerSignedWaiver.AllowUserToDeleteRows = false;
            this.dgvCustomerSignedWaiver.AllowUserToResizeColumns = false;
            this.dgvCustomerSignedWaiver.AllowUserToResizeRows = false;
            this.dgvCustomerSignedWaiver.AutoGenerateColumns = false;
            this.dgvCustomerSignedWaiver.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvCustomerSignedWaiver.BackgroundColor = System.Drawing.Color.White;
            this.dgvCustomerSignedWaiver.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCustomerSignedWaiver.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvCustomerSignedWaiver.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCustomerSignedWaiver.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCustomerSignedWaiver.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerSignedWaiver.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectRecord,
            this.waiverSetIdDataGridViewTextBoxColumn,
            this.signedWaiverFileNameDataGridViewTextBoxColumn,
            this.customerSignedWaiverIdDataGridViewTextBoxColumn,
            this.WaiverName,
            this.signedByNameDataGridViewTextBoxColumn,
            this.signedDateDataGridViewTextBoxColumn,
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn,
            this.waiverSetDetailIdDataGridViewTextBoxColumn,
            this.signedForDataGridViewTextBoxColumn,
            this.signedForNameDataGridViewTextBoxColumn,
            this.waiverCodeDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.deactivatedByDataGridViewTextBoxColumn,
            this.deactivationDateDataGridViewTextBoxColumn,
            this.deactivationApprovedByDataGridViewTextBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.signedByDataGridViewTextBoxColumn,
            this.waiverSetDescriptionDataGridViewTextBoxColumn});
            this.dgvCustomerSignedWaiver.DataSource = this.customerSignedWaiverDTOBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCustomerSignedWaiver.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCustomerSignedWaiver.EnableHeadersVisualStyles = false;
            this.dgvCustomerSignedWaiver.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvCustomerSignedWaiver.Location = new System.Drawing.Point(23, 41);
            this.dgvCustomerSignedWaiver.MultiSelect = false;
            this.dgvCustomerSignedWaiver.Name = "dgvCustomerSignedWaiver";
            this.dgvCustomerSignedWaiver.ReadOnly = true;
            this.dgvCustomerSignedWaiver.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvCustomerSignedWaiver.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCustomerSignedWaiver.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCustomerSignedWaiver.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCustomerSignedWaiver.RowTemplate.Height = 80;
            this.dgvCustomerSignedWaiver.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCustomerSignedWaiver.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCustomerSignedWaiver.Size = new System.Drawing.Size(904, 1078);
            this.dgvCustomerSignedWaiver.TabIndex = 13;
            this.dgvCustomerSignedWaiver.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerSignedWaiver_CellClick);
            // 
            // selectRecord
            // 
            this.selectRecord.FalseValue = "false";
            this.selectRecord.Frozen = true;
            this.selectRecord.HeaderText = "";
            this.selectRecord.MinimumWidth = 60;
            this.selectRecord.Name = "selectRecord";
            this.selectRecord.ReadOnly = true;
            this.selectRecord.TrueValue = "true";
            this.selectRecord.Width = 60;
            // 
            // waiverSetIdDataGridViewTextBoxColumn
            // 
            this.waiverSetIdDataGridViewTextBoxColumn.DataPropertyName = "WaiverSetId";
            this.waiverSetIdDataGridViewTextBoxColumn.HeaderText = "WaiverSetId";
            this.waiverSetIdDataGridViewTextBoxColumn.Name = "waiverSetIdDataGridViewTextBoxColumn";
            this.waiverSetIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiverSetIdDataGridViewTextBoxColumn.Visible = false;
            this.waiverSetIdDataGridViewTextBoxColumn.Width = 163;
            // 
            // signedWaiverFileNameDataGridViewTextBoxColumn
            // 
            this.signedWaiverFileNameDataGridViewTextBoxColumn.DataPropertyName = "SignedWaiverFileName";
            this.signedWaiverFileNameDataGridViewTextBoxColumn.HeaderText = "SignedWaiverFileName";
            this.signedWaiverFileNameDataGridViewTextBoxColumn.Name = "signedWaiverFileNameDataGridViewTextBoxColumn";
            this.signedWaiverFileNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedWaiverFileNameDataGridViewTextBoxColumn.Visible = false;
            this.signedWaiverFileNameDataGridViewTextBoxColumn.Width = 289;
            // 
            // customerSignedWaiverIdDataGridViewTextBoxColumn
            // 
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerSignedWaiverId";
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.Name = "customerSignedWaiverIdDataGridViewTextBoxColumn";
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.Width = 69;
            // 
            // WaiverName
            // 
            this.WaiverName.DataPropertyName = "WaiverName";
            this.WaiverName.HeaderText = "Waiver Name";
            this.WaiverName.Name = "WaiverName";
            this.WaiverName.ReadOnly = true;
            this.WaiverName.Width = 209;
            // 
            // signedByNameDataGridViewTextBoxColumn
            // 
            this.signedByNameDataGridViewTextBoxColumn.DataPropertyName = "SignedByName";
            this.signedByNameDataGridViewTextBoxColumn.HeaderText = "Signed By";
            this.signedByNameDataGridViewTextBoxColumn.Name = "signedByNameDataGridViewTextBoxColumn";
            this.signedByNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedByNameDataGridViewTextBoxColumn.Width = 166;
            // 
            // signedDateDataGridViewTextBoxColumn
            // 
            this.signedDateDataGridViewTextBoxColumn.DataPropertyName = "SignedDate";
            this.signedDateDataGridViewTextBoxColumn.HeaderText = "Signed Date";
            this.signedDateDataGridViewTextBoxColumn.Name = "signedDateDataGridViewTextBoxColumn";
            this.signedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedDateDataGridViewTextBoxColumn.Width = 194;
            // 
            // customerSignedWaiverHeaderIdDataGridViewTextBoxColumn
            // 
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerSignedWaiverHeaderId";
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.HeaderText = "CustomerSignedWaiverHeaderId";
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.Name = "customerSignedWaiverHeaderIdDataGridViewTextBoxColumn";
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.Visible = false;
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.Width = 425;
            // 
            // waiverSetDetailIdDataGridViewTextBoxColumn
            // 
            this.waiverSetDetailIdDataGridViewTextBoxColumn.DataPropertyName = "WaiverSetDetailId";
            this.waiverSetDetailIdDataGridViewTextBoxColumn.HeaderText = "WaiverSetDetailId";
            this.waiverSetDetailIdDataGridViewTextBoxColumn.Name = "waiverSetDetailIdDataGridViewTextBoxColumn";
            this.waiverSetDetailIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiverSetDetailIdDataGridViewTextBoxColumn.Visible = false;
            this.waiverSetDetailIdDataGridViewTextBoxColumn.Width = 250;
            // 
            // signedForDataGridViewTextBoxColumn
            // 
            this.signedForDataGridViewTextBoxColumn.DataPropertyName = "SignedFor";
            this.signedForDataGridViewTextBoxColumn.HeaderText = "SignedFor";
            this.signedForDataGridViewTextBoxColumn.Name = "signedForDataGridViewTextBoxColumn";
            this.signedForDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedForDataGridViewTextBoxColumn.Visible = false;
            this.signedForDataGridViewTextBoxColumn.Width = 156;
            // 
            // signedForNameDataGridViewTextBoxColumn
            // 
            this.signedForNameDataGridViewTextBoxColumn.DataPropertyName = "SignedForName";
            this.signedForNameDataGridViewTextBoxColumn.HeaderText = "Signed For";
            this.signedForNameDataGridViewTextBoxColumn.Name = "signedForNameDataGridViewTextBoxColumn";
            this.signedForNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedForNameDataGridViewTextBoxColumn.Width = 173;
            // 
            // waiverCodeDataGridViewTextBoxColumn
            // 
            this.waiverCodeDataGridViewTextBoxColumn.DataPropertyName = "WaiverCode";
            this.waiverCodeDataGridViewTextBoxColumn.HeaderText = "Waiver Code";
            this.waiverCodeDataGridViewTextBoxColumn.Name = "waiverCodeDataGridViewTextBoxColumn";
            this.waiverCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiverCodeDataGridViewTextBoxColumn.Width = 199;
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "Expiry Date";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            this.expiryDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.expiryDateDataGridViewTextBoxColumn.Width = 187;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            this.isActiveDataGridViewCheckBoxColumn.Width = 121;
            // 
            // deactivatedByDataGridViewTextBoxColumn
            // 
            this.deactivatedByDataGridViewTextBoxColumn.DataPropertyName = "DeactivatedBy";
            this.deactivatedByDataGridViewTextBoxColumn.HeaderText = "DeactivatedBy";
            this.deactivatedByDataGridViewTextBoxColumn.Name = "deactivatedByDataGridViewTextBoxColumn";
            this.deactivatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.deactivatedByDataGridViewTextBoxColumn.Visible = false;
            this.deactivatedByDataGridViewTextBoxColumn.Width = 208;
            // 
            // deactivationDateDataGridViewTextBoxColumn
            // 
            this.deactivationDateDataGridViewTextBoxColumn.DataPropertyName = "DeactivationDate";
            this.deactivationDateDataGridViewTextBoxColumn.HeaderText = "DeactivationDate";
            this.deactivationDateDataGridViewTextBoxColumn.Name = "deactivationDateDataGridViewTextBoxColumn";
            this.deactivationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.deactivationDateDataGridViewTextBoxColumn.Visible = false;
            this.deactivationDateDataGridViewTextBoxColumn.Width = 241;
            // 
            // deactivationApprovedByDataGridViewTextBoxColumn
            // 
            this.deactivationApprovedByDataGridViewTextBoxColumn.DataPropertyName = "DeactivationApprovedBy";
            this.deactivationApprovedByDataGridViewTextBoxColumn.HeaderText = "DeactivationApprovedBy";
            this.deactivationApprovedByDataGridViewTextBoxColumn.Name = "deactivationApprovedByDataGridViewTextBoxColumn";
            this.deactivationApprovedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.deactivationApprovedByDataGridViewTextBoxColumn.Visible = false;
            this.deactivationApprovedByDataGridViewTextBoxColumn.Width = 329;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            this.guidDataGridViewTextBoxColumn.Width = 92;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "Master Entity Id";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            this.masterEntityIdDataGridViewTextBoxColumn.Width = 226;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            this.createdByDataGridViewTextBoxColumn.Width = 168;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            this.creationDateDataGridViewTextBoxColumn.Width = 201;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Width = 213;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdateDateDataGridViewTextBoxColumn.Width = 258;
            // 
            // signedByDataGridViewTextBoxColumn
            // 
            this.signedByDataGridViewTextBoxColumn.DataPropertyName = "SignedBy";
            this.signedByDataGridViewTextBoxColumn.HeaderText = "SignedBy";
            this.signedByDataGridViewTextBoxColumn.Name = "signedByDataGridViewTextBoxColumn";
            this.signedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedByDataGridViewTextBoxColumn.Visible = false;
            this.signedByDataGridViewTextBoxColumn.Width = 148;
            // 
            // waiverSetDescriptionDataGridViewTextBoxColumn
            // 
            this.waiverSetDescriptionDataGridViewTextBoxColumn.DataPropertyName = "WaiverSetDescription";
            this.waiverSetDescriptionDataGridViewTextBoxColumn.HeaderText = "WaiverSetDescription";
            this.waiverSetDescriptionDataGridViewTextBoxColumn.Name = "waiverSetDescriptionDataGridViewTextBoxColumn";
            this.waiverSetDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiverSetDescriptionDataGridViewTextBoxColumn.Visible = false;
            this.waiverSetDescriptionDataGridViewTextBoxColumn.Width = 292;
            // 
            // customerSignedWaiverDTOBindingSource
            // 
            this.customerSignedWaiverDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.Waivers.CustomerSignedWaiverDTO);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.bigVerticalScrollCustomerSignedWaiver);
            this.panel1.Controls.Add(this.bigHorizontalScrollCustomerSignedWaiver);
            this.panel1.Controls.Add(this.dgvCustomerSignedWaiver);
            this.panel1.Location = new System.Drawing.Point(33, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1014, 1211);
            this.panel1.TabIndex = 14;
            // 
            // bigVerticalScrollCustomerSignedWaiver
            // 
            this.bigVerticalScrollCustomerSignedWaiver.AutoHide = false;
            this.bigVerticalScrollCustomerSignedWaiver.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollCustomerSignedWaiver.DataGridView = this.dgvCustomerSignedWaiver;
            this.bigVerticalScrollCustomerSignedWaiver.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollCustomerSignedWaiver.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollCustomerSignedWaiver.Location = new System.Drawing.Point(928, 41);
            this.bigVerticalScrollCustomerSignedWaiver.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollCustomerSignedWaiver.Name = "bigVerticalScrollCustomerSignedWaiver";
            this.bigVerticalScrollCustomerSignedWaiver.ScrollableControl = null;
            this.bigVerticalScrollCustomerSignedWaiver.ScrollViewer = null;
            this.bigVerticalScrollCustomerSignedWaiver.Size = new System.Drawing.Size(63, 1141);
            this.bigVerticalScrollCustomerSignedWaiver.TabIndex = 20019;
            this.bigVerticalScrollCustomerSignedWaiver.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCustomerSignedWaiver.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollCustomerSignedWaiver.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollCustomerSignedWaiver.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // bigHorizontalScrollCustomerSignedWaiver
            // 
            this.bigHorizontalScrollCustomerSignedWaiver.AutoHide = false;
            this.bigHorizontalScrollCustomerSignedWaiver.BackColor = System.Drawing.SystemColors.Control;
            this.bigHorizontalScrollCustomerSignedWaiver.DataGridView = this.dgvCustomerSignedWaiver;
            this.bigHorizontalScrollCustomerSignedWaiver.LeftButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Left_Button;
            this.bigHorizontalScrollCustomerSignedWaiver.LeftButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Left_Button_Disabled;
            this.bigHorizontalScrollCustomerSignedWaiver.Location = new System.Drawing.Point(23, 1119);
            this.bigHorizontalScrollCustomerSignedWaiver.Margin = new System.Windows.Forms.Padding(0);
            this.bigHorizontalScrollCustomerSignedWaiver.Name = "bigHorizontalScrollCustomerSignedWaiver";
            this.bigHorizontalScrollCustomerSignedWaiver.RightButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Right_Button;
            this.bigHorizontalScrollCustomerSignedWaiver.RightButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Right_Button_Disabled;
            this.bigHorizontalScrollCustomerSignedWaiver.ScrollableControl = null;
            this.bigHorizontalScrollCustomerSignedWaiver.ScrollViewer = null;
            this.bigHorizontalScrollCustomerSignedWaiver.Size = new System.Drawing.Size(904, 63);
            this.bigHorizontalScrollCustomerSignedWaiver.TabIndex = 20020;
            this.bigHorizontalScrollCustomerSignedWaiver.LeftButtonClick += new System.EventHandler(this.LeftButtonClick);
            this.bigHorizontalScrollCustomerSignedWaiver.RightButtonClick += new System.EventHandler(this.RightButtonClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(38, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(530, 67);
            this.label1.TabIndex = 15;
            this.label1.Text = "View Signed Waivers";
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.ForeColor = System.Drawing.Color.White;
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(605, 1670);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(325, 164);
            this.btnView.TabIndex = 1027;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.White;
            this.lblCustomer.Location = new System.Drawing.Point(35, 201);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(1011, 46);
            this.lblCustomer.TabIndex = 1029;
            this.lblCustomer.Text = "Customer : ";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmCustomerSignedWaivers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCustomerSignedWaivers";
            this.Text = "Customer Signed Waivers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCustomerSignedWaivers_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnView, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.lblCustomer, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerSignedWaiver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerSignedWaiverDTOBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCustomerSignedWaiver;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnView;
        //private System.Windows.Forms.Button btnCancel; 
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCustomerSignedWaiver;
        private System.Windows.Forms.BindingSource customerSignedWaiverDTOBindingSource;
        private Semnox.Core.GenericUtilities.BigHorizontalScrollBarView bigHorizontalScrollCustomerSignedWaiver;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedWaiverFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSignedWaiverIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn WaiverName;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedByNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSignedWaiverHeaderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetDetailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedForDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedForNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deactivatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deactivationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deactivationApprovedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblCustomer;
    }
}