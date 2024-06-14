/********************************************************************************************
 * Project Name - TrxLineDiscountsUI
 * Description  - Trx Line Discounts UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.150.0    22-Apr-2021    Abhishek        Modified : POS UI Redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Apply discount on a transaction line
    /// </summary>
    public partial class TrxLineDiscountsUI : Form
    {
        private readonly Utilities utilities;
        /// <summary>
        /// selected discount if dialogue result is OK
        /// </summary>
        private DiscountContainerDTO discountContainerDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<DiscountContainerDTO> applicableDiscountsDTOList = new List<DiscountContainerDTO>();
        /// <summary>
        /// To load all the discounts.
        /// </summary>
        /// <param name="utilities">parafait utilities</param>
        /// <param name="productId">product id of the line</param>
        /// <param name="categoryId">category of the line</param>
        public TrxLineDiscountsUI(Utilities utilities, int productId, int categoryId)
        {
            log.LogMethodEntry(utilities, productId, categoryId);
            this.utilities = utilities;
            InitializeComponent();            
            PopulateDiscounts(productId, categoryId);
            log.LogMethodExit();
        }
 
        private void PopulateDiscounts(int productId, int categoryId)
        {
            log.LogMethodEntry(productId, categoryId);
            DateTime currentDateTime = (new LookupValuesList(utilities.ExecutionContext)).GetServerDateTime();
            IEnumerable<DiscountContainerDTO> transactionDiscounts =
                DiscountContainerList.GetTransactionDiscountsBLList(utilities.ExecutionContext);
            foreach (DiscountContainerDTO discountContainerDTO in transactionDiscounts)
            {
                if (discountContainerDTO.CouponMandatory == "Y")
                {
                    continue;
                }

                if (discountContainerDTO.AutomaticApply == "Y")
                {
                    continue;
                }

                if (DiscountContainerList.IsDiscounted(utilities.ExecutionContext, discountContainerDTO.DiscountId,productId) == false)
                {
                    continue;
                }

                if (discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList != null &&
                    discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList.Any())
                {
                    continue;
                }
                applicableDiscountsDTOList.Add(discountContainerDTO);
            }

            flpDiscounts.Controls.Clear();
            if (applicableDiscountsDTOList == null || applicableDiscountsDTOList.Count == 0)
            {
                return;
            }
            foreach (DiscountContainerDTO discountContainerDTO in applicableDiscountsDTOList)
            {
                Button btn = new Button
                {
                    BackColor = System.Drawing.Color.Transparent,
                    BackgroundImage = global::Semnox.Parafait.Discounts.Properties.Resources.discount_button,
                    BackgroundImageLayout = ImageLayout.Zoom
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
                btn.ForeColor = System.Drawing.Color.White;
                btn.Margin = new Padding(6, 6, 3, 3);
                btn.Name = "btn" + discountContainerDTO.DiscountId;
                btn.Size = new System.Drawing.Size(103, 99);
                btn.Text = discountContainerDTO.DiscountName;
                btn.UseVisualStyleBackColor = false;
                btn.Click += btnDiscount_Click;
                btn.Tag = discountContainerDTO;
                flpDiscounts.Controls.Add(btn);
            }
            log.LogMethodExit();
        }


        private void btnDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button btn = sender as Button;
            if (btn != null)
            {
                discountContainerDTO = (DiscountContainerDTO)btn.Tag;
                lblDiscountSelected.Text = discountContainerDTO.DiscountName;
            }
            log.LogMethodExit();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        private void TrxLineDiscountsUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (applicableDiscountsDTOList.Count == 0)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "No discounts configured."));
                DialogResult = DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// return the selected discount DTO
        /// </summary>
        public DiscountContainerDTO SelectedDiscountsDTO
        {
            get
            {
                return discountContainerDTO;
            }
        }
    }
}
