using Semnox.Core;
//using Semnox.Core.SortableBindingList;
//using Semnox.Parafait.Context;
using Semnox.Parafait.Discounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// Used for Selecting resevation discounts.
    /// </summary>
    public partial class ReservationDiscountsListUI : Form
    {
        Utilities utilities;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int bookingId;
        SortableBindingList<DiscountsDTO> selectedDiscountsDTOList = new SortableBindingList<DiscountsDTO>();
        
        /// <summary>
        /// Constructor of ReservationDiscountsListUI
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="bookingId">Booking Id</param>
        public ReservationDiscountsListUI(Utilities utilities, int bookingId)
        {
            log.Debug("Starts-DiscountCouponIssueUI(utilities, bookingId) parameterized constructor.");
            this.utilities = utilities;
            this.bookingId = bookingId;
            InitializeComponent();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            log.Debug("Ends-DiscountCouponIssueUI(utilities, bookingId) parameterized constructor.");
        }

        private void ReservationDiscountsListUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-ReservationDiscountsListUI_Load() Event");
            DiscountsListBL discountsListBL = new DiscountsListBL();
            List<KeyValuePair<DiscountsDTO.SearchByParameters, string>> searchDiscountsParams = new List<KeyValuePair<DiscountsDTO.SearchByParameters, string>>();
            searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, DiscountsBL.DISCOUNT_TYPE_TRANSACTION));
            searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
            searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.AUTOMATIC_APPLY, "N"));
            searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISPLAY_IN_POS, "Y"));
            searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<DiscountsDTO> discountsDTOList = discountsListBL.GetDiscountsDTOList(searchDiscountsParams);
            List<ReservationDiscountsDTO> reservationDiscountsDTOList = null;
            if (bookingId != -1)
            {
                ReservationDiscountsListBL reservationDiscountsListBL = new ReservationDiscountsListBL();
                List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>> searchReservationDiscountsParams = new List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>>();
                searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.BOOKING_ID, bookingId.ToString()));
                searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                reservationDiscountsDTOList = reservationDiscountsListBL.GetReservationDiscountsDTOList(searchReservationDiscountsParams);
            }

            flpDiscounts.Controls.Clear();
            if (discountsDTOList != null)
            {
                for (int i = 0; i < discountsDTOList.Count; i++)
                {
                    Button DiscountButton = new Button();
                    DiscountButton.Click += new EventHandler(btnDiscount_Click);
                    DiscountButton.Name = "DiscountButton" + i.ToString();
                    DiscountButton.Text = discountsDTOList[i].DiscountName;
                    DiscountButton.Tag = discountsDTOList[i];
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
                    flpDiscounts.Controls.Add(DiscountButton);

                    if (reservationDiscountsDTOList != null)
                    {
                        foreach (var reservationDiscountsDTO in reservationDiscountsDTOList)
                        {
                            if (reservationDiscountsDTO.ReservationDiscountId == discountsDTOList[i].DiscountId)
                            {
                                DiscountButton.BackgroundImage = Properties.Resources.ManualProduct;
                                selectedDiscountsDTOList.Add(discountsDTOList[i]);
                                break;
                            }
                        }
                    }
                }
            }
            flpDiscounts.Refresh();
            discountsDTOListBS.DataSource = selectedDiscountsDTOList;
            log.Debug("Ends-ReservationDiscountsListUI_Load() Event");
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancel_Click() Event");
            Button b = (Button)sender;
            DiscountsDTO discountsDTO = (sender as Button).Tag as DiscountsDTO;
            if(selectedDiscountsDTOList.Contains(discountsDTO))
            {
                (sender as Button).BackgroundImage = Properties.Resources.DiplayGroupButton;
                selectedDiscountsDTOList.Remove(discountsDTO);
            }
            else
            {
                selectedDiscountsDTOList.Add(discountsDTO);
                (sender as Button).BackgroundImage = Properties.Resources.ManualProduct;
            }
            log.Debug("Ends-btnCancel_Click() Event");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancel_Click() Event");
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.Debug("Ends-btnCancel_Click() Event");
        }

        /// <summary>
        /// returns the selected discounts
        /// </summary>
        public SortableBindingList<DiscountsDTO> SelectedDiscountsDTOList
        {
            get
            {
                return selectedDiscountsDTOList;
            }
        }
    }
}
