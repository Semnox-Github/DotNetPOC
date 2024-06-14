using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    partial class frmSelectWaiverOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblReservationCodeOR = new System.Windows.Forms.Label();
            this.txtReservationCode = new System.Windows.Forms.TextBox();
            this.lblReservationCode = new System.Windows.Forms.Label();
            this.dgvWaiverSet = new System.Windows.Forms.DataGridView();
            this.waiverSetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chooseWaiverSet = new System.Windows.Forms.DataGridViewImageColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedRecursiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.waiverSetDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblReservationCodeORBarOne = new System.Windows.Forms.Label();
            this.lblReservationCodeORBarTwo = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bigVerticalScrollWaiverSet = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.lblSelection = new System.Windows.Forms.Label();
            this.lblWaiver = new System.Windows.Forms.Label();
            this.pnlReservationCode = new System.Windows.Forms.Panel();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.btnProceed = new System.Windows.Forms.Button();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTrxOTP = new System.Windows.Forms.Label();
            this.txtTrxOTP = new System.Windows.Forms.TextBox();
            this.pnlTrxOTP = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waiverSetDTOBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlReservationCode.SuspendLayout();
            this.pnlTrxOTP.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1080, 86);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sign Waivers ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Gotham Rounded Bold", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(0, 786);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1080, 209);
            this.label7.TabIndex = 12;
            this.label7.Text = "Please pick the waiver option from below to proceed";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblReservationCodeOR
            // 
            this.lblReservationCodeOR.BackColor = System.Drawing.Color.Transparent;
            this.lblReservationCodeOR.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReservationCodeOR.ForeColor = System.Drawing.Color.White;
            this.lblReservationCodeOR.Location = new System.Drawing.Point(479, 455);
            this.lblReservationCodeOR.Name = "lblReservationCodeOR";
            this.lblReservationCodeOR.Size = new System.Drawing.Size(128, 78);
            this.lblReservationCodeOR.TabIndex = 11;
            this.lblReservationCodeOR.Text = "OR";
            this.lblReservationCodeOR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtReservationCode
            // 
            this.txtReservationCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtReservationCode.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReservationCode.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtReservationCode.Location = new System.Drawing.Point(26, 16);
            this.txtReservationCode.MaxLength = 10;
            this.txtReservationCode.Multiline = true;
            this.txtReservationCode.Name = "txtReservationCode";
            this.txtReservationCode.Size = new System.Drawing.Size(379, 46);
            this.txtReservationCode.TabIndex = 10;
            this.txtReservationCode.Click += new System.EventHandler(this.textReservationCodeBox_Enter);
            this.txtReservationCode.Enter += new System.EventHandler(this.textReservationCodeBox_Enter);
            // 
            // lblReservationCode
            // 
            this.lblReservationCode.BackColor = System.Drawing.Color.Transparent;
            this.lblReservationCode.Font = new System.Drawing.Font("Gotham Rounded Bold", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReservationCode.ForeColor = System.Drawing.Color.White;
            this.lblReservationCode.Location = new System.Drawing.Point(0, 294);
            this.lblReservationCode.Name = "lblReservationCode";
            this.lblReservationCode.Size = new System.Drawing.Size(1059, 75);
            this.lblReservationCode.TabIndex = 9;
            this.lblReservationCode.Text = "Do you have Reservation Code?";
            this.lblReservationCode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dgvWaiverSet
            // 
            this.dgvWaiverSet.AllowUserToAddRows = false;
            this.dgvWaiverSet.AutoGenerateColumns = false;
            this.dgvWaiverSet.BackgroundColor = System.Drawing.Color.White;
            this.dgvWaiverSet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvWaiverSet.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvWaiverSet.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWaiverSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvWaiverSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWaiverSet.ColumnHeadersVisible = false;
            this.dgvWaiverSet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.waiverSetIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.chooseWaiverSet,
            this.isActiveDataGridViewCheckBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.siteidDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.isChangedRecursiveDataGridViewCheckBoxColumn});
            this.dgvWaiverSet.DataSource = this.waiverSetDTOBindingSource;
            this.dgvWaiverSet.EnableHeadersVisualStyles = false;
            this.dgvWaiverSet.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvWaiverSet.Location = new System.Drawing.Point(14, 88);
            this.dgvWaiverSet.MultiSelect = false;
            this.dgvWaiverSet.Name = "dgvWaiverSet";
            this.dgvWaiverSet.ReadOnly = true;
            this.dgvWaiverSet.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvWaiverSet.RowHeadersVisible = false;
            this.dgvWaiverSet.RowTemplate.Height = 80;
            this.dgvWaiverSet.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvWaiverSet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvWaiverSet.Size = new System.Drawing.Size(814, 567);
            this.dgvWaiverSet.TabIndex = 13;
            this.dgvWaiverSet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWaiverSet_CellContentClick);
            // 
            // waiverSetIdDataGridViewTextBoxColumn
            // 
            this.waiverSetIdDataGridViewTextBoxColumn.DataPropertyName = "WaiverSetId";
            this.waiverSetIdDataGridViewTextBoxColumn.HeaderText = "WaiverSet Id";
            this.waiverSetIdDataGridViewTextBoxColumn.Name = "waiverSetIdDataGridViewTextBoxColumn";
            this.waiverSetIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiverSetIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 600;
            // 
            // chooseWaiverSet
            // 
            this.chooseWaiverSet.FillWeight = 60F;
            this.chooseWaiverSet.HeaderText = "Column1";
            this.chooseWaiverSet.Image = global::Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            this.chooseWaiverSet.Name = "chooseWaiverSet";
            this.chooseWaiverSet.ReadOnly = true;
            this.chooseWaiverSet.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chooseWaiverSet.Width = 250;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // siteidDataGridViewTextBoxColumn
            // 
            this.siteidDataGridViewTextBoxColumn.DataPropertyName = "Site_id";
            this.siteidDataGridViewTextBoxColumn.HeaderText = "Site Id";
            this.siteidDataGridViewTextBoxColumn.Name = "siteidDataGridViewTextBoxColumn";
            this.siteidDataGridViewTextBoxColumn.ReadOnly = true;
            this.siteidDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.ReadOnly = true;
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Name";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Visible = false;
            // 
            // isChangedRecursiveDataGridViewCheckBoxColumn
            // 
            this.isChangedRecursiveDataGridViewCheckBoxColumn.DataPropertyName = "IsChangedRecursive";
            this.isChangedRecursiveDataGridViewCheckBoxColumn.HeaderText = "IsChangedRecursive";
            this.isChangedRecursiveDataGridViewCheckBoxColumn.Name = "isChangedRecursiveDataGridViewCheckBoxColumn";
            this.isChangedRecursiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isChangedRecursiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // waiverSetDTOBindingSource
            // 
            this.waiverSetDTOBindingSource.DataSource = typeof(Semnox.Parafait.Waiver.WaiverSetDTO);
            // 
            // lblReservationCodeORBarOne
            // 
            this.lblReservationCodeORBarOne.BackColor = System.Drawing.Color.Transparent;
            this.lblReservationCodeORBarOne.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReservationCodeORBarOne.Location = new System.Drawing.Point(0, 494);
            this.lblReservationCodeORBarOne.Name = "lblReservationCodeORBarOne";
            this.lblReservationCodeORBarOne.Size = new System.Drawing.Size(476, 5);
            this.lblReservationCodeORBarOne.TabIndex = 16;
            this.lblReservationCodeORBarOne.Text = "label5";
            // 
            // lblReservationCodeORBarTwo
            // 
            this.lblReservationCodeORBarTwo.BackColor = System.Drawing.Color.Transparent;
            this.lblReservationCodeORBarTwo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReservationCodeORBarTwo.Location = new System.Drawing.Point(604, 494);
            this.lblReservationCodeORBarTwo.Name = "lblReservationCodeORBarTwo";
            this.lblReservationCodeORBarTwo.Size = new System.Drawing.Size(476, 5);
            this.lblReservationCodeORBarTwo.TabIndex = 17;
            this.lblReservationCodeORBarTwo.Text = "label6";
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1870);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.bigVerticalScrollWaiverSet);
            this.panel1.Controls.Add(this.lblSelection);
            this.panel1.Controls.Add(this.lblWaiver);
            this.panel1.Controls.Add(this.dgvWaiverSet);
            this.panel1.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(106, 930);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(886, 679);
            this.panel1.TabIndex = 163;
            // 
            // bigVerticalScrollWaiverSet
            // 
            this.bigVerticalScrollWaiverSet.AutoHide = false;
            this.bigVerticalScrollWaiverSet.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollWaiverSet.DataGridView = this.dgvWaiverSet;
            this.bigVerticalScrollWaiverSet.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollWaiverSet.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollWaiverSet.Location = new System.Drawing.Point(810, 88);
            this.bigVerticalScrollWaiverSet.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollWaiverSet.Name = "bigVerticalScrollWaiverSet";
            this.bigVerticalScrollWaiverSet.ScrollableControl = null;
            this.bigVerticalScrollWaiverSet.ScrollViewer = null;
            this.bigVerticalScrollWaiverSet.Size = new System.Drawing.Size(63, 567);
            this.bigVerticalScrollWaiverSet.TabIndex = 165;
            this.bigVerticalScrollWaiverSet.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollWaiverSet.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollWaiverSet.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollWaiverSet.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // lblSelection
            // 
            this.lblSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelection.AutoEllipsis = true;
            this.lblSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblSelection.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelection.ForeColor = System.Drawing.Color.Thistle;
            this.lblSelection.Location = new System.Drawing.Point(609, 0);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(215, 85);
            this.lblSelection.TabIndex = 164;
            this.lblSelection.Text = "Choose";
            this.lblSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWaiver
            // 
            this.lblWaiver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWaiver.AutoEllipsis = true;
            this.lblWaiver.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiver.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiver.ForeColor = System.Drawing.Color.Thistle;
            this.lblWaiver.Location = new System.Drawing.Point(3, 0);
            this.lblWaiver.Name = "lblWaiver";
            this.lblWaiver.Size = new System.Drawing.Size(600, 85);
            this.lblWaiver.TabIndex = 163;
            this.lblWaiver.Text = "Waivers";
            this.lblWaiver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlReservationCode
            // 
            this.pnlReservationCode.BackColor = System.Drawing.Color.Transparent;
            this.pnlReservationCode.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.pnlReservationCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlReservationCode.Controls.Add(this.txtReservationCode);
            this.pnlReservationCode.Location = new System.Drawing.Point(323, 372);
            this.pnlReservationCode.Name = "pnlReservationCode";
            this.pnlReservationCode.Size = new System.Drawing.Size(434, 84);
            this.pnlReservationCode.TabIndex = 1039;
            this.pnlReservationCode.Click += new System.EventHandler(this.panel2_Click);
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
            this.btnShowKeyPad.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_Kiosk.Properties.Resources.Keyboard_1;
            this.btnShowKeyPad.Location = new System.Drawing.Point(837, 367);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(87, 83);
            this.btnShowKeyPad.TabIndex = 20002;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeypad_Click);
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
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(605, 1670);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(325, 164);
            this.btnProceed.TabIndex = 1028;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.White;
            this.lblCustomer.Location = new System.Drawing.Point(35, 244);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(1024, 46);
            this.lblCustomer.TabIndex = 20003;
            this.lblCustomer.Text = "Customer :";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(604, 742);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(476, 5);
            this.label2.TabIndex = 20006;
            this.label2.Text = "label6";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(0, 742);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(476, 5);
            this.label3.TabIndex = 20005;
            this.label3.Text = "label5";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(479, 703);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 78);
            this.label4.TabIndex = 20004;
            this.label4.Text = "OR";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTrxOTP
            // 
            this.lblTrxOTP.BackColor = System.Drawing.Color.Transparent;
            this.lblTrxOTP.Font = new System.Drawing.Font("Gotham Rounded Bold", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrxOTP.ForeColor = System.Drawing.Color.White;
            this.lblTrxOTP.Location = new System.Drawing.Point(0, 539);
            this.lblTrxOTP.Name = "lblTrxOTP";
            this.lblTrxOTP.Size = new System.Drawing.Size(1059, 75);
            this.lblTrxOTP.TabIndex = 20007;
            this.lblTrxOTP.Text = "Do you have Transaction OTP?";
            this.lblTrxOTP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtTrxOTP
            // 
            this.txtTrxOTP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTrxOTP.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTrxOTP.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtTrxOTP.Location = new System.Drawing.Point(26, 16);
            this.txtTrxOTP.MaxLength = 10;
            this.txtTrxOTP.Multiline = true;
            this.txtTrxOTP.Name = "txtTrxOTP";
            this.txtTrxOTP.Size = new System.Drawing.Size(379, 46);
            this.txtTrxOTP.TabIndex = 10;
            this.txtTrxOTP.Click += new System.EventHandler(this.textTrxOTPBox_Enter);
            this.txtTrxOTP.Enter += new System.EventHandler(this.textTrxOTPBox_Enter);
            // 
            // pnlTrxOTP
            // 
            this.pnlTrxOTP.BackColor = System.Drawing.Color.Transparent;
            this.pnlTrxOTP.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.pnlTrxOTP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlTrxOTP.Controls.Add(this.txtTrxOTP);
            this.pnlTrxOTP.Location = new System.Drawing.Point(323, 617);
            this.pnlTrxOTP.Name = "pnlTrxOTP";
            this.pnlTrxOTP.Size = new System.Drawing.Size(434, 84);
            this.pnlTrxOTP.TabIndex = 20008;
            // 
            // frmSelectWaiverOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblTrxOTP);
            this.Controls.Add(this.pnlTrxOTP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.pnlReservationCode);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblReservationCodeORBarTwo);
            this.Controls.Add(this.lblReservationCodeORBarOne);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblReservationCodeOR);
            this.Controls.Add(this.lblReservationCode);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmSelectWaiverOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSelectWaiverOption";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelectWaiverOption_Closing);
            this.Load += new System.EventHandler(this.frmSelectWaiverOption_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblReservationCode, 0);
            this.Controls.SetChildIndex(this.lblReservationCodeOR, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.lblReservationCodeORBarOne, 0);
            this.Controls.SetChildIndex(this.lblReservationCodeORBarTwo, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.Controls.SetChildIndex(this.pnlReservationCode, 0);
            this.Controls.SetChildIndex(this.btnShowKeyPad, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.lblCustomer, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.pnlTrxOTP, 0);
            this.Controls.SetChildIndex(this.lblTrxOTP, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waiverSetDTOBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlReservationCode.ResumeLayout(false);
            this.pnlReservationCode.PerformLayout();
            this.pnlTrxOTP.ResumeLayout(false);
            this.pnlTrxOTP.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblReservationCodeOR;
        private System.Windows.Forms.TextBox txtReservationCode;
        private System.Windows.Forms.Label lblReservationCode;
        private System.Windows.Forms.DataGridView dgvWaiverSet;
        private System.Windows.Forms.BindingSource waiverSetDTOBindingSource;
        private System.Windows.Forms.Label lblReservationCodeORBarOne;
        private System.Windows.Forms.Label lblReservationCodeORBarTwo;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSelection;
        private System.Windows.Forms.Label lblWaiver;
        //private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlReservationCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn chooseWaiverSet;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedRecursiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Button btnProceed;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollWaiverSet;
        private Label lblCustomer;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblTrxOTP;
        private TextBox txtTrxOTP;
        private Panel pnlTrxOTP;
    }
}