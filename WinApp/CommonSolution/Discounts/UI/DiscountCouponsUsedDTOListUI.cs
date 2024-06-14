/********************************************************************************************
 * Project Name - DiscountCouponsUsedDTOListUI
 * Description  - Discount CouponsUsed DTO List UI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-APR-2019   Raghuveera      added btnImport_Click Event 
 *2.70.2      05-Aug-2019   Girish Kundar   Added LogMethodEntry() and LogMethodExit() methods.
 *2.80        27-Jun-2020   Deeksha         Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// DiscountCouponsUsedDTOListUI Class.
    /// </summary>
    public partial class DiscountCouponsUsedDTOListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private int discountCouponHeaderId;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of DiscountCouponsUsedDTOListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="discountCouponHeaderId">Discount Coupon Header Id</param>
        public DiscountCouponsUsedDTOListUI(Utilities utilities, int discountCouponHeaderId)
        {
            log.LogMethodEntry(utilities , discountCouponHeaderId);
            InitializeComponent();
            this.utilities = utilities;
            this.discountCouponHeaderId = discountCouponHeaderId;
            utilities.setupDataGridProperties(ref dgvDiscountCouponsUsedDTOList);
            if(utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void DiscountCouponsUsedListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadDiscountCouponsUsedDTOList();
            log.LogMethodExit();
        }

        private void LoadDiscountCouponsUsedDTOList()
        {
            log.LogMethodEntry();
            DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(machineUserContext);
            List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
            if(chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.IS_ACTIVE, "Y"));
            }
            if (string.IsNullOrWhiteSpace(txtTransactionId.Text) == false)
            {
                int transactionId;
                if (int.TryParse(txtTransactionId.Text, out transactionId))
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID, txtTransactionId.Text));
                }
            }
            //searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if(string.IsNullOrWhiteSpace(txtCouponNumber.Text) == false)
            {
                searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.COUPON_NUMBER, txtCouponNumber.Text.ToUpper()));
            }
            searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.DISCOUNT_COUPON_HEADER_ID, discountCouponHeaderId.ToString()));
            List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchParameters);
            log.LogVariableState("discountCouponsUsedDTOList", discountCouponsUsedDTOList);
            SortableBindingList<DiscountCouponsUsedDTO> discountCouponsUsedDTOSortableList;
            if(discountCouponsUsedDTOList != null)
            {
                discountCouponsUsedDTOSortableList = new SortableBindingList<DiscountCouponsUsedDTO>(discountCouponsUsedDTOList);
            }
            else
            {
                discountCouponsUsedDTOSortableList = new SortableBindingList<DiscountCouponsUsedDTO>();
            }
            discountCouponsUsedDTOListBS.DataSource = discountCouponsUsedDTOSortableList;
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadDiscountCouponsUsedDTOList();
            log.LogMethodExit();
        }

        private void dgvDiscountCouponsUsedDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvDiscountCouponsUsedDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            txtCouponNumber.ResetText();
            txtTransactionId.ResetText();
            RefreshData();
            log.LogMethodExit();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            using (ImportDiscountCouponUsed importDiscountCouponUsed = new ImportDiscountCouponUsed(utilities))
            {
                importDiscountCouponUsed.ShowDialog();
                btnRefresh.PerformClick();
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvDiscountCouponsUsedDTOList.AllowUserToAddRows = true;
                dgvDiscountCouponsUsedDTOList.ReadOnly = false;
            }
            else
            {
                dgvDiscountCouponsUsedDTOList.AllowUserToAddRows = false;
                dgvDiscountCouponsUsedDTOList.ReadOnly = true;
            }
            log.LogMethodExit();
        }
    }
}
