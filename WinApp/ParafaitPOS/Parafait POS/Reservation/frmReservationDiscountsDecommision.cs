using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Reservation
{
    public partial class frmReservationDiscounts : Form
    {
        public class clsReturnData
        {
            public object discountId;
            public string discountName;
            public string discountPercentage;
        }
       // Color POSBackColor;
        Utilities Utilities;
        ParafaitEnv ParafaitEnv;
        MessageUtils MessageUtils;
        int BookingId = -1;
        public List<clsReturnData> returnData = null;
        //public List<ParafaitUtils.Reservation.clsReservationDiscount> selectedReservationDiscount = null;
        Semnox.Parafait.Transaction.Reservation reservation;
        // ref List<ParafaitUtils.Reservation.clsReservationDiscount> reservationTransactionDiscounts
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmReservationDiscounts(int pBookingId, Utilities inUtilities, ref Semnox.Parafait.Transaction.Reservation _reservation)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmReservationDiscounts(" + pBookingId + ",inUtilities,_reservation)");//Added for logger function on 08-Mar-2016
            BookingId = pBookingId;
            Utilities = inUtilities;
            ParafaitEnv = POSStatic.ParafaitEnv = Utilities.ParafaitEnv;
            MessageUtils = POSStatic.MessageUtils = Utilities.MessageUtils;
            ParafaitEnv.Initialize();
            InitializeComponent();
            reservation = _reservation;
            // selectedReservationDiscount = reservationTransactionDiscounts;
            log.Debug("Ends-frmReservationDiscounts(" + pBookingId + ",inUtilities,_reservation)");//Added for logger function on 08-Mar-2016
        }
        private void initializeDiscounts()
        {
            /*log.Debug("Starts-initializeDiscounts()");//Added for logger function on 08-Mar-2016
            SqlCommand Discountcmd = Utilities.getCommand();
            Discountcmd.CommandText = "Select * from Discounts " +
                                    "where active_flag = 'Y' and " +
                                    "display_in_pos = 'Y' " +
                                    "and isnull(automatic_apply, 'N') != 'Y'" +
                                    "and discount_type = 'T' " +
                                    "order by sort_order";
            DataTable dtDiscounts = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Discountcmd);
            da.Fill(dtDiscounts);
            flowLayoutPanelDiscounts.Controls.Clear();
            if (BookingId != -1)
                // returnData = new List<clsReturnData>();
                selectedReservationDiscount = new List<ParafaitUtils.Reservation.clsReservationDiscount>();
            for (int i = 0; i < dtDiscounts.Rows.Count; i++)
            {
                Button DiscountButton = new Button();
                DiscountButton.Click += new EventHandler(DiscountButton_Click);
                DiscountButton.Name = "DiscountButton" + i.ToString();
                DiscountButton.Text = dtDiscounts.Rows[i]["Discount_name"].ToString();
                DiscountButton.Tag = dtDiscounts.Rows[i]["Discount_id"];
                DiscountButton.Font = SampleButtonDiscount.Font;
                DiscountButton.BackColor = Color.Transparent;
                DiscountButton.ForeColor = SampleButtonDiscount.ForeColor;
                DiscountButton.Size = SampleButtonDiscount.Size;
                DiscountButton.FlatStyle = SampleButtonDiscount.FlatStyle;
                DiscountButton.FlatAppearance.BorderSize = 0;
                DiscountButton.FlatAppearance.MouseDownBackColor = SampleButtonDiscount.FlatAppearance.MouseDownBackColor;
                DiscountButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                DiscountButton.BackgroundImage = SampleButtonDiscount.BackgroundImage;
                DiscountButton.BackgroundImageLayout = SampleButtonDiscount.BackgroundImageLayout;
                DiscountButton.TextAlign = SampleButtonDiscount.TextAlign;
                if (dtDiscounts.Rows[i]["automatic_apply"].ToString() == "Y")
                    DiscountButton.Enabled = false;
                flowLayoutPanelDiscounts.Controls.Add(DiscountButton);
                // clsReturnData lclReturnData = new clsReturnData();
                ParafaitUtils.Reservation.clsReservationDiscount lclReturnData = new ParafaitUtils.Reservation.clsReservationDiscount();
                lclReturnData.discountId = dtDiscounts.Rows[i]["discount_id"];
                lclReturnData.discountName = DiscountButton.Text;
                lclReturnData.discountPercentage = dtDiscounts.Rows[i]["discount_percentage"].ToString();
                DiscountButton.Tag = lclReturnData;
                if (selectedReservationDiscount != null)
                {
                    bool found = false;
                    foreach (ParafaitUtils.Reservation.clsReservationDiscount d in selectedReservationDiscount)
                    {
                        if (d.discountId.Equals(lclReturnData.discountId))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        DiscountButton.BackgroundImage = global::Parafait_POS.Properties.Resources.ManualProduct;
                        dgvSelected.Rows.Add(lclReturnData.discountName + " - " + lclReturnData.discountPercentage + "%");
                    }
                    else
                    {
                        DiscountButton.BackgroundImage = global::Parafait_POS.Properties.Resources.DiplayGroupButton;
                    }
                }
                //Done to Color the Discounts button with Green color, which were previously selected by the user//
                if (BookingId != -1)
                {
                    // clsReturnData discount = new clsReturnData();
                    ParafaitUtils.Reservation.clsReservationDiscount discount = new ParafaitUtils.Reservation.clsReservationDiscount();
                    DataTable reservationDiscounts = Utilities.executeDataTable(@"Select discount_id, discount_name discountName, discount_percentage discountPercentage
                                                                                       from discounts
                                                                                       where discount_id in 
                                                                                        (Select ReservationDiscountId
                                                                                            from ReservationDiscounts
                                                                                            where BookingId = @BookingId)",
                                                                                   new SqlParameter("@BookingId", BookingId));
                    dgvSelected.Rows.Clear();
                    foreach (DataRow discountRow in reservationDiscounts.Rows)
                    {
                        dgvSelected.Rows.Add(discountRow["discountName"].ToString() + " - " + discountRow["discountPercentage"] + "%");

                        if (lclReturnData.discountId.Equals(discountRow["discount_id"]))
                        {
                            DiscountButton.BackgroundImage = global::Parafait_POS.Properties.Resources.ManualProduct;
                            discount.discountId = lclReturnData.discountId;
                            discount.discountName = lclReturnData.discountName;
                            discount.discountPercentage = lclReturnData.discountPercentage;
                        }
                    }
                    if (discount.discountId != null)//If discount is previously added then fill returndata, so next click on the same button will deselect the discount
                        selectedReservationDiscount.Add(discount);
                }
            }
            flowLayoutPanelDiscounts.Refresh();
            log.Debug("Ends-initializeDiscounts()");//Added for logger function on 08-Mar-2016*/
        }

        private void DiscountButton_Click(object sender, EventArgs e)
        {
            /*log.Debug("Starts-DiscountButton_Click()");//Added for logger function on 08-Mar-2016
            Button b = (Button)sender;
            //b.FlatAppearance.BorderColor = POSBackColor;
            // clsReturnData modData = (sender as Button).Tag as clsReturnData;
            ParafaitUtils.Reservation.clsReservationDiscount modData = (sender as Button).Tag as ParafaitUtils.Reservation.clsReservationDiscount;
            int discount_id = Convert.ToInt32(modData.discountId);
            if (selectedReservationDiscount == null)
                // returnData = new List<clsReturnData>();
                selectedReservationDiscount = new List<ParafaitUtils.Reservation.clsReservationDiscount>();
            bool found = false;
            //  clsReturnData foundData = null;
            ParafaitUtils.Reservation.clsReservationDiscount foundData = null;
            foreach (ParafaitUtils.Reservation.clsReservationDiscount d in selectedReservationDiscount)
            {
                if (d.discountId.Equals(modData.discountId))
                {
                    found = true;
                    foundData = d;
                    break;
                }
            }
            if (found)
            {
                selectedReservationDiscount.Remove(foundData);//returnData
                (sender as Button).BackgroundImage = global::Parafait_POS.Properties.Resources.DiplayGroupButton;
            }
            else
            {
                selectedReservationDiscount.Add(modData);

                (sender as Button).BackgroundImage = global::Parafait_POS.Properties.Resources.ManualProduct;
            }

            dgvSelected.Rows.Clear();
            foreach (ParafaitUtils.Reservation.clsReservationDiscount d in selectedReservationDiscount)
                dgvSelected.Rows.Add(d.discountName + " - " + d.discountPercentage + "%");

            log.Debug("Ends-DiscountButton_Click()");//Added for logger function on 08-Mar-2016*/
        }

        //On Clicking OK will Create Discount Lines//
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click()");//Added for logger function on 08-Mar-2016
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.Debug("Ends-btnClose_Click()");//Added for logger function on 08-Mar-2016
        }

        //Inserts Discounts into Reservation Discounts Table//
//        void CreateReservationDiscounts(int bookingId, int discountId, string discountPercentage)
//        {
//            if (bookingId != -1)
//            {
//                Utilities.executeNonQuery(@"If NOT EXISTS(SELECT * from ReservationDiscounts
//                                                                    where BookingId = @BookingId 
//                                                                    and ReservationDiscountId = @DiscountId )
//                                            Insert into ReservationDiscounts(BookingId,
//                                                                             ReservationDiscountId,
//                                                                             ReservationDiscountPecentage,
//                                                                             ReservationDiscountCategory)
//                                            values(@BookingId, @DiscountId, @DiscountPercentage, 'Transaction Discount' )",
//                                                               new SqlParameter("@BookingId", bookingId),
//                                                               new SqlParameter("@DiscountId", discountId),
//                                                               new SqlParameter("@DiscountPercentage", discountPercentage));
//            }
//        }

//        //Added to Create discount Lines and Insert Discounts selected into ReservationDiscounts Table on July 1, 2015//
//        void CreateDiscount(int discountId, string discountName, string discountPercentage, string couponNumber, int couponSetId, string message)
//        {
//            reservation.Transaction.createDiscount(discountId, couponNumber, couponSetId, ref message);
//            CreateReservationDiscounts(BookingId, discountId, discountPercentage);
//        }

        //Added to Load the Discounts on July 2, 2015//
        private void ReservationDiscounts_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-ReservationDiscounts_Load()");//Added for logger function on 08-Mar-2016
            initializeDiscounts();
            log.Debug("Ends-ReservationDiscounts_Load()");//Added for logger function on 08-Mar-2016
        }

        //Added to delete the DiscountLines when Discounts are Deselected on July 2, 2015//
        //void CancelDiscountLine()
        //{
        //    DataTable reservationDiscountID = Utilities.executeDataTable(@"Select ReservationDiscountID from ReservationDiscounts where BookingId = @BookingId", new SqlParameter("@BookingId", BookingId));
        //    foreach (DataRow dr in reservationDiscountID.Rows)
        //    {
        //        int discountId = Convert.ToInt32(dr["ReservationDiscountId"].ToString());
        //        reservation.Transaction.cancelDiscountLine(discountId);
        //    }
        //}
    }
}
