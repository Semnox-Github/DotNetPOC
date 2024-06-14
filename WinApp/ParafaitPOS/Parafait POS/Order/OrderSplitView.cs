/********************************************************************************************
 * Project Name - POS
 * Description  - Order split view
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      28-Nov-2018      Guru S A       Booking changes
 *2.140.0     27-Jun-2021      Fiona Lishal   Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class OrderSplitView : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private int orderId;
        OrderHeaderBL orderHeaderBL;
        List<SplitView> splitViewList;
        private Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox;

        public OrderSplitView(Utilities utilities, int orderId, Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            this.orderId = orderId;
            InitializeComponent();
            this.parafaitMessageBox = parafaitMessageBox;
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void OrderSplitView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Left = Screen.PrimaryScreen.WorkingArea.X;
            Top = Screen.PrimaryScreen.WorkingArea.Y;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            OrderHeaderList orderHeaderList = new OrderHeaderList(utilities.ExecutionContext);
            List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.ORDER_ID, orderId.ToString()));
            searchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TRANSACTION_STATUS_LIST, " 'BOOKING','RESERVED','OPEN','INITIATED','ORDERED','PREPARED','CLOSED','PENDING' "));
            List<OrderHeaderDTO> orderHeaderDTOs = orderHeaderList.GetOrderHeaderDTOList(searchParameters);
            orderHeaderBL = new OrderHeaderBL(utilities.ExecutionContext, orderHeaderDTOs[0]);
            RefreshSplitView();
            log.LogMethodExit();
        }

        private void RefreshSplitView()
        {
            log.LogMethodEntry();
            flpSplits.Controls.Clear();
            splitViewList = new List<SplitView>();
            foreach (var transaction in orderHeaderBL.GetTransactionList(utilities))
            {
                if(transaction.Status != Transaction.TrxStatus.CANCELLED && transaction.Status != Transaction.TrxStatus.SYSTEMABANDONED)
                {
                    SplitView splitView = new SplitView(utilities, parafaitMessageBox);
                    splitView.SetTransaction(transaction);
                    splitView.RefreshDisplay();
                    flpSplits.Controls.Add(splitView);
                    splitViewList.Add(splitView);
                    splitView.Click += SplitView_Click;
                    splitView.SelectionChanged += SplitView_SelectionChanged;
                    splitView.PaymentChanged += SplitView_PaymentChanged;
                }
            }
            UpdateAmounts();
            log.LogMethodExit();
        }

        private void SplitView_PaymentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateAmounts();
            log.LogMethodExit();
        }

        private void UpdateAmounts()
        {
            log.LogMethodEntry();
            decimal totalAmount = 0;
            decimal totalBalanceAmount = 0;
            if (splitViewList != null && splitViewList.Count > 0)
            {
                foreach (var splitView in splitViewList)
                {
                    totalAmount += (decimal)splitView.Transaction.Net_Transaction_Amount;
                    totalBalanceAmount += (decimal)(splitView.Transaction.Net_Transaction_Amount - splitView.Transaction.TotalPaidAmount);
                }
            }
            lblTotalAmount.Text = totalAmount.ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            lblTotalBalanceAmount.Text = totalBalanceAmount.ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            log.LogMethodExit();
        }

        private void SplitView_SelectionChanged(object sender, EventArgs e)
        {
            SplitView selectionChnagedSplitView = sender as SplitView;
            if(selectionChnagedSplitView.GetSelectedRows().Count > 0)
            {
                foreach (var splitView in splitViewList)
                {
                    if(splitView != selectionChnagedSplitView)
                    {
                        splitView.ClearSelectedRows();
                    }
                }
            }
        }

        private void SplitView_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SplitView destinationSplitView = sender as SplitView;
            SplitView sourceSplitView = null;
            foreach (var splitView in splitViewList)
            {
                if(destinationSplitView != splitView && splitView.GetSelectedRows().Count > 0)
                {
                    sourceSplitView = splitView;
                    break;
                }
            }
            if(sourceSplitView == null)
            {
                parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1724), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                log.LogMethodExit(null, "No Items selected");
                return;
            }
            List<Transaction.TransactionLine> transactionLines = sourceSplitView.GetOneLinePerSelectedRow();
            if (IsDiscountedLines(transactionLines))
            {
                DialogResult result = parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1722), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    log.LogMethodExit(null, "User cancelled to move discounted lines");
                    return;
                }
            }
            
            destinationSplitView.AddLines(sourceSplitView.Transaction, transactionLines);
            sourceSplitView.RemoveLines(transactionLines);
            sourceSplitView.Transaction.SetServiceCharges(null);
            sourceSplitView.Transaction.SetAutoGratuityAmount(null);
            sourceSplitView.Transaction.updateAmounts();
            sourceSplitView.RefreshDisplay();
            destinationSplitView.Transaction.updateAmounts();
            destinationSplitView.Transaction.SetServiceCharges(null);
            destinationSplitView.Transaction.SetAutoGratuityAmount(null);
            destinationSplitView.Transaction.updateAmounts();
            destinationSplitView.Transaction.CalculateOrderTypeGroup();
            destinationSplitView.RefreshDisplay();
            log.LogMethodExit();
        }

        bool IsAnySplitChanged()
        {
            log.LogMethodEntry();
            bool result = false;
            foreach (var splitView in GetOpenSplitViews())
            {
                if(splitView.IsChanged)
                {
                    result = true;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private void btnAddSplit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                List<SplitView> openSplitViewList = GetOpenSplitViews();
                if(openSplitViewList.Count == 0)
                {
                    log.LogMethodExit(null, "No Open transactions");
                    return;
                }
                int noOfSplits = (int)NumberPadForm.ShowNumberPadForm("No of Splits", '0', utilities);
                if(noOfSplits > 0)
                {
                    Application.UseWaitCursor = true;
                    for (int i = 0; i < noOfSplits; i++)
                    {
                        Transaction transaction = new Transaction(utilities);
                        transaction.PrimaryCard = openSplitViewList[0].Transaction.PrimaryCard;
                        transaction.EntitlementReferenceDate = openSplitViewList[0].Transaction.EntitlementReferenceDate;
                        transaction.customerDTO = openSplitViewList[0].Transaction.customerDTO;
                        CopyManualDiscounts(transaction, openSplitViewList[0].Transaction);
                        orderHeaderBL.AddTransactions(new List<Transaction>() { transaction }, utilities);
                        SplitView splitView = new SplitView(utilities, parafaitMessageBox);
                        splitView.SetTransaction(transaction);
                        splitView.RefreshDisplay();
                        splitViewList.Add(splitView);
                        flpSplits.Controls.Add(splitView);
                        splitView.Click += SplitView_Click;
                        splitView.SelectionChanged += SplitView_SelectionChanged;
                        splitView.PaymentChanged += SplitView_PaymentChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
            }
            Application.UseWaitCursor = false;
            log.LogMethodExit();
        }

        private void CopyManualDiscounts(Transaction newSplit, Transaction template)
        {
            log.LogMethodEntry(newSplit, template);
            if(template.DiscountApplicationHistoryDTOList != null)
            {
                newSplit.DiscountApplicationHistoryDTOList = new List<DiscountApplicationHistoryDTO>();
                foreach (var discountApplicationHistoryDTO in template.DiscountApplicationHistoryDTOList)
                {
                    DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(utilities.ExecutionContext, discountApplicationHistoryDTO.DiscountId);
                    if(discountContainerDTO == null)
                    {
                        continue;
                    }
                    if(discountContainerDTO.AutomaticApply == "N" &&
                       discountContainerDTO.CouponMandatory == "N" && discountApplicationHistoryDTO.TransactionLineBL == null)
                    {
                        newSplit.DiscountApplicationHistoryDTOList.Add(GetDiscountApplicationHistoryCopy(discountApplicationHistoryDTO));
                    }
                }
            }
            log.LogMethodExit();
        }

        private DiscountApplicationHistoryDTO GetDiscountApplicationHistoryCopy(DiscountApplicationHistoryDTO discountApplicationHistoryDTO)
        {
            log.LogMethodEntry(discountApplicationHistoryDTO);
            DiscountApplicationHistoryDTO newDiscountApplicationHistoryDTO = new DiscountApplicationHistoryDTO();
            newDiscountApplicationHistoryDTO.ApprovedBy = discountApplicationHistoryDTO.ApprovedBy;
            newDiscountApplicationHistoryDTO.CouponNumber = discountApplicationHistoryDTO.CouponNumber;
            newDiscountApplicationHistoryDTO.DiscountId = discountApplicationHistoryDTO.DiscountId;
            newDiscountApplicationHistoryDTO.Remarks = discountApplicationHistoryDTO.Remarks;
            newDiscountApplicationHistoryDTO.VariableDiscountAmount = discountApplicationHistoryDTO.VariableDiscountAmount;
            newDiscountApplicationHistoryDTO.TransactionLineBL = discountApplicationHistoryDTO.TransactionLineBL;
            log.LogMethodExit(newDiscountApplicationHistoryDTO);
            return newDiscountApplicationHistoryDTO;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(IsAnySplitChanged())
            {
                log.LogMethodExit(null, "Please save the record.");
                parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                return;
            }
            Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Application.UseWaitCursor = true;
                SqlConnection sqlConnection = utilities.getConnection();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    foreach (var splitView in splitViewList)
                    {
                        splitView.Save(sqlTransaction);
                    }
                    sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                log.Error("Error occured while saving the split", ex);
            }
            RefreshData();
            Application.UseWaitCursor = false;
            log.LogMethodExit();
        }

        private void btnSplitEqual_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Application.UseWaitCursor = true;
                SplitView selectedSplitView = null;
                foreach (var splitView in splitViewList)
                {
                    if (splitView.GetSelectedRows().Count > 0)
                    {
                        selectedSplitView = splitView;
                        break;
                    }
                }
                List<SplitView> openTransactionSplitView = GetOpenSplitViews();
                if (openTransactionSplitView.Count < 2)
                {
                    Application.UseWaitCursor = false;
                    log.LogMethodExit(null, "Atleast two transaction should be open for split line to work");
                    return;
                }
                if (selectedSplitView != null)
                {
                    decimal proportion = 1m / (decimal)openTransactionSplitView.Count;
                    List<Transaction.TransactionLine> selectedLines = selectedSplitView.GetAllLinesOfSelectedRows();
                    if(IsDiscountedLines(selectedLines))
                    {
                        DialogResult result = parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1722), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.YesNo);
                        if (result != DialogResult.Yes)
                        {
                            Application.UseWaitCursor = false;
                            log.LogMethodExit(null, "User cancelled to split discounted lines");
                            return;
                        }
                    }
                    Dictionary<Transaction.TransactionLine, Transaction.TransactionLine> lineMap;
                    List<Transaction.TransactionLine> copiedSelectedLines = selectedSplitView.GetTransactionLineListCopy(selectedLines, out lineMap);
                    selectedSplitView.UpdateLineLevelDiscountHistoryDTO(selectedSplitView.Transaction, lineMap);
                    foreach (var line in copiedSelectedLines)
                    {
                        line.quantity = line.quantity * proportion;
                    }
                    
                    selectedSplitView.AddLines(selectedSplitView.Transaction, copiedSelectedLines); 
                    foreach (var splitView in openTransactionSplitView)
                    {
                        if (splitView != selectedSplitView)
                        {
                            splitView.AddLines(selectedSplitView.Transaction, copiedSelectedLines);
                        }
                    }
                    selectedSplitView.RemoveLines(selectedLines);
                    foreach (var splitView in openTransactionSplitView)
                    {
                        splitView.Transaction.updateAmounts();
                        splitView.Transaction.SetServiceCharges(null);
                        splitView.Transaction.SetAutoGratuityAmount(null);
                        splitView.Transaction.updateAmounts();
                        splitView.Transaction.CalculateOrderTypeGroup();
                        splitView.RefreshDisplay();
                    }
                }
                else
                {
                    parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1724), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
            }
            Application.UseWaitCursor = false;
            log.LogMethodExit();
        }

        private bool IsDiscountedLines(List<Transaction.TransactionLine> selectedLines)
        {
            log.LogMethodEntry(selectedLines);
            bool result = false;
            foreach (var line in selectedLines)
            {
                if(line.DiscountQualifierList.Any() ||
                    (line.TransactionDiscountsDTOList != null &&
                    line.TransactionDiscountsDTOList.Count > 0))
                {
                    result = true;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        

        private List<SplitView> GetOpenSplitViews()
        {
            List<SplitView> openTransactionSplitView = new List<SplitView>();
            foreach (var splitView in splitViewList)
            {
                if (splitView.Transaction.Status == Transaction.TrxStatus.OPEN || splitView.Transaction.Status == Transaction.TrxStatus.INITIATED
                    || splitView.Transaction.Status == Transaction.TrxStatus.ORDERED || splitView.Transaction.Status == Transaction.TrxStatus.PREPARED)
                {
                    openTransactionSplitView.Add(splitView);
                }
            }

            return openTransactionSplitView;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Application.UseWaitCursor = true;
                RefreshData();
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
            }
            Application.UseWaitCursor = false;
            log.LogMethodExit();
        }

        private void btnUndoSplit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (IsAnySplitChanged())
            {
                log.LogMethodExit(null, "Please save the record.");
                parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                return;
            }
            try
            {
                Application.UseWaitCursor = true;
                List<SplitView> openSplitViews = GetOpenSplitViews();
                if (openSplitViews.Count > 1)
                {
                    Transaction mergedTransaction = openSplitViews[0].Transaction;
                    SqlConnection sqlConnection = utilities.getConnection();
                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        for (int i = 1; i < openSplitViews.Count; i++)
                        {
                            TransactionService transactionService = new TransactionService(utilities);
                            mergedTransaction = transactionService.MergeTransactions(mergedTransaction, openSplitViews[i].Transaction, sqlTransaction);
                        }
                        string message = string.Empty;
                        if (mergedTransaction.SaveOrder(ref message, sqlTransaction) != 0)
                        {
                            log.LogMethodExit(null, "Error occured while saving transaction : " + message);
                            throw new Exception(message);
                        }
                        sqlTransaction.Commit();
                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
            }
            Application.UseWaitCursor = false;
            log.LogMethodExit();
        }
    }
}
