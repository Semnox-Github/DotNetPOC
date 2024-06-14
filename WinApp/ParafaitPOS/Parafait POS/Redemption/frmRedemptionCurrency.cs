/********************************************************************************************
* Project Name - Parafait_POS - Redemption
* Description  - frmRedemptionCurrency 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.8.0       26-Sep-2019      Dakshakh           Redemption currency rule enhancement         
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS.Redemption
{
    public partial class frmRedemptionCurrency : Form
    {
        List<clsRedemption.clsRedemptionCurrency> currencyList;
        Utilities _utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public delegate void RefreshMethodDelegate();
        public RefreshMethodDelegate setRefreshCallBack;

        public delegate void SetCurrencyRuleCheckFlagDelegate();
        public SetCurrencyRuleCheckFlagDelegate setCurrencyRuleCallBack;


        public delegate void ShortCutKeyMethodDelegate(object sender, KeyEventArgs e);
        public ShortCutKeyMethodDelegate setShortCutKeyCallBack;

        public delegate bool ProcessCmdKeyMethodDelegate(ref Message msg, Keys keyData);
        public ProcessCmdKeyMethodDelegate setProcessCmdKeyCallBack;

        public delegate void SetLastActivityTimeDelegate();
        public SetLastActivityTimeDelegate SetLastActivityTime;

        /// <summary>
        /// frmRedemptionCurrency
        /// </summary>
        /// <param name="inUtilities">inUtilities</param>
        /// <param name="_currencyList">_currencyList</param>
        public frmRedemptionCurrency(Utilities inUtilities, List<clsRedemption.clsRedemptionCurrency> _currencyList)
        {
            log.LogMethodEntry(inUtilities, _currencyList);
            _utilities = inUtilities;
            currencyList = _currencyList;
            InitializeComponent();
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmRedemptionCurrency_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            redemptionCurrencyListBS.DataSource = currencyList;
            redemptionCurrencyListBS.ResetBindings(false);
            SetDGVFormat();
            SetFocus();
            log.LogMethodExit();
        }

        /// <summary>
        /// frmRedemptionCurrency_Activated
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmRedemptionCurrency_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            redemptionCurrencyListBS.DataSource = currencyList;
            redemptionCurrencyListBS.ResetBindings(false);
            SetDGVFormat();
            SetFocus();
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvRedemptionCurrency_CellClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvRedemptionCurrency_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == deleteButton.Index && e.RowIndex >= 0 && dgvRedemptionCurrency[currencyNameDataGridViewTextBoxColumn.Index, e.RowIndex].Value != null)
                    {
                        int currencyId = Convert.ToInt32(dgvRedemptionCurrency[currencyIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value);
                        if (currencyId != -1)
                        {
                            clsRedemption.clsRedemptionCurrency foundItem = currencyList.Find(delegate (clsRedemption.clsRedemptionCurrency item) { return (item.currencyId == currencyId); });
                            if (foundItem != null)
                            {
                                redemptionCurrencyListBS.RemoveAt(e.RowIndex);
                                dgvRedemptionCurrency.Refresh();
                            }
                        }

                    }
                    else if (e.ColumnIndex == quantityDataGridViewTextBoxColumn.Index && dgvRedemptionCurrency[currencyNameDataGridViewTextBoxColumn.Index, e.RowIndex].Value != null && Convert.ToInt32(dgvRedemptionCurrency["currencyIdDataGridViewTextBoxColumn", e.RowIndex].Value) != -1)
                    {
                        int Quantity = Convert.ToInt32(NumberPadForm.ShowNumberPadForm(_utilities.MessageUtils.getMessage(10039), '-', _utilities));
                        if (Quantity <= 0)
                        {
                            log.Error("Ends-dgvRedemptionCurrency_CellClick() as Quantity <= 0");
                            return;
                        }
                        dgvRedemptionCurrency[quantityDataGridViewTextBoxColumn.Index, e.RowIndex].Value = Quantity;
                        int valueInTickets = Convert.ToInt32(dgvRedemptionCurrency[valueInTicketsDataGridViewTextBoxColumn.Index, e.RowIndex].Value);
                        int total = Quantity * valueInTickets;
                        dgvRedemptionCurrency[TotalTickets.Index, e.RowIndex].Value = total;
                    }
                }
                else
                {
                    log.Error("Ends-dgvRedemptionCurrency_CellClick() as Select Valid Index");
                    return;

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
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
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvRedemptionCurrency_DataBindingComplete
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvRedemptionCurrency_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (redemptionCurrencyListBS.Current != null && redemptionCurrencyListBS.Current is clsRedemption.clsRedemptionCurrency)
                {
                    for (int i = 0; i < redemptionCurrencyListBS.Count; i++)
                    {
                        if (currencyList[i].redemptionCurrencyRuleId > -1)
                        {
                            dgvRedemptionCurrency[TotalTickets.Index, i].Value = currencyList[i].redemptionRuleTicketDelta * currencyList[i].quantity;
                            dgvRedemptionCurrency[valueInTicketsDataGridViewTextBoxColumn.Index, i].Value = currencyList[i].redemptionRuleTicketDelta;

                            clsRedemption.clsRedemptionCurrency item = redemptionCurrencyListBS.Current as clsRedemption.clsRedemptionCurrency;
                        }
                        else
                        {
                            dgvRedemptionCurrency[valueInTicketsDataGridViewTextBoxColumn.Index, i].Value = currencyList[i].ValueInTickets;
                            dgvRedemptionCurrency[TotalTickets.Index, i].Value = currencyList[i].ValueInTickets * currencyList[i].quantity;
                            clsRedemption.clsRedemptionCurrency var = redemptionCurrencyListBS.Current as clsRedemption.clsRedemptionCurrency;

                        }
                    }
                    SetFocus();
                    SetLastActivityTime();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        

        /// <summary>
        /// frmRedemptionCurrency_FormClosed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmRedemptionCurrency_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            setCurrencyRuleCallBack();
            setRefreshCallBack();
            log.LogMethodExit();
        }

        /// <summary>
        /// SetFocus
        /// </summary>
        private void SetFocus()
        {
            log.LogMethodEntry();
            if (dgvRedemptionCurrency != null && dgvRedemptionCurrency.RowCount > 0)
            {
                dgvRedemptionCurrency[deleteButton.Index, 0].Selected = false;
                dgvRedemptionCurrency[currencyNameDataGridViewTextBoxColumn.Index, 0].Selected = true;
                btnClose.Focus();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// ProcessCmdKey
        /// </summary>
        /// <param name="msg">msg</param>
        /// <param name="keyData">keyData</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            try
            {
                SetLastActivityTime();
                if (keyData.Equals(Keys.ControlKey | Keys.Control))
                {
                    setProcessCmdKeyCallBack(ref msg, keyData);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit("base.ProcessCmdKey");
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// FrmRedemptionCurrency_KeyUp
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void FrmRedemptionCurrency_KeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                setShortCutKeyCallBack(sender, e);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvRedemptionCurrency_CellEnter
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvRedemptionCurrency_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.RowIndex > -1)
                {
                    if (dgvRedemptionCurrency["currencyIdDataGridViewTextBoxColumn", e.RowIndex].Value != null
                        && Convert.ToInt32(dgvRedemptionCurrency["currencyIdDataGridViewTextBoxColumn", e.RowIndex].Value) == -1)
                    {
                        dgvRedemptionCurrency["currencyIdDataGridViewTextBoxColumn", e.RowIndex].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetDGVFormat()
        {
            log.LogMethodEntry();
            dgvRedemptionCurrency.Columns["quantityDataGridViewTextBoxColumn"].DefaultCellStyle = 
                dgvRedemptionCurrency.Columns["valueInTicketsDataGridViewTextBoxColumn"].DefaultCellStyle = 
                dgvRedemptionCurrency.Columns["TotalTickets"].DefaultCellStyle   = _utilities.gridViewNumericCellStyle();
            log.LogMethodExit();
        }

    }
}
