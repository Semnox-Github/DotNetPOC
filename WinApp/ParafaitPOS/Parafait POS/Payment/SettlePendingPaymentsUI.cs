using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.logging;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    /// <summary>
    /// Used to settle pending payments.
    /// </summary>
    public partial class SettlePendingPaymentsUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Dictionary<int, PaymentGateway> paymentgateways = new Dictionary<int, PaymentGateway>();

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities">Parafait utilities</param>
        public SettlePendingPaymentsUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            this.utilities = utilities;
            InitializeComponent();
            utilities.setupDataGridProperties(ref dgvUnsettledPayments);
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
            log.LogMethodExit(null);
        }

        private void SettlePendingPaymentsUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Application.DoEvents();
            PopulateUserCombo();
            InitializePaymentGateways();
            LoadUnsettledPayments();
            selectedDataGridViewCheckBoxColumn.HeaderText = selectedDataGridViewCheckBoxColumn.HeaderText + "\n";
            PrintMerchantReceipt.HeaderText = PrintMerchantReceipt.HeaderText + "\n";
            log.LogMethodExit(null);
        }

        private void PopulateUserCombo()//starts: 2017-05-13 changes
        {
            log.LogMethodEntry();
            List<UsersDTO> usersDTOList = null;
            UsersList usersList = new UsersList(machineUserContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
            usersDTOList = usersList.GetAllUsers(searchParameters);
            if(usersDTOList == null)
            {
                usersDTOList = new List<UsersDTO>();
            }
            usersDTOList.Insert(0, new UsersDTO());
            usersDTOList[0].LoginId = "All";
            usersDTOList[0].UserName = "All";
            if(utilities.ParafaitEnv.LoginID.Equals("semnox") == false)
            {
                UsersDTO semnoxUserDTO = null;
                for(int i = 0; i < usersDTOList.Count; i++)
                {
                    if(usersDTOList[i].LoginId == "semnox")
                    {
                        semnoxUserDTO = usersDTOList[i];
                    }
                }
                if(semnoxUserDTO != null)
                {
                    usersDTOList.Remove(semnoxUserDTO);
                }
            }
            cmbUser.DataSource = usersDTOList;
            cmbUser.ValueMember = "LoginId";
            cmbUser.DisplayMember = "LoginId";
            cmbUser.SelectedValue = utilities.ParafaitEnv.LoginID;
            if(utilities.ParafaitEnv.Manager_Flag.Equals("Y") || utilities.ParafaitEnv.LoginID.Equals("semnox"))
            {
                cmbUser.Enabled = true;
            }
            else
            {
                cmbUser.Enabled = false;
            }
            log.LogMethodExit(null);
        }

        private void InitializePaymentGateways()
        {
            log.LogMethodEntry();
            try
            {
                PaymentModeList paymentModesListBL = new PaymentModeList(machineUserContext);
                List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(true);
                if(paymentModesDTOList != null)
                {
                    foreach(var paymentModesDTO in paymentModesDTOList)
                    {
                        try
                        {
                            PaymentMode paymentModesBL = new PaymentMode(machineUserContext, paymentModesDTO);
                            paymentgateways.Add(paymentModesDTO.PaymentModeId, PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModesBL.Gateway));
                        }
                        catch (Exception ex1)
                        {
                            log.Error("Error occurred while initializing Payment Gateway", ex1);
                            log.Error("*" + ex1.Message);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while initializing Payment Gateway", ex); 
                paymentgateways = new Dictionary<int, PaymentGateway>();
            }
            log.LogMethodExit(null);
        }

        private void LoadUnsettledPayments()
        {
            log.LogMethodEntry();
            List<TransactionPaymentsDTO> unsettledTransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
            TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
            foreach(var paymentGateway in paymentgateways)
            {
                List<CCTransactionsPGWDTO> pendingCCTransactionsPGWDTOList = paymentGateway.Value.GetAllUnsettledCreditCardTransactions();
                if(pendingCCTransactionsPGWDTOList != null && pendingCCTransactionsPGWDTOList.Count > 0)
                {
                    List<int> responseIdList = new List<int>();
                    foreach(var cCTransactionsPGWDTO in pendingCCTransactionsPGWDTOList)
                    {
                        responseIdList.Add(cCTransactionsPGWDTO.ResponseID);
                    }
                    List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    //searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.POS_MACHINE, utilities.ParafaitEnv.POSMachine));//2017-09-27 commented
                    if (cmbUser.SelectedValue.ToString() != "All")
                    {
                        searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.LAST_UPDATED_USER, cmbUser.SelectedValue.ToString()));
                    }
                    if(string.IsNullOrEmpty(txtTrxid.Text) == false)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, txtTrxid.Text));
                    }
                    searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentGateway.Key.ToString()));
                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(searchParameters, responseIdList);
                    if(transactionPaymentsDTOList != null)
                    {
                        unsettledTransactionPaymentsDTOList.AddRange(transactionPaymentsDTOList);
                    }
                }
            }
            pendingTransactionPaymentDTOListBS.DataSource = unsettledTransactionPaymentsDTOList;
            log.LogMethodExit(null);
        }

        private void dgvUnsettledPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex >= 0)
            {
                if (dgvUnsettledPayments.Columns[e.ColumnIndex].Name.Equals("selectedDataGridViewCheckBoxColumn"))
                {
                    DataGridViewCheckBoxCell cell = dgvUnsettledPayments.Rows[e.RowIndex].Cells[selectedDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell;
                    if (cell != null)
                    {
                        cell.Value = !Convert.ToBoolean(cell.Value);
                    }
                }
                else if (dgvUnsettledPayments.Columns[e.ColumnIndex].Name.Equals("PrintMerchantReceipt"))
                {
                    DataGridViewCheckBoxCell cell = dgvUnsettledPayments.Rows[e.RowIndex].Cells[PrintMerchantReceipt.Index] as DataGridViewCheckBoxCell;
                    if (cell != null)
                    {
                        cell.Value = !Convert.ToBoolean(cell.Value);
                    }
                }
            }

            log.LogMethodExit(null);
        }

        private void dgvUnsettledPayments_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            for(int i = 0; i < dgvUnsettledPayments.RowCount; i++)
            {
                DataGridViewCheckBoxCell cell = dgvUnsettledPayments.Rows[i].Cells[selectedDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell;
                if(cell != null)
                {
                    cell.Value = false;
                }
                cell = dgvUnsettledPayments.Rows[i].Cells[PrintMerchantReceipt.Index] as DataGridViewCheckBoxCell;
                if (cell != null)
                {
                    cell.Value = false;
                }
            }
            log.LogMethodExit(null);
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);            
            bool isError = false;
            bool IsForcedSettlement = true;//2017-09-27
            int counter = 0;
            Transaction transaction;
            string message="";
            TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
            List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters;
            List<TransactionPaymentsDTO> transactionPaymentsfilteredDTOList;
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            textBoxMessageLine.Text = "";
            foreach (DataGridViewRow dr in dgvUnsettledPayments.Rows)
            {                
                if (dr.Cells["selectedDataGridViewCheckBoxColumn"].Value.Equals(true))
                {
                    counter++;
                    IsForcedSettlement = false;
                }
                if (counter > 1)
                {
                    IsForcedSettlement = true;
                    break;
                }
            }//2017-09-27
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = pendingTransactionPaymentDTOListBS.DataSource as List<TransactionPaymentsDTO>;
            if(transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dgvUnsettledPayments.RowCount; i++)
                    {
                        DataGridViewCheckBoxCell cell = dgvUnsettledPayments.Rows[i].Cells[selectedDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell;
                        if (cell != null && Convert.ToBoolean(cell.Value) == true)
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = transactionPaymentsDTOList[i];
                            if (paymentgateways.ContainsKey(transactionPaymentsDTO.PaymentModeId))
                            {
                                PaymentGateway paymentGateway = paymentgateways[transactionPaymentsDTO.PaymentModeId];
                                cell = dgvUnsettledPayments.Rows[i].Cells[PrintMerchantReceipt.Index] as DataGridViewCheckBoxCell;
                                if (cell != null && Convert.ToBoolean(cell.Value) == true)
                                {
                                    paymentGateway.PrintReceipt = true;
                                }
                                else
                                {
                                    paymentGateway.PrintReceipt = false;
                                }
                                transaction = new Transaction(utilities);
                                transaction.Trx_id = transactionPaymentsDTO.TransactionId;
                                transaction.getTotalPaidAmount(null);
                                paymentGateway.SetTransactionAmount(Convert.ToDecimal(transaction.TotalPaidAmount));
                                paymentGateway.SetTotalTipAmountEntered(Convert.ToDecimal(transaction.Tip_Amount));
                                //TrxUserLogsBL trxUserLogsBL = new TrxUserLogsBL();
                                //paymentGateway.ApproverId = -1; 
                                
                                TransactionPaymentsDTO settledTransactionPaymentsDTO = paymentGateway.PerformSettlement(transactionPaymentsDTO, IsForcedSettlement);//2017-09-27
                                if (settledTransactionPaymentsDTO != null)
                                {
                                    settledTransactionPaymentsDTO.PosMachine = utilities.ParafaitEnv.POSMachine;
                                    TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(utilities.ExecutionContext, settledTransactionPaymentsDTO);
                                    transactionPaymentsBL.Save();
                                    try
                                    {
                                        TrxUserLogsBL trxUserLogs = new TrxUserLogsBL(settledTransactionPaymentsDTO.TransactionId, -1, utilities.ParafaitEnv.LoginID, DateTime.Now, utilities.ParafaitEnv.POSMachineId, "CCSETTLEMENT", "Transaction id "+ settledTransactionPaymentsDTO.TransactionId+"(Payment Id:"+ settledTransactionPaymentsDTO .PaymentId+ ") is settled with tipamount " + settledTransactionPaymentsDTO.TipAmount, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.LoginID, utilities.ExecutionContext);
                                        trxUserLogs.Save(null);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Error occurred when inserting into TrxUserLogs ", ex);
                                    }

                                    searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                                    searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, settledTransactionPaymentsDTO.TransactionId.ToString()));
                                    transactionPaymentsfilteredDTOList = transactionPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(searchParameters);

                                    if (transactionPaymentsfilteredDTOList != null && transactionPaymentsfilteredDTOList.Count > 0)
                                    {
                                        foreach (TransactionPaymentsDTO transactionPaymentsfilteredDTO in transactionPaymentsfilteredDTOList)
                                        {
                                            if (paymentGateway.IsSettlementPending(transactionPaymentsfilteredDTO))
                                            {
                                                textBoxMessageLine.Text = utilities.MessageUtils.getMessage(2206) + utilities.MessageUtils.getMessage(2207, settledTransactionPaymentsDTO.TransactionId);
                                                log.Debug("There are few more transactions to complete.");
                                                LoadUnsettledPayments();
                                                isError = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!isError)
                                    {
                                        transaction = transactionUtils.CreateTransactionFromDB(transactionPaymentsDTO.TransactionId, utilities);
                                        if (transaction.SaveTransacation(ref message) != 0)
                                        {
                                            textBoxMessageLine.Text = "Transaction Id: " + settledTransactionPaymentsDTO.TransactionId + " : " + " " + utilities.MessageUtils.getMessage(2206) + message;
                                            return;
                                        }
                                        else
                                        {
                                            textBoxMessageLine.Text = "Transaction Id: " + settledTransactionPaymentsDTO.TransactionId + " : " + utilities.MessageUtils.getMessage(2206);
                                        }
                                    }
                                    else
                                    {
                                        isError = false;
                                    }

                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                    LoadUnsettledPayments();
                    textBoxMessageLine.Text = utilities.MessageUtils.getMessage("Process is successfully completed.") + ((IsForcedSettlement) ? "" : textBoxMessageLine.Text);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while performing settlement", ex);
                    textBoxMessageLine.Text = "Error occurred while performing settlement. " + ex.Message;
                }
            }
            else
            {
                textBoxMessageLine.Text = utilities.MessageUtils.getMessage("No transactions to complete!...");
            }
            log.LogMethodExit(null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit(null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadUnsettledPayments();
            log.LogMethodExit(null);
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            showNumberPadForm('-');
            log.LogMethodExit(null);
        }
        void showNumberPadForm(char firstKey)
        {
            log.LogMethodEntry();
            double varAmount = NumberPadForm.ShowNumberPadForm(utilities.MessageUtils.getMessage("Enter Trx Id"), firstKey, utilities);            
            txtTrxid.Text = (Convert.ToInt32(varAmount)<0?"": Convert.ToInt32(varAmount).ToString());
            log.LogMethodExit();
        }

        private void chbTrxSelect_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            for (int i = 0; i < dgvUnsettledPayments.RowCount; i++)
            {
                DataGridViewCheckBoxCell cell = dgvUnsettledPayments.Rows[i].Cells[selectedDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell;
                if (cell != null)
                {
                    cell.Value = chbTrxSelect.Checked;
                }
            }
            log.LogMethodExit(null);
        }

        private void chbPrintMerchantCopy_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            for (int i = 0; i < dgvUnsettledPayments.RowCount; i++)
            {
                DataGridViewCheckBoxCell cell = dgvUnsettledPayments.Rows[i].Cells[PrintMerchantReceipt.Index] as DataGridViewCheckBoxCell;
                if (cell != null)
                {
                    cell.Value = chbPrintMerchantCopy.Checked;
                }
            }
            log.LogMethodExit(null);
        }
    }
}
