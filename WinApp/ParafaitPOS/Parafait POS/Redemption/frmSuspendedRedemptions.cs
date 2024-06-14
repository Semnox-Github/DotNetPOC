/********************************************************************************************
* Project Name - Parafait_POS - Redemption
* Description  - frmSuspendedRedemptions 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.8.0       26-Sep-2019      Dakshakh           Redemption currency rule enhancement         
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Redemption;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS.Redemption
{
    public partial class frmSuspendedRedemptions : Form
    {
        Utilities _utilities;
        List<RedemptionCurrencyRuleBL> redemptionCurrencyRuleBLList;
        //List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleList;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        string cardWithSuspendedRedemption=null;
        private string parentScreenNumber = string.Empty;

        public delegate void RetrievedIDDelegate(object retrievedId);
        public RetrievedIDDelegate setRetrievedParamsCallback;
        public delegate void SetLastActivityTimeDelegate();
        public SetLastActivityTimeDelegate SetLastActivityTime;
        /// <summary>
        /// frmSuspendedRedemptions
        /// </summary>
        /// <param name="utils">utils</param>
        public frmSuspendedRedemptions(Utilities utils, List<RedemptionCurrencyRuleBL> redemptionCurrencyRuleBLList)
        {
            log.LogMethodEntry(utils,redemptionCurrencyRuleBLList);
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            InitializeComponent();
            this.label1.Location = new System.Drawing.Point(this.dgvSuspended.Width / 2, 0);
            _utilities = utils;
            _utilities.setLanguage(this);//added on 26-Jul-2017
            this.redemptionCurrencyRuleBLList = redemptionCurrencyRuleBLList;
            log.LogMethodExit();
        }

        /// <summary>
        /// frmSuspendedRedemptions
        /// </summary>
        /// <param name="utils">utils</param>
        /// <param name="cardNumber">cardNumber</param>
        public frmSuspendedRedemptions(Utilities utils, string cardNumber, List<RedemptionCurrencyRuleBL> redemptionCurrencyRuleBLList) 
            : this(utils, redemptionCurrencyRuleBLList)
        {
            log.LogMethodEntry(utils, cardNumber);
            this.cardWithSuspendedRedemption = cardNumber;
            log.LogMethodExit();
         }

        /// <summary>
        /// frmSuspendedRedemptions_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmSuspendedRedemptions_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                dcETickets.DefaultCellStyle =
                dcGiftCount.DefaultCellStyle =
                dcManualTickets.DefaultCellStyle =
                dcRedeemed.DefaultCellStyle =
                dcCurrencyTickets.DefaultCellStyle =
                dcVoucherTickets.DefaultCellStyle = _utilities.gridViewNumericCellStyle();
                dcTimeSuspended.DefaultCellStyle = _utilities.gridViewDateTimeCellStyle();
                SetParentScreenNumber();
                refresh(cardWithSuspendedRedemption);
                SetLastActivityTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, parentScreenNumber));
            }
            log.LogMethodExit();
        }

       

        /// <summary>
        /// refresh
        /// </summary>
        /// <param name="cardNumber">cardNumber</param>
        void refresh(string cardNumber=null)
        {
            log.LogMethodEntry(cardNumber);
            /*DataTable dt = _utilities.executeDataTable(@"select * from EventLog where Category = 'REDEMPTION-SUSPEND' 
                                                                                                                        and (Username = @loginId or @all = 1)",*/
            DataTable dt;
            if (cardNumber == null || chkShowAll.Checked)
            {
                dt = _utilities.executeDataTable(@"select * from SuspendedRedemption where Category = 'REDEMPTION-SUSPEND' 
                                                            and (Username = @loginId or @all = 1)",
                                                            new System.Data.SqlClient.SqlParameter("@loginId", _utilities.ParafaitEnv.LoginID),
                                                            new System.Data.SqlClient.SqlParameter("@all", chkShowAll.Checked));
            }
            else
            {
                  dt = _utilities.executeDataTable(@"select * from SuspendedRedemption where Category = 'REDEMPTION-SUSPEND'  
                                                            and Value like @cardNum ",
                                                          new System.Data.SqlClient.SqlParameter("@cardNum", "%<" + cardNumber + ">%"));
            }

            string message = "";
            dgvSuspended.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                string id = dr["data"].ToString();
                clsRedemption red = new clsRedemption(_utilities);
                if (red.retrieveSuspended(id, ref message))
                    message = "";
                red.ApplyCurrencyRule(redemptionCurrencyRuleBLList);
                
                int giftCount = 0;
                foreach(clsRedemption.clsProducts item in red.productList)
                    giftCount += item.Quantity;

                dgvSuspended.Rows.Add(id,
                    red.cardList.Count > 0 ? red.cardList[0].cardNumber : "",
                    red.getETickets(),
                    red.getPhysicalTickets(),
                    red.getManualTickets(),
                    red.getCurrencyTickets(),
                    red.getTotalRedeemed(),
                    giftCount,
                    dr["Timestamp"],
                    message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// chkShowAll_CheckedChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (string.IsNullOrEmpty(cardWithSuspendedRedemption) == false && chkShowAll.Checked == false)
                    refresh(cardWithSuspendedRedemption);
                else
                    refresh();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, parentScreenNumber));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnClose_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                string emptyRetrive = null;
                setRetrievedParamsCallback(emptyRetrive);
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, parentScreenNumber));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnDelete_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (dgvSuspended.CurrentRow != null)
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Are you sure you want to delete?"),
                        MessageContainerList.GetMessage(_utilities.ExecutionContext, "Confirm Delete" )+
                        MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, parentScreenNumber), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        _utilities.executeScalar("delete from SuspendedRedemption where data = @id",
                                              new System.Data.SqlClient.SqlParameter("@id", dgvSuspended.CurrentRow.Cells["dcData"].Value));
                        if (string.IsNullOrEmpty(cardWithSuspendedRedemption) == false && chkShowAll.Checked == false)
                            refresh(cardWithSuspendedRedemption);
                        else
                            refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }
         
        /// <summary>
        /// btnRetrieve_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (dgvSuspended.CurrentRow != null)
                {
                    object retrievedId = dgvSuspended.CurrentRow.Cells["dcData"].Value;
                    //retrieve = true;//Added 7-Jun-2017
                    //Commented 7-Jun-2017
                    //this.DialogResult = System.Windows.Forms.DialogResult.OK; 
                    setRetrievedParamsCallback(retrievedId); //End update 7-Jun-2017
                    Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvSuspended_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime(); 
            }
            catch (Exception ex)
            {
                log.Error(ex); 
            }
            log.LogMethodExit();
        }

        private void dgvSuspended_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetParentScreenNumber()
        {
            log.LogMethodEntry();
            try
            {
                frmScanAndRedeem parentForm = this.Owner as frmScanAndRedeem;
                if (parentForm != null)
                {
                    parentScreenNumber = parentForm.GetCurrentScreenNumber;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
