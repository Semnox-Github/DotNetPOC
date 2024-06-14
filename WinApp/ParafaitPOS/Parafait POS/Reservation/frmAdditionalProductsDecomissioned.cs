/********************************************************************************************
 * Project Name - Reservation
 * Description  - AdditionalProducts selection form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Booking phase 2 enhancement changes - Decommissioned
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmAdditionalProductsDecomissioned : Form
    {
        Utilities _utilities;
        MessageUtils MessageUtils;
        int _BookingProductId;
        Semnox.Parafait.Transaction.ReservationBL Reservation;
       // List<KeyValuePair<AttractionBooking, int>> lstAdditionalAttractionProductslist;//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
       // AttractionBooking atb;//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public class clsReturnData
        {
            public string displayGroup;
            public object ProductId;
            public decimal Price;
            public string productName;
            public decimal quantity;
            public string productType;
        }

        public List<clsReturnData> ReturnData = null;
        public frmAdditionalProductsDecomissioned(int BookingProductId, Utilities inUtilities)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmAdditionalProducts(" + BookingProductId + ",inUtilities,lstAdditionalAttractionProductsList)");//Added for logger function on 08-Mar-2016
            _BookingProductId = BookingProductId;
            _utilities = inUtilities;
            MessageUtils = _utilities.MessageUtils;
            Reservation = new Semnox.Parafait.Transaction.ReservationBL(_utilities.ExecutionContext, _utilities,-1);
            ParafaitEnv ParafaitEnv = _utilities.ParafaitEnv;
            //lstAdditionalAttractionProductslist = lstAdditionalAttractionProductsList;//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
         //   atb = new AttractionBooking(_utilities);//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
            ParafaitEnv.Initialize();
            InitializeComponent();

            DataTable dt = new DataTable();//GGGG Reservation.GetAdditionalProductsDisplayGroups(BookingProductId);

            this.Load += frmProductModifiers_Load;

            flpModifierSets.Controls.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Button btnModifierSet = new Button();
                btnModifierSet.Size = sampleModifierSetButton.Size;
                btnModifierSet.BackColor = sampleModifierSetButton.BackColor;
                btnModifierSet.ForeColor = sampleModifierSetButton.ForeColor;
                btnModifierSet.BackgroundImage = sampleModifierSetButton.BackgroundImage;
                btnModifierSet.BackgroundImageLayout = sampleModifierSetButton.BackgroundImageLayout;
                btnModifierSet.FlatStyle = sampleModifierSetButton.FlatStyle;
                btnModifierSet.FlatAppearance.BorderColor = sampleModifierSetButton.FlatAppearance.BorderColor;
                btnModifierSet.FlatAppearance.BorderSize = sampleModifierSetButton.FlatAppearance.BorderSize;
                btnModifierSet.FlatAppearance.CheckedBackColor = sampleModifierSetButton.FlatAppearance.CheckedBackColor;
                btnModifierSet.FlatAppearance.MouseDownBackColor = sampleModifierSetButton.FlatAppearance.MouseDownBackColor;
                btnModifierSet.FlatAppearance.MouseOverBackColor = sampleModifierSetButton.FlatAppearance.MouseOverBackColor;
                btnModifierSet.Text = dr["display_group"].ToString();
                btnModifierSet.Tag = dr["display_group"];
                btnModifierSet.Click += btnModifierSet_Click;

                flpModifierSets.Controls.Add(btnModifierSet);
            }
            log.Debug("Ends-frmAdditionalProducts(" + BookingProductId + ",inUtilities,lstAdditionalAttractionProductsList)");//Added for logger function on 08-Mar-2016
        }

       /* //Begin Modification - Jan-20-2016- Added to create Attraction Product Object//
        public bool createAttractionProduct(int product_id, double price, int quantity, int parentProductId)
        {
            log.Debug("Starts-frmAdditionalProducts(" + product_id + "," + price + "," + quantity + ", " + parentProductId + ")");//Added for logger function on 08-Mar-2016
            bool retVal = false;
            AttractionSchedules ats = new AttractionSchedules(null, product_id, quantity);

            if (ats.returnDGV == null) // schedules not specified.
            {
                atb = null;
                log.Info("frmAdditionalProducts(" + product_id + "," + price + "," + quantity + ", " + parentProductId + ") - no data");//Added for logger function on 08-Mar-2016
            }
            else if (ats.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                log.Info("frmAdditionalProducts(" + product_id + "," + price + "," + quantity + ", " + parentProductId + ") - OK is clicked");//Added for logger function on 08-Mar-2016
                DataGridView dgv = ats.returnDGV;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv["Desired Units", i].Value != null && dgv["Desired Units", i].Value != DBNull.Value)
                    {
                        int qty = Convert.ToInt32(dgv["Desired Units", i].Value);
                        if (qty <= 0)
                            continue;
                        if (qty > 999)
                            qty = 999;
                        //  AttractionBooking atb = new AttractionBooking(Utilities);
                        atb.AttractionPlayId = Convert.ToInt32(dgv["AttractionPlayId", i].Value);
                        atb.AttractionPlayName = dgv["Play Name", i].Value.ToString();
                        atb.AttractionScheduleId = Convert.ToInt32(dgv["AttractionScheduleId", i].Value);
                        atb.ScheduleTime = Convert.ToDateTime(dgv["Schedule Time", i].Value);
                        atb.BookedUnits = qty;
                        if (dgv["Total Units", i].Value != DBNull.Value)
                            atb.AvailableUnits = Convert.ToInt32(dgv["Total Units", i].Value);
                        atb.Price = Convert.ToDouble(dgv["Price", i].Value == DBNull.Value ? 0 : dgv["Price", i].Value);
                        if (dgv["Expiry Date", i].Value != DBNull.Value)
                            atb.ExpiryDate = Convert.ToDateTime(dgv["Expiry Date", i].Value);
                        atb.PromotionId = Convert.ToInt32(dgv["PromotionId", i].Value);
                        atb.SelectedSeats = (dgv["Seats", i].Tag == null ? null : dgv["Seats", i].Tag as List<int>);
                        atb.SelectedSeatNames = (dgv["PickSeats", i].Tag == null ? null : dgv["PickSeats", i].Tag as List<string>);
                        lstAdditionalAttractionProductslist.Add(new KeyValuePair<AttractionBooking, int>(atb, product_id));
                        break;
                    }
                }
            }
            else
                log.Info("frmAdditionalProducts(" + product_id + "," + price + "," + quantity + ", " + parentProductId + ") - Cancel is clicked");//Added for logger function on 08-Mar-2016
                retVal = false;

            ats.Dispose();
            return retVal;
        }
        //End Modification - Jan-20-2016- Added to create Attraction Product Object//*/

        void frmProductModifiers_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmProductModifiers_Load()");//Added for logger function on 08-Mar-2016
            refreshSeleted();
            if (flpModifierSets.Controls.Count > 0)
                btnModifierSet_Click(flpModifierSets.Controls[0], null);

            log.Debug("Ends-frmProductModifiers_Load()");//Added for logger function on 08-Mar-2016
        }

        void btnModifierSet_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnModifierSet_Click()");//Added for logger function on 08-Mar-2016
            foreach (Control c in flpModifierSets.Controls)
            {
                c.BackgroundImage = sampleModifierSetButton.BackgroundImage;
            }

            object displayGroup = (sender as Button).Tag;
            (sender as Button).BackgroundImage = global::Parafait_POS.Properties.Resources.pressed2;

            this.Text = "Additional Products    Display Group: " + (sender as Button).Text;

            DataTable dt = new DataTable();//GGGG Reservation.GetAdditionalProductsByDispalyGroup(_BookingProductId, displayGroup.ToString());

            flpModifierList.Controls.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Button btnModifierProduct = new Button();
                btnModifierProduct.Size = sampleModifierProduct.Size;
                btnModifierProduct.BackColor = sampleModifierProduct.BackColor;
                btnModifierProduct.BackgroundImage = sampleModifierProduct.BackgroundImage;
                btnModifierProduct.BackgroundImageLayout = sampleModifierProduct.BackgroundImageLayout;
                btnModifierProduct.FlatStyle = sampleModifierProduct.FlatStyle;
                btnModifierProduct.FlatAppearance.BorderColor = sampleModifierProduct.FlatAppearance.BorderColor;
                btnModifierProduct.FlatAppearance.BorderSize = sampleModifierProduct.FlatAppearance.BorderSize;
                btnModifierProduct.FlatAppearance.CheckedBackColor = sampleModifierProduct.FlatAppearance.CheckedBackColor;
                btnModifierProduct.FlatAppearance.MouseDownBackColor = sampleModifierProduct.FlatAppearance.MouseDownBackColor;
                btnModifierProduct.FlatAppearance.MouseOverBackColor = sampleModifierProduct.FlatAppearance.MouseOverBackColor;
                btnModifierProduct.Text = dr["product_name"].ToString();

                clsReturnData lclReturnData = new clsReturnData();
                lclReturnData.ProductId = dr["product_id"];
                lclReturnData.productType = dr["product_type"].ToString();
                lclReturnData.Price = Convert.ToDecimal(dr["price"]);
                lclReturnData.productName = btnModifierProduct.Text;
                lclReturnData.displayGroup = displayGroup.ToString();
                btnModifierProduct.Tag = lclReturnData;

                if (ReturnData != null)
                {
                    bool found = false;
                    foreach (clsReturnData d in ReturnData)
                    {
                        if (d.ProductId.Equals(lclReturnData.ProductId))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        btnModifierProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.ManualProduct;
                    }
                    else
                    {
                        btnModifierProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.ComboProduct;
                    }
                }

                btnModifierProduct.Click += btnModifierProduct_Click;

                flpModifierList.Controls.Add(btnModifierProduct);
            }
            log.Debug("Ends-btnModifierSet_Click()");//Added for logger function on 08-Mar-2016
        }

        void btnModifierProduct_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnModifierProduct_Click()");//Added for logger function on 08-Mar-2016
            clsReturnData modData = (sender as Button).Tag as clsReturnData;
                

            if (ReturnData == null)
                ReturnData = new List<clsReturnData>();

            bool found = false;
            clsReturnData foundData = null;
            foreach (clsReturnData d in ReturnData)
            {
                if (d.ProductId.Equals(modData.ProductId))
                {
                    found = true;
                    foundData = d;
                    break;
                }
            }
            if (found)
            {
                ReturnData.Remove(foundData);
                (sender as Button).BackgroundImage = global::Parafait_POS.Properties.Resources.ComboProduct;
                //Begin Modification- Jan-20-2016-Remove the Additional Attraction Product when product is deselected//
                //if (lstAdditionalAttractionProductslist != null && lstAdditionalAttractionProductslist.Count > 0)
                //{
                //    for (int j = lstAdditionalAttractionProductslist.Count - 1; j >= 0; j--)
                //    {
                //        if (Convert.ToInt32(foundData.ProductId) == lstAdditionalAttractionProductslist[j].Value)
                //        {
                //            lstAdditionalAttractionProductslist.RemoveAt(j);
                //        }
                //    }
                //}
                //End Modification - Jan-20-2016
            }
            else
            {
                //Added ShowNumberPadForm method to enter the quantity of Additional Products on May 13,2015//
                modData.quantity = (decimal)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Enter Quantity"), '-', _utilities);
                if(modData.quantity == 0)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(479));
                    log.Info("Ends-btnModifierProduct_Click() as Product Quantity not entered");//Added for logger function on 08-Mar-2016
                    return;
                }
                ReturnData.Add(modData);
                (sender as Button).BackgroundImage = global::Parafait_POS.Properties.Resources.ManualProduct;
            }

         
            refreshSeleted();
            log.Debug("Ends-btnModifierProduct_Click()");//Added for logger function on 08-Mar-2016
        }

        void refreshSeleted()
        {
            log.Debug("Starts-refreshSeleted()");//Added for logger function on 08-Mar-2016
            dgvSelected.Rows.Clear();
            foreach (clsReturnData d in ReturnData)
                dgvSelected.Rows.Add(d.displayGroup + " - " + d.productName);

            foreach (Control c in flpModifierSets.Controls)
            {
                if (c.Text.EndsWith("●"))
                    c.Text = c.Text.TrimEnd('●').TrimEnd();

                bool found = false;
                foreach (clsReturnData d in ReturnData)
                {
                    if (d.displayGroup.Equals(c.Text))
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    c.Text += "     ●";
                }
            }
            log.Debug("Ends-refreshSeleted()");//Added for logger function on 08-Mar-2016
        }
    }
}
