
/********************************************************************************************
 * Project Name - ReservationDiscountsListUI
 * Description  - ReservationDiscountsListUI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.70        26-Mar-2019   Guru S A          Booking phase 2 changes   
 *2.150.0     27-Apr-2021   Abhishek          Modified : POS UI Redesign
 ********************************************************************************************/

using Semnox.Parafait.Discounts;
using System;
using System.Collections.Generic; 
using System.Drawing; 
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Linq;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Used for Selecting resevation discounts.
    /// </summary>
    public partial class ReservationDiscountsListUI : Form
    {
        Utilities utilities;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private int productId = -1;
        private int comboProductId = -1; 
        private List<DiscountContainerDTO> fromTransactionDiscountContainerDTOList;
        private SortableBindingList<DiscountContainerDTO> selectedDiscountsDTOList = new SortableBindingList<DiscountContainerDTO>();

        /// <summary>
        /// Constructor of ReservationDiscountsListUI
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="appliedDiscountContainerDTOList"></param>
        /// <param name="productId"></param>
        /// <param name="comboProductId"></param>
        public ReservationDiscountsListUI(Utilities utilities, List<DiscountContainerDTO> appliedDiscountContainerDTOList, int productId = -1, int comboProductId = -1)
        {
            log.LogMethodEntry(productId, comboProductId, appliedDiscountContainerDTOList);
            this.utilities = utilities;
            this.productId = productId;
            this.comboProductId = comboProductId; 
            this.fromTransactionDiscountContainerDTOList = appliedDiscountContainerDTOList;
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
            log.LogMethodExit();
        }

        
        private void ReservationDiscountsListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Products product = null;
            if (productId != -1 && comboProductId != -1)
            {
                product = new Products(utilities.ExecutionContext, productId, false);
            }

            DateTime currentDateTime = (new LookupValuesList(utilities.ExecutionContext)).GetServerDateTime();
            List<DiscountContainerDTO> discountContainerDTOList = new List<DiscountContainerDTO>();
            IEnumerable<DiscountContainerDTO> transactionDiscounts =
                DiscountContainerList.GetTransactionDiscountsBLList(utilities.ExecutionContext);
            foreach (DiscountContainerDTO transactionDiscount in transactionDiscounts)
            {
                if (transactionDiscount.CouponMandatory == "Y")
                {
                    continue;
                }

                if (transactionDiscount.AutomaticApply == "Y")
                {
                    continue;
                }

                if (DiscountContainerList.IsDiscountAvailable(utilities.ExecutionContext, transactionDiscount.DiscountId, currentDateTime) == false)
                {
                    continue;
                }

                if (product != null && productId != -1 && comboProductId != -1)
                {
                    if (DiscountContainerList.IsDiscounted(utilities.ExecutionContext, transactionDiscount.DiscountId, productId) == false)
                    {
                        continue;
                    }

                    if (transactionDiscount.DiscountPurchaseCriteriaContainerDTOList != null &&
                        transactionDiscount.DiscountPurchaseCriteriaContainerDTOList.Any())
                    {
                        continue;
                    }
                }

                discountContainerDTOList.Add(transactionDiscount);
            }


            //List<ReservationDiscountsDTO> reservationDiscountsDTOList = null;//already commented
            //if (bookingId != -1)
            //{
            //    ReservationDiscountsListBL reservationDiscountsListBL = new ReservationDiscountsListBL();
            //    List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>> searchReservationDiscountsParams = new List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>>();
            //    searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.BOOKING_ID, bookingId.ToString()));
            //    searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            //    reservationDiscountsDTOList = reservationDiscountsListBL.GetReservationDiscountsDTOList(searchReservationDiscountsParams);
            //}

            flpDiscounts.Controls.Clear();
            if (discountContainerDTOList.Any())
            {
                for (int i = 0; i < discountContainerDTOList.Count; i++)
                {
                    Button DiscountButton = new Button();
                    DiscountButton.Click += new EventHandler(btnDiscount_Click);
                    DiscountButton.Name = "DiscountButton" + i.ToString();
                    DiscountButton.Text = discountContainerDTOList[i].DiscountName;
                    DiscountButton.Tag = discountContainerDTOList[i];
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

                    if (fromTransactionDiscountContainerDTOList != null && fromTransactionDiscountContainerDTOList.Count > 0)
                    {
                        foreach (DiscountContainerDTO selectedDiscountsDTO in fromTransactionDiscountContainerDTOList)
                        {
                            if (selectedDiscountsDTO.DiscountId == discountContainerDTOList[i].DiscountId)
                            {
                                DiscountButton.BackgroundImage = Properties.Resources.ManualProduct;
                                selectedDiscountsDTOList.Add(discountContainerDTOList[i]);
                                break;
                            }
                        }
                    }
                }
            }
            flpDiscounts.Refresh();
            discountsDTOListBS.DataSource = selectedDiscountsDTOList;
            log.LogMethodExit();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button b = (Button)sender;
            DiscountContainerDTO discountsDTO = (sender as Button).Tag as DiscountContainerDTO;
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
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the selected discounts
        /// </summary>
        public SortableBindingList<DiscountContainerDTO> SelectedDiscountsDTOList
        {
            get
            {
                return selectedDiscountsDTOList;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }
    }
}
