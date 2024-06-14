using System;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    partial class frmRescheduleSummaryUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRescheduleSummaryUI));
            this.lblBookingProductLabel = new System.Windows.Forms.Label();
            this.lblBookingProductName = new System.Windows.Forms.Label();
            this.lblGuestQtyValue = new System.Windows.Forms.Label();
            this.lblGuestQtyLabel = new System.Windows.Forms.Label();
            this.lblOldFacilityMapValue = new System.Windows.Forms.Label();
            this.lblOldFacilityMapLabel = new System.Windows.Forms.Label();
            this.lblOldScheduleValue = new System.Windows.Forms.Label();
            this.lblOldScheduleLabel = new System.Windows.Forms.Label();
            this.lblNewFacilityMapValue = new System.Windows.Forms.Label();
            this.lblNewFacilityMapLabel = new System.Windows.Forms.Label();
            this.lblNewScheduleValue = new System.Windows.Forms.Label();
            this.lblNewScheduleLabel = new System.Windows.Forms.Label();
            this.pnlGrpPackageProducts = new System.Windows.Forms.Panel();
            this.vScrollPackgeProducts = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlPackageProduct = new System.Windows.Forms.FlowLayoutPanel();
            this.lblOverRide1 = new System.Windows.Forms.Label();
            this.lblCancel1 = new System.Windows.Forms.Label();
            this.lblRescheduleStatus1 = new System.Windows.Forms.Label();
            this.lblQty1 = new System.Windows.Forms.Label();
            this.lblProductName1 = new System.Windows.Forms.Label();
            this.pnlGrpAdditionalProducts = new System.Windows.Forms.Panel();
            this.vScrollAdditionalProducts = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlAdditionalPackageProduct = new System.Windows.Forms.FlowLayoutPanel();
            this.lblOverRide2 = new System.Windows.Forms.Label();
            this.lblCancel2 = new System.Windows.Forms.Label();
            this.lblRescheduleStatus2 = new System.Windows.Forms.Label();
            this.lblQty2 = new System.Windows.Forms.Label();
            this.lblProductName2 = new System.Windows.Forms.Label();
            this.pnlGrpAdditionalSlots = new System.Windows.Forms.Panel();
            this.vScrollAdditionalSlots = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlAdditionalSlots = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCancel3 = new System.Windows.Forms.Label();
            this.lblRescheduleStatus3 = new System.Windows.Forms.Label();
            this.lblProductName3 = new System.Windows.Forms.Label();
            this.lblDetailsDisplay = new System.Windows.Forms.Label();
            this.lblDetailsHeader = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReschedule = new System.Windows.Forms.Button();
            this.btnCloseDisplayPanel = new System.Windows.Forms.Button();
            this.pnlDetailsDisplay = new System.Windows.Forms.Panel();
            this.pnlDetailsDisplayChild = new System.Windows.Forms.Panel();
            this.fpnlMainPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeaderOne = new System.Windows.Forms.Panel();
            this.tpnlHeaderOne = new System.Windows.Forms.TableLayoutPanel();
            this.lblErrorInfo1 = new System.Windows.Forms.Label();
            this.pbxErrorInfo1 = new System.Windows.Forms.PictureBox();
            this.lblDescription1 = new System.Windows.Forms.Label();
            this.btnExpandCollapse1 = new System.Windows.Forms.Button();
            this.pnlHeaderTwo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDescription2 = new System.Windows.Forms.Label();
            this.pbxErrorInfo2 = new System.Windows.Forms.PictureBox();
            this.lblErrorInfo2 = new System.Windows.Forms.Label();
            this.btnExpandCollapse2 = new System.Windows.Forms.Button();
            this.pnlHeaderThree = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDescription3 = new System.Windows.Forms.Label();
            this.pbxErrorInfo3 = new System.Windows.Forms.PictureBox();
            this.lblErrorInfo3 = new System.Windows.Forms.Label();
            this.btnExpandCollapse3 = new System.Windows.Forms.Button();
            this.pnlGrpPackageProducts.SuspendLayout();
            this.pnlGrpAdditionalProducts.SuspendLayout();
            this.pnlGrpAdditionalSlots.SuspendLayout();
            this.pnlDetailsDisplay.SuspendLayout();
            this.pnlDetailsDisplayChild.SuspendLayout();
            this.fpnlMainPanel.SuspendLayout();
            this.pnlHeaderOne.SuspendLayout();
            this.tpnlHeaderOne.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxErrorInfo1)).BeginInit();
            this.pnlHeaderTwo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxErrorInfo2)).BeginInit();
            this.pnlHeaderThree.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxErrorInfo3)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBookingProductLabel
            // 
            this.lblBookingProductLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookingProductLabel.Location = new System.Drawing.Point(5, 2);
            this.lblBookingProductLabel.Name = "lblBookingProductLabel";
            this.lblBookingProductLabel.Size = new System.Drawing.Size(117, 35);
            this.lblBookingProductLabel.TabIndex = 109;
            this.lblBookingProductLabel.Text = "Booking Product:";
            this.lblBookingProductLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBookingProductName
            // 
            this.lblBookingProductName.Location = new System.Drawing.Point(120, 12);
            this.lblBookingProductName.MaximumSize = new System.Drawing.Size(350, 15);
            this.lblBookingProductName.Name = "lblBookingProductName";
            this.lblBookingProductName.Size = new System.Drawing.Size(300, 15);
            this.lblBookingProductName.TabIndex = 111;
            this.lblBookingProductName.Text = "Booking Product:";
            this.lblBookingProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGuestQtyValue
            // 
            this.lblGuestQtyValue.AutoSize = true;
            this.lblGuestQtyValue.Location = new System.Drawing.Point(599, 12);
            this.lblGuestQtyValue.Name = "lblGuestQtyValue";
            this.lblGuestQtyValue.Size = new System.Drawing.Size(14, 15);
            this.lblGuestQtyValue.TabIndex = 113;
            this.lblGuestQtyValue.Text = "0";
            this.lblGuestQtyValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGuestQtyLabel
            // 
            this.lblGuestQtyLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuestQtyLabel.Location = new System.Drawing.Point(484, 2);
            this.lblGuestQtyLabel.Name = "lblGuestQtyLabel";
            this.lblGuestQtyLabel.Size = new System.Drawing.Size(117, 35);
            this.lblGuestQtyLabel.TabIndex = 112;
            this.lblGuestQtyLabel.Text = "Guest Quantity:";
            this.lblGuestQtyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOldFacilityMapValue
            // 
            this.lblOldFacilityMapValue.AutoSize = true;
            this.lblOldFacilityMapValue.Location = new System.Drawing.Point(599, 44);
            this.lblOldFacilityMapValue.Name = "lblOldFacilityMapValue";
            this.lblOldFacilityMapValue.Size = new System.Drawing.Size(14, 15);
            this.lblOldFacilityMapValue.TabIndex = 117;
            this.lblOldFacilityMapValue.Text = "0";
            this.lblOldFacilityMapValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOldFacilityMapLabel
            // 
            this.lblOldFacilityMapLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldFacilityMapLabel.Location = new System.Drawing.Point(484, 34);
            this.lblOldFacilityMapLabel.Name = "lblOldFacilityMapLabel";
            this.lblOldFacilityMapLabel.Size = new System.Drawing.Size(117, 35);
            this.lblOldFacilityMapLabel.TabIndex = 116;
            this.lblOldFacilityMapLabel.Text = "Old Facility Map:";
            this.lblOldFacilityMapLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOldScheduleValue
            // 
            this.lblOldScheduleValue.AutoSize = true;
            this.lblOldScheduleValue.Location = new System.Drawing.Point(120, 44);
            this.lblOldScheduleValue.Name = "lblOldScheduleValue";
            this.lblOldScheduleValue.Size = new System.Drawing.Size(302, 15);
            this.lblOldScheduleValue.TabIndex = 115;
            this.lblOldScheduleValue.Text = "30-May-2020 10:00:00 AM to 30-May-2020 11:00:00 AM";
            this.lblOldScheduleValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOldScheduleLabel
            // 
            this.lblOldScheduleLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldScheduleLabel.Location = new System.Drawing.Point(5, 34);
            this.lblOldScheduleLabel.Name = "lblOldScheduleLabel";
            this.lblOldScheduleLabel.Size = new System.Drawing.Size(117, 35);
            this.lblOldScheduleLabel.TabIndex = 114;
            this.lblOldScheduleLabel.Text = "Old Schedule:";
            this.lblOldScheduleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNewFacilityMapValue
            // 
            this.lblNewFacilityMapValue.AutoSize = true;
            this.lblNewFacilityMapValue.Location = new System.Drawing.Point(599, 76);
            this.lblNewFacilityMapValue.Name = "lblNewFacilityMapValue";
            this.lblNewFacilityMapValue.Size = new System.Drawing.Size(14, 15);
            this.lblNewFacilityMapValue.TabIndex = 121;
            this.lblNewFacilityMapValue.Text = "0";
            this.lblNewFacilityMapValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewFacilityMapLabel
            // 
            this.lblNewFacilityMapLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewFacilityMapLabel.Location = new System.Drawing.Point(484, 66);
            this.lblNewFacilityMapLabel.Name = "lblNewFacilityMapLabel";
            this.lblNewFacilityMapLabel.Size = new System.Drawing.Size(117, 35);
            this.lblNewFacilityMapLabel.TabIndex = 120;
            this.lblNewFacilityMapLabel.Text = "New Facility Map:";
            this.lblNewFacilityMapLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNewScheduleValue
            // 
            this.lblNewScheduleValue.AutoSize = true;
            this.lblNewScheduleValue.Location = new System.Drawing.Point(120, 76);
            this.lblNewScheduleValue.Name = "lblNewScheduleValue";
            this.lblNewScheduleValue.Size = new System.Drawing.Size(302, 15);
            this.lblNewScheduleValue.TabIndex = 119;
            this.lblNewScheduleValue.Text = "30-May-2020 10:00:00 AM to 30-May-2020 11:00:00 AM";
            this.lblNewScheduleValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewScheduleLabel
            // 
            this.lblNewScheduleLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewScheduleLabel.Location = new System.Drawing.Point(5, 66);
            this.lblNewScheduleLabel.Name = "lblNewScheduleLabel";
            this.lblNewScheduleLabel.Size = new System.Drawing.Size(117, 35);
            this.lblNewScheduleLabel.TabIndex = 118;
            this.lblNewScheduleLabel.Text = "New Schedule:";
            this.lblNewScheduleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlGrpPackageProducts
            // 
            this.pnlGrpPackageProducts.BackColor = System.Drawing.Color.Transparent;
            this.pnlGrpPackageProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrpPackageProducts.Controls.Add(this.vScrollPackgeProducts);
            this.pnlGrpPackageProducts.Controls.Add(this.pnlPackageProduct);
            this.pnlGrpPackageProducts.Controls.Add(this.lblOverRide1);
            this.pnlGrpPackageProducts.Controls.Add(this.lblCancel1);
            this.pnlGrpPackageProducts.Controls.Add(this.lblRescheduleStatus1);
            this.pnlGrpPackageProducts.Controls.Add(this.lblQty1);
            this.pnlGrpPackageProducts.Controls.Add(this.lblProductName1);
            this.pnlGrpPackageProducts.Location = new System.Drawing.Point(3, 43);
            this.pnlGrpPackageProducts.Name = "pnlGrpPackageProducts";
            this.pnlGrpPackageProducts.Size = new System.Drawing.Size(952, 238);
            this.pnlGrpPackageProducts.TabIndex = 122;
            // 
            // vScrollPackgeProducts
            // 
            this.vScrollPackgeProducts.AutoHide = false;
            this.vScrollPackgeProducts.DataGridView = null;
            this.vScrollPackgeProducts.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollPackgeProducts.DownButtonBackgroundImage")));
            this.vScrollPackgeProducts.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollPackgeProducts.DownButtonDisabledBackgroundImage")));
            this.vScrollPackgeProducts.Location = new System.Drawing.Point(882, 21);
            this.vScrollPackgeProducts.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollPackgeProducts.Name = "vScrollPackgeProducts";
            this.vScrollPackgeProducts.ScrollableControl = this.pnlPackageProduct;
            this.vScrollPackgeProducts.ScrollViewer = null;
            this.vScrollPackgeProducts.Size = new System.Drawing.Size(40, 212);
            this.vScrollPackgeProducts.TabIndex = 116;
            this.vScrollPackgeProducts.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollPackgeProducts.UpButtonBackgroundImage")));
            this.vScrollPackgeProducts.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollPackgeProducts.UpButtonDisabledBackgroundImage")));
            this.vScrollPackgeProducts.UpButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.vScrollPackgeProducts.DownButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // pnlPackageProduct
            // 
            this.pnlPackageProduct.AutoScroll = true;
            this.pnlPackageProduct.Location = new System.Drawing.Point(2, 21);
            this.pnlPackageProduct.Name = "pnlPackageProduct";
            this.pnlPackageProduct.Size = new System.Drawing.Size(900, 212);
            this.pnlPackageProduct.TabIndex = 115;
            // 
            // lblOverRide1
            // 
            this.lblOverRide1.BackColor = System.Drawing.Color.Transparent;
            this.lblOverRide1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverRide1.Location = new System.Drawing.Point(800, -5);
            this.lblOverRide1.Name = "lblOverRide1";
            this.lblOverRide1.Size = new System.Drawing.Size(80, 30);
            this.lblOverRide1.TabIndex = 114;
            this.lblOverRide1.Text = "Override?";
            this.lblOverRide1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCancel1
            // 
            this.lblCancel1.BackColor = System.Drawing.Color.Transparent;
            this.lblCancel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancel1.Location = new System.Drawing.Point(735, -5);
            this.lblCancel1.Name = "lblCancel1";
            this.lblCancel1.Size = new System.Drawing.Size(80, 30);
            this.lblCancel1.TabIndex = 113;
            this.lblCancel1.Text = "Cancel?";
            this.lblCancel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRescheduleStatus1
            // 
            this.lblRescheduleStatus1.BackColor = System.Drawing.Color.Transparent;
            this.lblRescheduleStatus1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRescheduleStatus1.Location = new System.Drawing.Point(525, -5);
            this.lblRescheduleStatus1.Name = "lblRescheduleStatus1";
            this.lblRescheduleStatus1.Size = new System.Drawing.Size(117, 30);
            this.lblRescheduleStatus1.TabIndex = 112;
            this.lblRescheduleStatus1.Text = "Reschedule Status";
            this.lblRescheduleStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblQty1
            // 
            this.lblQty1.BackColor = System.Drawing.Color.Transparent;
            this.lblQty1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQty1.Location = new System.Drawing.Point(345, -5);
            this.lblQty1.Name = "lblQty1";
            this.lblQty1.Size = new System.Drawing.Size(80, 30);
            this.lblQty1.TabIndex = 111;
            this.lblQty1.Text = "Quantity";
            this.lblQty1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductName1
            // 
            this.lblProductName1.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName1.Location = new System.Drawing.Point(110, -5);
            this.lblProductName1.Name = "lblProductName1";
            this.lblProductName1.Size = new System.Drawing.Size(117, 30);
            this.lblProductName1.TabIndex = 110;
            this.lblProductName1.Text = "Product Name";
            this.lblProductName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlGrpAdditionalProducts
            // 
            this.pnlGrpAdditionalProducts.BackColor = System.Drawing.Color.Transparent;
            this.pnlGrpAdditionalProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrpAdditionalProducts.Controls.Add(this.vScrollAdditionalProducts);
            this.pnlGrpAdditionalProducts.Controls.Add(this.pnlAdditionalPackageProduct);
            this.pnlGrpAdditionalProducts.Controls.Add(this.lblOverRide2);
            this.pnlGrpAdditionalProducts.Controls.Add(this.lblCancel2);
            this.pnlGrpAdditionalProducts.Controls.Add(this.lblRescheduleStatus2);
            this.pnlGrpAdditionalProducts.Controls.Add(this.lblQty2);
            this.pnlGrpAdditionalProducts.Controls.Add(this.lblProductName2);
            this.pnlGrpAdditionalProducts.Location = new System.Drawing.Point(3, 327);
            this.pnlGrpAdditionalProducts.Name = "pnlGrpAdditionalProducts";
            this.pnlGrpAdditionalProducts.Size = new System.Drawing.Size(952, 330);
            this.pnlGrpAdditionalProducts.TabIndex = 123;
            // 
            // vScrollAdditionalProducts
            // 
            this.vScrollAdditionalProducts.AutoHide = false;
            this.vScrollAdditionalProducts.DataGridView = null;
            this.vScrollAdditionalProducts.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalProducts.DownButtonBackgroundImage")));
            this.vScrollAdditionalProducts.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalProducts.DownButtonDisabledBackgroundImage")));
            this.vScrollAdditionalProducts.Location = new System.Drawing.Point(882, 19);
            this.vScrollAdditionalProducts.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollAdditionalProducts.Name = "vScrollAdditionalProducts";
            this.vScrollAdditionalProducts.ScrollableControl = this.pnlAdditionalPackageProduct;
            this.vScrollAdditionalProducts.ScrollViewer = null;
            this.vScrollAdditionalProducts.Size = new System.Drawing.Size(40, 306);
            this.vScrollAdditionalProducts.TabIndex = 116;
            this.vScrollAdditionalProducts.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalProducts.UpButtonBackgroundImage")));
            this.vScrollAdditionalProducts.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalProducts.UpButtonDisabledBackgroundImage")));
            this.vScrollAdditionalProducts.UpButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.vScrollAdditionalProducts.DownButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // pnlAdditionalPackageProduct
            // 
            this.pnlAdditionalPackageProduct.AutoScroll = true;
            this.pnlAdditionalPackageProduct.Location = new System.Drawing.Point(2, 21);
            this.pnlAdditionalPackageProduct.Name = "pnlAdditionalPackageProduct";
            this.pnlAdditionalPackageProduct.Size = new System.Drawing.Size(900, 306);
            this.pnlAdditionalPackageProduct.TabIndex = 115;
            // 
            // lblOverRide2
            // 
            this.lblOverRide2.BackColor = System.Drawing.Color.Transparent;
            this.lblOverRide2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverRide2.Location = new System.Drawing.Point(800, -5);
            this.lblOverRide2.Name = "lblOverRide2";
            this.lblOverRide2.Size = new System.Drawing.Size(80, 30);
            this.lblOverRide2.TabIndex = 114;
            this.lblOverRide2.Text = "Override?";
            this.lblOverRide2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCancel2
            // 
            this.lblCancel2.BackColor = System.Drawing.Color.Transparent;
            this.lblCancel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancel2.Location = new System.Drawing.Point(735, -5);
            this.lblCancel2.Name = "lblCancel2";
            this.lblCancel2.Size = new System.Drawing.Size(80, 30);
            this.lblCancel2.TabIndex = 113;
            this.lblCancel2.Text = "Cancel?";
            this.lblCancel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRescheduleStatus2
            // 
            this.lblRescheduleStatus2.BackColor = System.Drawing.Color.Transparent;
            this.lblRescheduleStatus2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRescheduleStatus2.Location = new System.Drawing.Point(525, -5);
            this.lblRescheduleStatus2.Name = "lblRescheduleStatus2";
            this.lblRescheduleStatus2.Size = new System.Drawing.Size(117, 30);
            this.lblRescheduleStatus2.TabIndex = 112;
            this.lblRescheduleStatus2.Text = "Reschedule Status";
            this.lblRescheduleStatus2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblQty2
            // 
            this.lblQty2.BackColor = System.Drawing.Color.Transparent;
            this.lblQty2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQty2.Location = new System.Drawing.Point(345, -5);
            this.lblQty2.Name = "lblQty2";
            this.lblQty2.Size = new System.Drawing.Size(80, 30);
            this.lblQty2.TabIndex = 111;
            this.lblQty2.Text = "Quantity";
            this.lblQty2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductName2
            // 
            this.lblProductName2.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName2.Location = new System.Drawing.Point(110, -5);
            this.lblProductName2.Name = "lblProductName2";
            this.lblProductName2.Size = new System.Drawing.Size(117, 30);
            this.lblProductName2.TabIndex = 110;
            this.lblProductName2.Text = "Product Name";
            this.lblProductName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlGrpAdditionalSlots
            // 
            this.pnlGrpAdditionalSlots.BackColor = System.Drawing.Color.Transparent;
            this.pnlGrpAdditionalSlots.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrpAdditionalSlots.Controls.Add(this.vScrollAdditionalSlots);
            this.pnlGrpAdditionalSlots.Controls.Add(this.pnlAdditionalSlots);
            this.pnlGrpAdditionalSlots.Controls.Add(this.lblCancel3);
            this.pnlGrpAdditionalSlots.Controls.Add(this.lblRescheduleStatus3);
            this.pnlGrpAdditionalSlots.Controls.Add(this.lblProductName3);
            this.pnlGrpAdditionalSlots.Location = new System.Drawing.Point(3, 703);
            this.pnlGrpAdditionalSlots.Name = "pnlGrpAdditionalSlots";
            this.pnlGrpAdditionalSlots.Size = new System.Drawing.Size(952, 127);
            this.pnlGrpAdditionalSlots.TabIndex = 124;
            // 
            // vScrollAdditionalSlots
            // 
            this.vScrollAdditionalSlots.AutoHide = false;
            this.vScrollAdditionalSlots.DataGridView = null;
            this.vScrollAdditionalSlots.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalSlots.DownButtonBackgroundImage")));
            this.vScrollAdditionalSlots.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalSlots.DownButtonDisabledBackgroundImage")));
            this.vScrollAdditionalSlots.Location = new System.Drawing.Point(882, 29);
            this.vScrollAdditionalSlots.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollAdditionalSlots.Name = "vScrollAdditionalSlots";
            this.vScrollAdditionalSlots.ScrollableControl = this.pnlAdditionalSlots;
            this.vScrollAdditionalSlots.ScrollViewer = null;
            this.vScrollAdditionalSlots.Size = new System.Drawing.Size(40, 92);
            this.vScrollAdditionalSlots.TabIndex = 116;
            this.vScrollAdditionalSlots.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalSlots.UpButtonBackgroundImage")));
            this.vScrollAdditionalSlots.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollAdditionalSlots.UpButtonDisabledBackgroundImage")));
            // 
            // pnlAdditionalSlots
            // 
            this.pnlAdditionalSlots.AutoScroll = true;
            this.pnlAdditionalSlots.Location = new System.Drawing.Point(2, 31);
            this.pnlAdditionalSlots.Name = "pnlAdditionalSlots";
            this.pnlAdditionalSlots.Size = new System.Drawing.Size(900, 92);
            this.pnlAdditionalSlots.TabIndex = 115;
            // 
            // lblCancel3
            // 
            this.lblCancel3.BackColor = System.Drawing.Color.Transparent;
            this.lblCancel3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancel3.Location = new System.Drawing.Point(735, 2);
            this.lblCancel3.Name = "lblCancel3";
            this.lblCancel3.Size = new System.Drawing.Size(80, 35);
            this.lblCancel3.TabIndex = 113;
            this.lblCancel3.Text = "Cancel?";
            this.lblCancel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRescheduleStatus3
            // 
            this.lblRescheduleStatus3.BackColor = System.Drawing.Color.Transparent;
            this.lblRescheduleStatus3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRescheduleStatus3.Location = new System.Drawing.Point(525, 2);
            this.lblRescheduleStatus3.Name = "lblRescheduleStatus3";
            this.lblRescheduleStatus3.Size = new System.Drawing.Size(117, 35);
            this.lblRescheduleStatus3.TabIndex = 112;
            this.lblRescheduleStatus3.Text = "Reschedule Status";
            this.lblRescheduleStatus3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductName3
            // 
            this.lblProductName3.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName3.Location = new System.Drawing.Point(60, 2);
            this.lblProductName3.Name = "lblProductName3";
            this.lblProductName3.Size = new System.Drawing.Size(224, 35);
            this.lblProductName3.TabIndex = 110;
            this.lblProductName3.Text = "Additional Reservation Slots";
            this.lblProductName3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDetailsDisplay
            // 
            this.lblDetailsDisplay.AutoSize = true;
            this.lblDetailsDisplay.BackColor = System.Drawing.Color.White;
            this.lblDetailsDisplay.Location = new System.Drawing.Point(2, 2);
            this.lblDetailsDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.lblDetailsDisplay.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblDetailsDisplay.Name = "lblDetailsDisplay";
            this.lblDetailsDisplay.Size = new System.Drawing.Size(0, 15);
            this.lblDetailsDisplay.TabIndex = 126;
            // 
            // lblDetailsHeader
            // 
            this.lblDetailsHeader.BackColor = this.BackColor;
            this.lblDetailsHeader.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsHeader.Location = new System.Drawing.Point(4, 3);
            this.lblDetailsHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetailsHeader.Name = "lblDetailsHeader";
            this.lblDetailsHeader.Size = new System.Drawing.Size(340, 28);
            this.lblDetailsHeader.TabIndex = 126;
            this.lblDetailsHeader.Text = "Reschedule Status";
            this.lblDetailsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(529, 581);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 34);
            this.btnCancel.TabIndex = 126;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBlack_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBlack_MouseUp);
            // 
            // btnReschedule
            // 
            this.btnReschedule.BackColor = System.Drawing.Color.Transparent;
            this.btnReschedule.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnReschedule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReschedule.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnReschedule.FlatAppearance.BorderSize = 0;
            this.btnReschedule.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnReschedule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReschedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReschedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReschedule.ForeColor = System.Drawing.Color.White;
            this.btnReschedule.Location = new System.Drawing.Point(383, 580);
            this.btnReschedule.Name = "btnReschedule";
            this.btnReschedule.Size = new System.Drawing.Size(104, 36);
            this.btnReschedule.TabIndex = 125;
            this.btnReschedule.Text = "Reschedule";
            this.btnReschedule.UseVisualStyleBackColor = false;
            this.btnReschedule.Click += new System.EventHandler(this.btnReschedule_Click);
            this.btnReschedule.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBlack_MouseDown);
            this.btnReschedule.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBlack_MouseUp);
            // 
            // btnCloseDisplayPanel
            // 
            this.btnCloseDisplayPanel.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseDisplayPanel.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Remove_Btn_Normal;
            this.btnCloseDisplayPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseDisplayPanel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCloseDisplayPanel.FlatAppearance.BorderSize = 0;
            this.btnCloseDisplayPanel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCloseDisplayPanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseDisplayPanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseDisplayPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseDisplayPanel.ForeColor = System.Drawing.Color.White;
            this.btnCloseDisplayPanel.Location = new System.Drawing.Point(322, 3);
            this.btnCloseDisplayPanel.Name = "btnCloseDisplayPanel";
            this.btnCloseDisplayPanel.Size = new System.Drawing.Size(25, 25);
            this.btnCloseDisplayPanel.TabIndex = 126;
            this.btnCloseDisplayPanel.UseVisualStyleBackColor = false;
            this.btnCloseDisplayPanel.Click += new System.EventHandler(this.pnlDetailsDisplayLostFocus);
            this.btnCloseDisplayPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCloseDisplayPanel_MouseDown);
            this.btnCloseDisplayPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCloseDisplayPanel_MouseUp);
            // 
            // pnlDetailsDisplay
            // 
            this.pnlDetailsDisplay.BackColor = System.Drawing.Color.White;
            this.pnlDetailsDisplay.BackgroundImage = global::Parafait_POS.Properties.Resources.WhiteBox400x400;
            this.pnlDetailsDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDetailsDisplay.Controls.Add(this.btnCloseDisplayPanel);
            this.pnlDetailsDisplay.Controls.Add(this.pnlDetailsDisplayChild);
            this.pnlDetailsDisplay.Controls.Add(this.lblDetailsHeader);
            this.pnlDetailsDisplay.Location = new System.Drawing.Point(629, 2);
            this.pnlDetailsDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.pnlDetailsDisplay.Name = "pnlDetailsDisplay";
            this.pnlDetailsDisplay.Size = new System.Drawing.Size(350, 220);
            this.pnlDetailsDisplay.TabIndex = 127;
            this.pnlDetailsDisplay.Visible = false;
            this.pnlDetailsDisplay.Leave += new System.EventHandler(this.pnlDetailsDisplayLostFocus);
            // 
            // pnlDetailsDisplayChild
            // 
            this.pnlDetailsDisplayChild.AutoSize = true;
            this.pnlDetailsDisplayChild.BackColor = System.Drawing.Color.White;
            this.pnlDetailsDisplayChild.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetailsDisplayChild.Controls.Add(this.lblDetailsDisplay);
            this.pnlDetailsDisplayChild.Location = new System.Drawing.Point(3, 32);
            this.pnlDetailsDisplayChild.Margin = new System.Windows.Forms.Padding(2);
            this.pnlDetailsDisplayChild.Name = "pnlDetailsDisplayChild";
            this.pnlDetailsDisplayChild.Size = new System.Drawing.Size(350, 184);
            this.pnlDetailsDisplayChild.TabIndex = 127;
            this.pnlDetailsDisplayChild.Click += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // fpnlMainPanel
            // 
            this.fpnlMainPanel.Controls.Add(this.pnlHeaderOne);
            this.fpnlMainPanel.Controls.Add(this.pnlGrpPackageProducts);
            this.fpnlMainPanel.Controls.Add(this.pnlHeaderTwo);
            this.fpnlMainPanel.Controls.Add(this.pnlGrpAdditionalProducts);
            this.fpnlMainPanel.Controls.Add(this.pnlHeaderThree);
            this.fpnlMainPanel.Controls.Add(this.pnlGrpAdditionalSlots);
            this.fpnlMainPanel.Location = new System.Drawing.Point(20, 107);
            this.fpnlMainPanel.Name = "fpnlMainPanel";
            this.fpnlMainPanel.Size = new System.Drawing.Size(958, 450);
            this.fpnlMainPanel.TabIndex = 117;
            // 
            // pnlHeaderOne
            // 
            this.pnlHeaderOne.BackColor = System.Drawing.Color.LightBlue;
            this.pnlHeaderOne.Controls.Add(this.tpnlHeaderOne);
            this.pnlHeaderOne.Controls.Add(this.btnExpandCollapse1);
            this.pnlHeaderOne.ForeColor = System.Drawing.Color.Black;
            this.pnlHeaderOne.Location = new System.Drawing.Point(3, 3);
            this.pnlHeaderOne.Name = "pnlHeaderOne";
            this.pnlHeaderOne.Size = new System.Drawing.Size(958, 34);
            this.pnlHeaderOne.TabIndex = 117;
            // 
            // tpnlHeaderOne
            // 
            this.tpnlHeaderOne.AutoSize = true;
            this.tpnlHeaderOne.ColumnCount = 3;
            this.tpnlHeaderOne.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnlHeaderOne.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnlHeaderOne.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnlHeaderOne.Controls.Add(this.lblErrorInfo1, 2, 0);
            this.tpnlHeaderOne.Controls.Add(this.pbxErrorInfo1, 1, 0);
            this.tpnlHeaderOne.Controls.Add(this.lblDescription1, 0, 0);
            this.tpnlHeaderOne.Location = new System.Drawing.Point(3, 3);
            this.tpnlHeaderOne.Name = "tpnlHeaderOne";
            this.tpnlHeaderOne.RowCount = 1;
            this.tpnlHeaderOne.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpnlHeaderOne.Size = new System.Drawing.Size(503, 38);
            this.tpnlHeaderOne.TabIndex = 130;
            // 
            // lblErrorInfo1
            // 
            this.lblErrorInfo1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorInfo1.Location = new System.Drawing.Point(250, 0);
            this.lblErrorInfo1.Name = "lblErrorInfo1";
            this.lblErrorInfo1.Size = new System.Drawing.Size(250, 35);
            this.lblErrorInfo1.TabIndex = 128;
            this.lblErrorInfo1.Text = " (0) ";
            this.lblErrorInfo1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbxErrorInfo1
            // 
            this.pbxErrorInfo1.BackgroundImage = global::Parafait_POS.Properties.Resources.WarningTriangleIcon;
            this.pbxErrorInfo1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxErrorInfo1.Location = new System.Drawing.Point(209, 3);
            this.pbxErrorInfo1.Name = "pbxErrorInfo1";
            this.pbxErrorInfo1.Size = new System.Drawing.Size(35, 30);
            this.pbxErrorInfo1.TabIndex = 127;
            this.pbxErrorInfo1.TabStop = false;
            // 
            // lblDescription1
            // 
            this.lblDescription1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription1.Location = new System.Drawing.Point(3, 0);
            this.lblDescription1.Name = "lblDescription1";
            this.lblDescription1.Size = new System.Drawing.Size(200, 35);
            this.lblDescription1.TabIndex = 127;
            this.lblDescription1.Text = "Package Products:";
            this.lblDescription1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExpandCollapse1
            // 
            this.btnExpandCollapse1.BackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExpandCollapse1.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExpandCollapse1.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapse1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapse1.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapse1.Image = global::Parafait_POS.Properties.Resources.CollapseArrow;
            this.btnExpandCollapse1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapse1.Location = new System.Drawing.Point(642, 0);
            this.btnExpandCollapse1.Name = "btnExpandCollapse1";
            this.btnExpandCollapse1.Size = new System.Drawing.Size(317, 34);
            this.btnExpandCollapse1.TabIndex = 129;
            this.btnExpandCollapse1.Tag = "";
            this.btnExpandCollapse1.UseVisualStyleBackColor = false;
            this.btnExpandCollapse1.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // pnlHeaderTwo
            // 
            this.pnlHeaderTwo.BackColor = System.Drawing.Color.LightBlue;
            this.pnlHeaderTwo.Controls.Add(this.tableLayoutPanel1);
            this.pnlHeaderTwo.Controls.Add(this.btnExpandCollapse2);
            this.pnlHeaderTwo.ForeColor = System.Drawing.Color.Black;
            this.pnlHeaderTwo.Location = new System.Drawing.Point(3, 287);
            this.pnlHeaderTwo.Name = "pnlHeaderTwo";
            this.pnlHeaderTwo.Size = new System.Drawing.Size(958, 34);
            this.pnlHeaderTwo.TabIndex = 125;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblDescription2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbxErrorInfo2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblErrorInfo2, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(503, 38);
            this.tableLayoutPanel1.TabIndex = 131;
            // 
            // lblDescription2
            // 
            this.lblDescription2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription2.Location = new System.Drawing.Point(3, 0);
            this.lblDescription2.Name = "lblDescription2";
            this.lblDescription2.Size = new System.Drawing.Size(200, 35);
            this.lblDescription2.TabIndex = 127;
            this.lblDescription2.Text = "Additional Products:";
            this.lblDescription2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbxErrorInfo2
            // 
            this.pbxErrorInfo2.BackgroundImage = global::Parafait_POS.Properties.Resources.WarningTriangleIcon;
            this.pbxErrorInfo2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxErrorInfo2.Location = new System.Drawing.Point(209, 3);
            this.pbxErrorInfo2.Name = "pbxErrorInfo2";
            this.pbxErrorInfo2.Size = new System.Drawing.Size(35, 30);
            this.pbxErrorInfo2.TabIndex = 127;
            this.pbxErrorInfo2.TabStop = false;
            // 
            // lblErrorInfo2
            // 
            this.lblErrorInfo2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorInfo2.Location = new System.Drawing.Point(250, 0);
            this.lblErrorInfo2.Name = "lblErrorInfo2";
            this.lblErrorInfo2.Size = new System.Drawing.Size(250, 35);
            this.lblErrorInfo2.TabIndex = 128;
            this.lblErrorInfo2.Text = " (0) ";
            this.lblErrorInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExpandCollapse2
            // 
            this.btnExpandCollapse2.BackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExpandCollapse2.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExpandCollapse2.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapse2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapse2.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapse2.Image = global::Parafait_POS.Properties.Resources.ExpandArrow;
            this.btnExpandCollapse2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapse2.Location = new System.Drawing.Point(643, 0);
            this.btnExpandCollapse2.Name = "btnExpandCollapse2";
            this.btnExpandCollapse2.Size = new System.Drawing.Size(317, 34);
            this.btnExpandCollapse2.TabIndex = 130;
            this.btnExpandCollapse2.Tag = "";
            this.btnExpandCollapse2.UseVisualStyleBackColor = false;
            this.btnExpandCollapse2.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // pnlHeaderThree
            // 
            this.pnlHeaderThree.BackColor = System.Drawing.Color.LightBlue;
            this.pnlHeaderThree.Controls.Add(this.tableLayoutPanel2);
            this.pnlHeaderThree.Controls.Add(this.btnExpandCollapse3);
            this.pnlHeaderThree.ForeColor = System.Drawing.Color.Black;
            this.pnlHeaderThree.Location = new System.Drawing.Point(3, 663);
            this.pnlHeaderThree.Name = "pnlHeaderThree";
            this.pnlHeaderThree.Size = new System.Drawing.Size(958, 34);
            this.pnlHeaderThree.TabIndex = 126;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblDescription3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pbxErrorInfo3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblErrorInfo3, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(503, 38);
            this.tableLayoutPanel2.TabIndex = 133;
            // 
            // lblDescription3
            // 
            this.lblDescription3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription3.Location = new System.Drawing.Point(3, 0);
            this.lblDescription3.Name = "lblDescription3";
            this.lblDescription3.Size = new System.Drawing.Size(200, 35);
            this.lblDescription3.TabIndex = 127;
            this.lblDescription3.Text = "Additional Reservation Slots:";
            this.lblDescription3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbxErrorInfo3
            // 
            this.pbxErrorInfo3.BackgroundImage = global::Parafait_POS.Properties.Resources.WarningTriangleIcon;
            this.pbxErrorInfo3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxErrorInfo3.Location = new System.Drawing.Point(209, 3);
            this.pbxErrorInfo3.Name = "pbxErrorInfo3";
            this.pbxErrorInfo3.Size = new System.Drawing.Size(35, 30);
            this.pbxErrorInfo3.TabIndex = 127;
            this.pbxErrorInfo3.TabStop = false;
            // 
            // lblErrorInfo3
            // 
            this.lblErrorInfo3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorInfo3.Location = new System.Drawing.Point(250, 0);
            this.lblErrorInfo3.Name = "lblErrorInfo3";
            this.lblErrorInfo3.Size = new System.Drawing.Size(250, 35);
            this.lblErrorInfo3.TabIndex = 128;
            this.lblErrorInfo3.Text = " (0) ";
            this.lblErrorInfo3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExpandCollapse3
            // 
            this.btnExpandCollapse3.BackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExpandCollapse3.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExpandCollapse3.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapse3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapse3.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapse3.Image = global::Parafait_POS.Properties.Resources.CollapseArrow;
            this.btnExpandCollapse3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapse3.Location = new System.Drawing.Point(643, 0);
            this.btnExpandCollapse3.Name = "btnExpandCollapse3";
            this.btnExpandCollapse3.Size = new System.Drawing.Size(317, 34);
            this.btnExpandCollapse3.TabIndex = 131;
            this.btnExpandCollapse3.Tag = "";
            this.btnExpandCollapse3.UseVisualStyleBackColor = false;
            this.btnExpandCollapse3.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // frmRescheduleSummaryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1003, 637);
            this.Controls.Add(this.fpnlMainPanel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReschedule);
            this.Controls.Add(this.lblNewFacilityMapValue);
            this.Controls.Add(this.lblNewFacilityMapLabel);
            this.Controls.Add(this.lblNewScheduleValue);
            this.Controls.Add(this.lblNewScheduleLabel);
            this.Controls.Add(this.lblOldFacilityMapValue);
            this.Controls.Add(this.lblOldFacilityMapLabel);
            this.Controls.Add(this.lblOldScheduleValue);
            this.Controls.Add(this.lblOldScheduleLabel);
            this.Controls.Add(this.lblGuestQtyValue);
            this.Controls.Add(this.lblGuestQtyLabel);
            this.Controls.Add(this.lblBookingProductName);
            this.Controls.Add(this.lblBookingProductLabel);
            this.Controls.Add(this.pnlDetailsDisplay);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRescheduleSummaryUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reschedule Summary";
            this.Load += new System.EventHandler(this.frmRescheduleSummaryUI_Load);
            this.pnlGrpPackageProducts.ResumeLayout(false);
            this.pnlGrpAdditionalProducts.ResumeLayout(false);
            this.pnlGrpAdditionalSlots.ResumeLayout(false);
            this.pnlDetailsDisplay.ResumeLayout(false);
            this.pnlDetailsDisplay.PerformLayout();
            this.pnlDetailsDisplayChild.ResumeLayout(false);
            this.pnlDetailsDisplayChild.PerformLayout();
            this.fpnlMainPanel.ResumeLayout(false);
            this.pnlHeaderOne.ResumeLayout(false);
            this.pnlHeaderOne.PerformLayout();
            this.tpnlHeaderOne.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxErrorInfo1)).EndInit();
            this.pnlHeaderTwo.ResumeLayout(false);
            this.pnlHeaderTwo.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxErrorInfo2)).EndInit();
            this.pnlHeaderThree.ResumeLayout(false);
            this.pnlHeaderThree.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxErrorInfo3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
         


        #endregion 
        private System.Windows.Forms.Label lblBookingProductLabel;
        private System.Windows.Forms.Label lblBookingProductName;
        private System.Windows.Forms.Label lblGuestQtyValue;
        private System.Windows.Forms.Label lblGuestQtyLabel;
        private System.Windows.Forms.Label lblOldFacilityMapValue;
        private System.Windows.Forms.Label lblOldFacilityMapLabel;
        private System.Windows.Forms.Label lblOldScheduleValue;
        private System.Windows.Forms.Label lblOldScheduleLabel;
        private System.Windows.Forms.Label lblNewFacilityMapValue;
        private System.Windows.Forms.Label lblNewFacilityMapLabel;
        private System.Windows.Forms.Label lblNewScheduleValue;
        private System.Windows.Forms.Label lblNewScheduleLabel;
        private System.Windows.Forms.Panel pnlGrpPackageProducts;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollPackgeProducts;
        private System.Windows.Forms.FlowLayoutPanel pnlPackageProduct;
        private System.Windows.Forms.Label lblOverRide1;
        private System.Windows.Forms.Label lblCancel1;
        private System.Windows.Forms.Label lblRescheduleStatus1;
        private System.Windows.Forms.Label lblQty1;
        private System.Windows.Forms.Label lblProductName1;
        private System.Windows.Forms.Panel pnlGrpAdditionalProducts;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollAdditionalProducts;
        private System.Windows.Forms.FlowLayoutPanel pnlAdditionalPackageProduct;
        private System.Windows.Forms.Label lblOverRide2;
        private System.Windows.Forms.Label lblCancel2;
        private System.Windows.Forms.Label lblRescheduleStatus2;
        private System.Windows.Forms.Label lblQty2;
        private System.Windows.Forms.Label lblProductName2;
        private System.Windows.Forms.Panel pnlGrpAdditionalSlots;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollAdditionalSlots;
        private System.Windows.Forms.FlowLayoutPanel pnlAdditionalSlots;
        private System.Windows.Forms.Label lblCancel3;
        private System.Windows.Forms.Label lblRescheduleStatus3;
        private System.Windows.Forms.Label lblProductName3;
        private System.Windows.Forms.Button btnCancel;        
        private System.Windows.Forms.Button btnReschedule;
        private System.Windows.Forms.Button btnCloseDisplayPanel;
        private System.Windows.Forms.Panel pnlDetailsDisplay;
        private System.Windows.Forms.Panel pnlDetailsDisplayChild;
        private System.Windows.Forms.Label lblDetailsDisplay;
        private System.Windows.Forms.Label lblDetailsHeader;
        private FlowLayoutPanel fpnlMainPanel;
        private Panel pnlHeaderOne;
        private Label lblDescription1;
        private PictureBox pbxErrorInfo1;
        private Label lblErrorInfo1;
        private Button btnExpandCollapse1;
        private Panel pnlHeaderTwo;
        private Button btnExpandCollapse2;
        private Panel pnlHeaderThree;
        private Button btnExpandCollapse3;
        private TableLayoutPanel tpnlHeaderOne;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblErrorInfo2;
        private PictureBox pbxErrorInfo2;
        private Label lblDescription2;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblErrorInfo3;
        private PictureBox pbxErrorInfo3;
        private Label lblDescription3;
    }
}