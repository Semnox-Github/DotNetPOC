/********************************************************************************************
 * Project Name - DiscountCouponIssueUI
 * Description  - Discount Coupon Issue UI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       05-Aug-2019   Girish Kundar   Added LogMethodEntry() and LogMethodExit() methods. 
 *2.120.0      21-Apr-2021    Abhishek        Modified : POS UI Redesign
 ********************************************************************************************/
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Used for Issuing discount coupons.
    /// </summary>
    public partial class DiscountCouponIssueUI : Form
    {

       private Utilities utilities;
       private  static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       private AlphaNumericKeyPad keypad;
       private Control currentAlphanumericTextBox;
       private DiscountCouponsHeaderDTO discountCouponsHeaderDTO;
       private List<DiscountCouponsDTO> discountCouponsDTOList;
       private DateTime effectiveDate;
       private DateTime expiryDate;
       private int productQuantity;
       private int businessStartTime;

        /// <summary>
        /// Parameterized Constructor.
        /// </summary>
        /// <param name="utilities">parafait utilities</param>
        /// <param name="discountCouponsHeaderDTO">discount Coupons Header DTO</param>
        /// <param name="effectiveDate">Coupons Effective date</param>
        /// <param name="expiryDate">Coupons Expiry date</param>
        public DiscountCouponIssueUI(Utilities utilities, 
                                     DiscountCouponsHeaderDTO discountCouponsHeaderDTO,
                                     DateTime effectiveDate,
                                     DateTime expiryDate,
                                     int productQuantity)//add one param as promptQuantity
        {
            log.LogMethodEntry(utilities,  discountCouponsHeaderDTO,  effectiveDate, expiryDate, productQuantity);
            this.utilities = utilities;
            this.discountCouponsHeaderDTO = discountCouponsHeaderDTO;
            this.effectiveDate = effectiveDate;
            this.expiryDate = expiryDate;
            this.productQuantity = productQuantity;
            InitializeComponent();
            utilities.setupDataGridProperties(ref dgvDiscountCouponsDTOList);
            utilities.setLanguage(this);
            dgvDiscountCouponsDTOList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            log.LogMethodExit();
        }

        private void DiscountCouponIssueUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            lblCouponCount.Text = Convert.ToString(discountCouponsHeaderDTO.Count == null ? 1 : discountCouponsHeaderDTO.Count);
            if (int.TryParse(utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessStartTime) == false)
            {
                businessStartTime = 6;
            }
            if (utilities.getParafaitDefaults("ALLOW_EDIT_COUPON_VALIDITY_IN_POS").ToLower() == "y")
            {
                dtpEffectiveFrom.Enabled = true;
                dtpExpiresOn.Enabled = true;
            }
            else
            {
                dtpEffectiveFrom.Enabled = false;
                dtpExpiresOn.Enabled = false;
            }
            dtpEffectiveFrom.Value = Convert.ToDateTime(effectiveDate.Date.AddHours(businessStartTime).ToString("dddd, dd-MMM-yyyy h:mm tt"));
            if (discountCouponsHeaderDTO.ExpiryDate != null)
            {
                dtpExpiresOn.Value = Convert.ToDateTime(discountCouponsHeaderDTO.ExpiryDate.Value.AddHours(businessStartTime).ToString("dddd, dd-MMM-yyyy h:mm tt"));
            }
            else
            {
                dtpExpiresOn.Value = Convert.ToDateTime(expiryDate.Date.AddDays(1).AddHours(businessStartTime).ToString("dddd, dd-MMM-yyyy h:mm tt"));
            }
            DiscountContainerDTO discountContainerDTO =
                DiscountContainerList.GetDiscountContainerDTO(utilities.ExecutionContext, discountCouponsHeaderDTO.DiscountId);
            if (discountContainerDTO == null)
            {
                string message = MessageContainerList.GetMessage(utilities.ExecutionContext, 2196, "Discount", discountCouponsHeaderDTO.DiscountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            lblDiscountName.Text = discountContainerDTO.DiscountName;
            lblDiscountValue.Text = discountContainerDTO.DiscountAmount > 0 ? discountContainerDTO.DiscountAmount.ToString() : (discountContainerDTO.DiscountPercentage + "%");
            DiscountCouponsHeaderBL discountCouponsHeaderBL = new DiscountCouponsHeaderBL(utilities.ExecutionContext, discountCouponsHeaderDTO);
            txtFrom.Text = discountCouponsHeaderBL.GetRandomCouponNumber(10);
            log.LogMethodExit();
        }

        private void btnAlphaKeypad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (currentAlphanumericTextBox == null)
            {
                currentAlphanumericTextBox = txtFrom;
            }
            Point p = currentAlphanumericTextBox.PointToScreen(Point.Empty);
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, currentAlphanumericTextBox);
                if (p.Y + 60 + keypad.Height < Screen.PrimaryScreen.WorkingArea.Height)
                    keypad.Location = new Point(this.Location.X, p.Y + 60);
                else
                    keypad.Location = new Point(this.Location.X, this.PointToScreen(currentAlphanumericTextBox.Location).Y - keypad.Height);
                keypad.Show();
            }
            else if (keypad.Visible)
                keypad.Hide();
            else
            {
                keypad.Show();
            }
            log.LogMethodExit();
        }

        private void DiscountCouponIssueUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.Close();
            log.LogMethodExit();
        }

        private void textbox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentAlphanumericTextBox = sender as Control;
            log.LogMethodExit();
        }

        private void dgvDiscountCouponsDTOList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentAlphanumericTextBox = e.Control;
            log.LogMethodExit();
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dtpEffectiveFrom.Value < ServerDateTime.Now.Date)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(2836));
                return;
            }
            if (dtpExpiresOn.Value < dtpEffectiveFrom.Value)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1899));
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFrom.Text))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", lblFrom.Text.Replace(":","")));
                return;
            }
            if (dtpExpiresOn.Value > Convert.ToDateTime(discountCouponsHeaderDTO.ExpiryDate.Value.AddHours(businessStartTime).ToString("dddd, dd-MMM-yyyy h:mm tt")))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(5175));
                return;
            }
            DiscountCouponsHeaderBL discountCouponsHeaderBL = new DiscountCouponsHeaderBL(utilities.ExecutionContext, discountCouponsHeaderDTO);
            discountCouponsDTOList = discountCouponsHeaderBL.IssueCoupons(txtFrom.Text.Replace(" ", ""), dtpEffectiveFrom.Value,dtpExpiresOn.Value, productQuantity);
            discountCouponsDTOListBS.DataSource = discountCouponsDTOList;
            log.LogMethodExit();
        }

        private void btnIssueCoupons_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrWhiteSpace(txtFrom.Text))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", lblFrom.Text.Replace(":", "")));
                return;
            }
            int count = discountCouponsHeaderDTO.Count == null ? 1 : Convert.ToInt32(discountCouponsHeaderDTO.Count*productQuantity);
            log.LogVariableState("count" , count);
            if (discountCouponsDTOList == null || discountCouponsDTOList.Count < count)
            {
                btnGO.PerformClick();
                //only one coupon to be generated.
                if (count != 1)
                {
                    return;
                }
            }
            string message = MessageContainerList.GetMessage(utilities.ExecutionContext,684);
            bool valid = BackgroundProcessRunner.Run<bool>(()=> { return ValidateDiscountCouponsDTOList(discountCouponsDTOList);}, message, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
            if(valid == false)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1128));
                return;
            }
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }

        private bool ValidateDiscountCouponsDTOList(List<DiscountCouponsDTO> discountCouponsDTOList)
        {
            log.LogMethodEntry(discountCouponsDTOList);
            bool valid = true;
            try
            {
                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(utilities.ExecutionContext, discountCouponsDTOList);
                discountCouponsListBL.Validate();
            }
            catch (ValidationException)
            {
                valid = false;
            }
            catch (Exception ex)
            {
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private bool ValidateDiscountCouponsDTO(DiscountCouponsDTO discountCouponsDTO)
        {
            log.LogMethodEntry(discountCouponsDTO);
            bool valid = true;
            try
            {
                DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(utilities.ExecutionContext, discountCouponsDTO);
                discountCouponsBL.ValidateCouponDefinition();
            }
            catch (DuplicateCouponException)
            {
                valid = false;
            }
            catch (ForeignKeyException)
            {
                valid = false;
            }
            catch (Exception)
            {
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            log.LogMethodExit();
        }

        private void dgvDiscountCouponsDTOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvDiscountCouponsDTOList.RowCount > 0)
            {
                for (int i = 0; i < dgvDiscountCouponsDTOList.RowCount; i++)
                {
                    dgvDiscountCouponsDTOList.Rows[i].Cells[serialNumber.Index].Value = Convert.ToString(i + 1);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the issued discount coupon list.
        /// </summary>
        public List<DiscountCouponsDTO> DiscountCouponsDTOList
        {
            get
            {
                return discountCouponsDTOList;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
