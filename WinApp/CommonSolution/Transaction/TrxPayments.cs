
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Transaction
{
    public partial class TrxPayments : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int TrxId;
        Utilities Utilities;
        public AuthenticateManagerDelegate authenticateManager;
        bool IsTipEnabled = false; //Modification on 09-Nov-2015:Tip feature
        List<TransactionPaymentsDTO> transactionPaymentsDTOList;
        public TrxPayments(int pTrxId, Utilities ParafaitUtilities, AuthenticateManagerDelegate authenticateManager)
        {
            log.LogMethodEntry(pTrxId, ParafaitUtilities);
               
            InitializeComponent();
            TrxId = pTrxId;
            Utilities = ParafaitUtilities;
            this.authenticateManager = authenticateManager;
            log.LogMethodExit(null);
        }

        private void TrxPayments_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            //Begin Modification on 09-Nov-2015:Tip feature
            DataTable dt = Utilities.executeDataTable(@"declare @TrxStatus nvarchar(50), @ref nvarchar(100)
                                                        (select @TrxStatus = Status,  @ref = PaymentReference from trx_header where TrxId= @TrxId)
                                                         select  P.PaymentModeId, ParentPaymentId, PaymentMode Mode, Amount,Convert(Numeric(18,2),isnull(TipAmount,0)) as TipAmount, c.card_number, l.Attribute ""Ent.Type"",
                                                            CreditCardNumber c_c_number, NameOnCreditCard Name_on_c_c, CreditCardName C_C_Name, 
                                                            CreditCardAuthorization ""Authorization"", Reference,(select LookupValue from LookupValues l
                                                         where tp.PaymentModeId =p.PaymentModeId  and  p.Gateway = l.LookupValueId) as Gateway,
                                                          p.isCreditCard,
                                                          p.isCash,
                                                          tp.PaymentId,
                                                          tp.CCResponseId,
                                                          tp.PaymentDate
                                                          ,tp.ApprovedBy Approved_By
                                                          ,@TrxStatus as status,
                                                        (case  when @TrxStatus != 'CLOSED' then 'N'
                                                               when  @ref like '%Reversal%' then 'N' 
		                                                       when (isDebitCard ='Y' or (isCreditCard = 'Y' and Gateway is not null)) then 'N' 
		                                                       when (1 = (Select top 1 1 from trx_header where OriginalTrxID = @TrxId)) then 'N'
			                                                   else 'Y' end)as'IsEdit'
                                                         from trxPayments tp left outer join cards c
                                                            on c.card_id = tp.cardId
                                                          left outer join LoyaltyAttributes l
                                                           on CardEntitlementType = l.CreditPlusType,
                                                            PaymentModes p
                                                          where p.PaymentModeId = tp.PaymentModeId
                                                          and tp.TrxId = @TrxId",
                                                        new SqlParameter[] { new SqlParameter("@TrxId", TrxId) });
            log.LogVariableState("@TrxId", TrxId);

            LoadTransactionPaymentsDTOList();

            dgvPayments.DataSource = dt;
            Utilities.setupDataGridProperties(ref dgvPayments);            
            dgvPayments.Columns["Amount"].DefaultCellStyle = dgvPayments.Columns["TipAmount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();

            dgvPayments.Columns["Gateway"].Visible =
                dgvPayments.Columns["isCreditCard"].Visible =
                dgvPayments.Columns["PaymentId"].Visible =
                dgvPayments.Columns["CCResponseId"].Visible =
                dgvPayments.Columns["PaymentDate"].Visible =
                dgvPayments.Columns["status"].Visible =
                dgvPayments.Columns["isCash"].Visible =
                dgvPayments.Columns["PaymentModeId"].Visible = // Added on 26-Dec-2016 for changing the payment details enhanacement
                dgvPayments.Columns["IsEdit"].Visible =  // Added on 26-Dec-2016 for changing the payment details enhanacement
            dgvPayments.Columns["ParentPaymentId"].Visible = false;


            Utilities.setLanguage(this);
            IsTipEnabled = Utilities.getParafaitDefaults("SHOW_TIP_AMOUNT_KEYPAD").Equals("Y");//End Modification on 09-Nov-2015

            log.LogMethodExit(null);
        }

        public void LoadTransactionPaymentsDTOList()
        {
            log.LogMethodEntry();

            TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
            List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
            searchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.NON_REVERSED_PAYMENT, string.Empty));
            transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchParams);

            log.LogMethodExit(null);    
        }

        private TransactionPaymentsDTO GetTransactionPaymentsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);

            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(Convert.ToInt32(dataRow["PaymentId"]),
                                                                                        dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                                                        dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                                                                                        dataRow["Amount"] == DBNull.Value ? 0d : Convert.ToDouble(dataRow["Amount"]),
                                                                                        dataRow["CreditCardNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardNumber"]),
                                                                                        dataRow["NameOnCreditCard"] == DBNull.Value ? "" : Convert.ToString(dataRow["NameOnCreditCard"]),
                                                                                        dataRow["CreditCardName"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardName"]),
                                                                                        dataRow["CreditCardExpiry"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardExpiry"]),
                                                                                        dataRow["CreditCardAuthorization"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardAuthorization"]),
                                                                                        dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                                                                        dataRow["CardEntitlementType"] == DBNull.Value ? "" : Convert.ToString(dataRow["CardEntitlementType"]),
                                                                                        dataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardCreditPlusId"]),
                                                                                        dataRow["OrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderId"]),
                                                                                        dataRow["Reference"] == DBNull.Value ? "" : Convert.ToString(dataRow["Reference"]),
                                                                                        dataRow["Guid"] == DBNull.Value ? "" : Convert.ToString(dataRow["Guid"]),
                                                                                        dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                        dataRow["CCResponseId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CCResponseId"]),
                                                                                        dataRow["Memo"] == DBNull.Value ? "" : Convert.ToString(dataRow["Memo"]),
                                                                                        dataRow["PaymentDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["PaymentDate"]),
                                                                                        dataRow["LastUpdatedUser"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                                                        dataRow["ParentPaymentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentPaymentId"]),
                                                                                        dataRow["TenderedAmount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["TenderedAmount"]),
                                                                                        dataRow["TipAmount"] == DBNull.Value ? 0d : Convert.ToDouble(dataRow["TipAmount"]),
                                                                                        dataRow["SplitId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SplitId"]),
                                                                                        dataRow["PosMachine"] == DBNull.Value ? "" : Convert.ToString(dataRow["PosMachine"]),
                                                                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                                        dataRow["CurrencyCode"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyCode"]),
                                                                                        dataRow["CurrencyRate"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["CurrencyRate"])
                                                                                        );

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        private void dgvPayments_CellMouseEnter(object sender, DataGridViewCellEventArgs e)//Begin Modification on 09-Nov-2015:Tip feature
        {
            log.LogMethodEntry(sender, e);

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit(null);
                return;
            }
            if (IsTipEnabled && !IsPaymentReversed(dgvPayments.Rows[e.RowIndex].Cells["PaymentId"].Value))
            {
                object gateway = dgvPayments.Rows[e.RowIndex].Cells["Gateway"].Value;
                
                //Check If status not cancelled && Not Reversed
                if (!dgvPayments.Rows[e.RowIndex].Cells["status"].Value.ToString().Equals("CANCELLED") && !IsPaymentReversed(dgvPayments.Rows[e.RowIndex].Cells["PaymentId"].Value))
                {
                    //if (dgvPayments.Columns[e.ColumnIndex].Name.Equals("TipAmount") && dgvPayments.Rows[e.RowIndex].Cells["TipAmount"].Value.ToString().Equals("0.00"))
                    //{
                    //    bool flag = false;
                    //    if (dgvPayments.Rows[e.RowIndex].Cells["isCash"].ToString()[0].Equals('Y'))
                    //    {
                    //        flag = true;
                    //    }
                    //    else if (!(string.IsNullOrEmpty(Convert.ToString(gateway)) && dgvPayments.Rows[e.RowIndex].Cells["isCreditCard"].ToString()[0].Equals('Y')))
                    //    {
                    //        int paymentId = Convert.ToInt32(dgvPayments.Rows[e.RowIndex].Cells["PaymentId"].Value);
                    //        TransactionPaymentsDTO transactionPaymentsDTO = null;
                    //        if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
                    //        {
                    //            foreach (var item in transactionPaymentsDTOList)
                    //            {
                    //                if (item.PaymentId == paymentId)
                    //                {
                    //                    transactionPaymentsDTO = item;
                    //                }
                    //            }
                    //        }
                    //        if (transactionPaymentsDTO != null)
                    //        {
                    //            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(Convert.ToString(gateway));

                    //            if (paymentGateway.IsTipAllowed(transactionPaymentsDTO))
                    //            {
                    //                flag = true;

                    //            }
                    //        }
                    //    }
                    //    if (flag)
                    //    {
                    //        dgvPayments.Cursor = Cursors.Hand;
                    //        try
                    //        {
                    //            dgvPayments[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvPayments.DefaultCellStyle.Font, FontStyle.Underline);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            log.Fatal("Ends-dgvSavedPayments_CellMouseEnter() due to exception " + ex.Message);//Modified for Adding logger feature on 08-Mar-2016
                    //        }
                    //    }
                    //}
                    DataRow dr = dgvPayments.Rows[e.RowIndex].Tag as DataRow;
                    if (dgvPayments.Columns[e.ColumnIndex].Name == "TipAmt" &&
                        dgvPayments.Rows[e.RowIndex].Cells["TipAmt"].Value.Equals("0.00") &&
                        !dgvPayments.Rows[e.RowIndex].Cells["paidPaymentAmount"].Value.ToString().Contains("-"))
                    {
                        bool flag = false;
                        if (dr["isCash"].ToString()[0].Equals('Y'))
                        {
                            flag = true;
                        }
                        else if (!(string.IsNullOrEmpty(gateway.ToString()) && dgvPayments.Rows[e.RowIndex].Cells["isCreditCard"].ToString()[0].Equals('Y')))
                        {
                            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(gateway.ToString());

                            Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO transactionPaymentsDTO = GetTransactionPaymentsDTO(dr);
                            if (paymentGateway.IsTipAllowed(transactionPaymentsDTO))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            dgvPayments.Cursor = Cursors.Hand;
                            try
                            {
                                dgvPayments[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvPayments.DefaultCellStyle.Font, FontStyle.Underline);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Ends-dgvSavedPayments_CellMouseEnter() due to exception", ex);
                                log.Fatal("Ends-dgvSavedPayments_CellMouseEnter() due to exception " + ex.Message);//Modified for Adding logger feature on 08-Mar-2016
                            }
                        }
                    }
                }
            }

            log.LogMethodExit(null);
        }//End Modification on 09-Nov-2015

        private void dgvPayments_CellMouseLeave(object sender, DataGridViewCellEventArgs e)//Begin Modification on 09-Nov-2015:Tip feature
        {
            log.LogMethodEntry(sender, e);

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodEntry(null);
                return;
            }
            if (IsTipEnabled)
            {
                object gateway = dgvPayments.Rows[e.RowIndex].Cells["Gateway"].Value;
                //Check If status not cancelled
                if (!dgvPayments.Rows[e.RowIndex].Cells["status"].Value.ToString().Equals("CANCELLED"))
                {
                    //((gateway is null && (Iscreditcard ||isCash)) || IsMercury) 
                    if (string.IsNullOrEmpty(gateway.ToString()) && 
                        (dgvPayments.Rows[e.RowIndex].Cells["isCash"].Value.ToString().Equals("Y") || 
                        dgvPayments.Rows[e.RowIndex].Cells["isCreditCard"].Value.ToString().Equals("Y")))
                    {
                        if (dgvPayments.Columns[e.ColumnIndex].Name.Equals("TipAmount") && dgvPayments.Rows[e.RowIndex].Cells["TipAmount"].Value.ToString().Equals("0.00"))
                        {
                            double Amount = Convert.ToDouble(dgvPayments.Rows[e.RowIndex].Cells["Amount"].Value.ToString());
                            if (Amount > 0)
                            {
                                dgvPayments.Cursor = Cursors.Default;
                                try
                                {
                                    dgvPayments[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvPayments.DefaultCellStyle.Font, FontStyle.Regular);
                                }
                                catch(Exception ex)
                                {
                                    log.Error("Initializing new Font Unsuccessful! ", ex);
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(null);
        }//End Modification on 09-Nov-2015

        private void dgvPayments_CellClick(object sender, DataGridViewCellEventArgs e)//Begin Modification on 09-Nov-2015:Tip feature
        {
            log.LogMethodEntry(sender, e);

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                log.LogMethodExit(null);
                return;
            }
            try
            {
                int managerId = -1;
                if (dgvPayments.Columns[e.ColumnIndex].Name.Equals("TipAmount") && !dgvPayments.Rows[e.RowIndex].Cells["status"].Value.ToString().Equals("CANCELLED"))//TipAmount:Modification for Tipamount adding on 15-oct-2015
                {
                    if (IsTipEnabled)
                    {
                        string paymentMode = "";
                        TransactionPaymentsBL trxPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, int.Parse(dgvPayments.Rows[e.RowIndex].Cells["PaymentId"].Value.ToString()));
                        TransactionPaymentsDTO trxPaymentDTO = trxPaymentsBL.TransactionPaymentsDTO;
                        if (trxPaymentDTO == null || (trxPaymentDTO != null && trxPaymentDTO.Amount < 0))
                        {
                            log.LogMethodExit(null);
                            return;
                        }
                        int compare = trxPaymentDTO.PaymentDate.CompareTo(ServerDateTime.Now.AddDays(-1));
                        if (compare < 0)
                        {
                            log.LogMethodExit(null, "Tip adjustment should be done with in 24 hours. Credit card payment gateway");
                            MessageBox.Show("Tip adjustment should be done with in 24 hours.", "Credit card payment gateway");
                            log.LogMethodExit(null);
                            return;
                        }
                        Transaction transaction = new Transaction(Utilities);
                        if (trxPaymentDTO.paymentModeDTO.IsCreditCard && !string.IsNullOrEmpty(trxPaymentDTO.paymentModeDTO.GatewayLookUp.ToString())
                             && trxPaymentDTO.paymentModeDTO.GatewayLookUp != PaymentGateways.None)
                        {

                            //frmTip formTip = null;
                            try
                            {
                                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(trxPaymentDTO.paymentModeDTO.GatewayLookUp.ToString());
                                if (paymentGateway.IsTipAllowed(trxPaymentDTO))
                                {
                                    //formTip = new frmTip(Utilities, 0.00, trxPaymentDTO.Amount, "CreditCard", false);
                                    //formTip.ShowDialog();
                                    if(authenticateManager != null && Utilities.getParafaitDefaults("MANAGER_APPROVAL_REQUIRED_FOR_TIP_ADJUSTMENT").Equals("Y"))
                                    {
                                        authenticateManager(ref managerId);
                                        if (managerId == -1)
                                        {
                                            log.LogMethodEntry("Manager is not approved.");
                                            return;
                                        }
                                    }
                                    
                                    double totalTransactionAmount = (transactionPaymentsDTOList != null) ? transactionPaymentsDTOList.Sum(x => x.Amount) : 0.0;
                                    double totalTipAmount= (transactionPaymentsDTOList != null) ? transactionPaymentsDTOList.Sum(x => x.TipAmount) : 0.0;
                                    using (frmFinalizeTransaction frmFinalizeTransaction = new frmFinalizeTransaction(Utilities, Convert.ToDecimal(totalTransactionAmount), Convert.ToDecimal(totalTipAmount), Convert.ToDecimal(trxPaymentDTO.Amount), Convert.ToDecimal(trxPaymentDTO.TipAmount), trxPaymentDTO.CreditCardNumber, MessageBox.Show))
                                    {
                                        if (frmFinalizeTransaction.ShowDialog() == DialogResult.Cancel)
                                        {
                                            log.LogMethodExit("Cancelled");
                                            return;
                                        }
                                        if (frmFinalizeTransaction.TipAmount <= 0 && !paymentGateway.IsTipAdjustmentAllowed)
                                        {
                                            log.LogMethodExit(null);
                                            return;
                                        }

                                        //if (MessageBox.Show(Utilities.MessageUtils.getMessage(953, string.Format("{0:" + Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", frmFinalizeTransaction.TipAmount)), "Tip Amount", MessageBoxButtons.YesNo) == DialogResult.No)//Begin Modification on 18-Nov-2015:Tip feature
                                        //{
                                        //    log.LogMethodExit(null);
                                        //    return;
                                        //}
                                        trxPaymentDTO.TipAmount = Convert.ToDouble(frmFinalizeTransaction.TipAmount);
                                        TransactionPaymentsDTO transactionPaymentsDTO = trxPaymentDTO;
                                        TransactionPaymentsDTO tipPaymentTransactionPaymentsDTO = paymentGateway.PayTip(transactionPaymentsDTO);
                                        if (tipPaymentTransactionPaymentsDTO != null)
                                        {
                                            tipPaymentTransactionPaymentsDTO.PosMachine = Utilities.ParafaitEnv.POSMachine;
                                            TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                                            transactionPaymentsBL.Save();
                                            transaction.InsertTrxLogs(tipPaymentTransactionPaymentsDTO.TransactionId, -1, Utilities.ParafaitEnv.LoginID, "CCSETTLEMENT", "Transaction id " + tipPaymentTransactionPaymentsDTO.TransactionId + "(Payment Id:" + tipPaymentTransactionPaymentsDTO.PaymentId + ") is settled with tipamount " + tipPaymentTransactionPaymentsDTO.TipAmount + ".",null,managerId.ToString(),Utilities.getServerTime());
                                            List<int> lstUserList = new List<int>();
                                            transaction.CreateTipPayment(trxPaymentDTO.PaymentId, lstUserList, null);
                                            dgvPayments.Rows[e.RowIndex].Cells["TipAmount"].Value = tipPaymentTransactionPaymentsDTO.TipAmount.ToString("0.00");
                                            LoadTransactionPaymentsDTOList();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Payment gateway", ex);
                                MessageBox.Show(ex.Message, "Payment gateway");
                                log.LogMethodExit(null);
                                return;
                            }
                            
                        }
                        else if (trxPaymentDTO.paymentModeDTO.IsCash ||
                                    (trxPaymentDTO.paymentModeDTO.IsCreditCard &&
                                      (string.IsNullOrEmpty(trxPaymentDTO.paymentModeDTO.GatewayLookUp.ToString())
                                       || trxPaymentDTO.paymentModeDTO.GatewayLookUp == PaymentGateways.None)
                                    )
                                )
                        {
                            if (trxPaymentDTO.paymentModeDTO.IsCash)
                            {
                                paymentMode = "Cash";
                            }
                            else
                            {
                                paymentMode = "CreditCard";
                            }
                            if (dgvPayments.Rows[e.RowIndex].Cells["TipAmount"].Value.ToString().Equals("0.00"))
                            {
                                frmTip formTip = new frmTip(Utilities, 0.00, trxPaymentDTO.Amount, paymentMode, false);
                                formTip.ShowDialog();
                                if (formTip.TipAmount == 0)
                                {
                                    log.LogMethodExit(null);
                                    return;
                                }
                                if (MessageBox.Show(Utilities.MessageUtils.getMessage(953, string.Format("{0:" + Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", formTip.TipAmount)), "Tip Amount", MessageBoxButtons.YesNo) == DialogResult.No)//Begin Modification on 18-Nov-2015:Tip feature
                                {
                                    log.LogMethodExit(null);
                                    return;
                                }//End Modification on 18-Nov-2015:Tip feature
                                if (formTip.TipAmount > 0)
                                {
                                    trxPaymentDTO.TipAmount = formTip.TipAmount;
                                    TransactionPaymentsBL trxOtherPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                                    trxOtherPaymentsBL.Save();
                                    transaction.CreateTipPayment(trxPaymentDTO.PaymentId, formTip.lstUserId, null);
                                    //End: Modification Added For Tip Payment insert on 04-Feb-2016//
                                    dgvPayments.Rows[e.RowIndex].Cells["TipAmount"].Value = trxPaymentDTO.TipAmount.ToString("0.00");
                                }
                                formTip.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                log.Error("Error occured while paying the tip", e1);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit(null);
        }//End Modification on 09-Nov-2015

        public bool IsPaymentReversed(object paymentId)
        {
            log.LogMethodEntry(paymentId);

            bool found = false;

            if (paymentId.Equals(DBNull.Value))
            {
                log.LogMethodExit(false);
                return false;
            }

            foreach (DataGridViewRow dr in dgvPayments.Rows)
            {
                if (dr.Cells["ParentPaymentId"].Value.Equals(paymentId))
                {
                    found = true;
                    break;
                }
            }

            log.LogMethodExit(found);
            return found;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                if (dgvPayments.CurrentRow != null)
                {
                    int paymentId = Convert.ToInt32(dgvPayments.CurrentRow.Cells["PaymentId"].Value);
                    if (dgvPayments.CurrentRow.Cells["IsEdit"].Value.ToString() == "Y")
                    {
                        frmEditPaymentMode frm = new frmEditPaymentMode(TrxId, paymentId, Utilities);
                        frm.ShowDialog();
                        frm.Dispose();
                        btnEdit.Enabled = false; //To prevent retrant call to same core function
                        TrxPayments_Load(null, null); // To refresh the grid
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while editing the payment mode", ex);
            }

            log.LogMethodExit(null);
        }

        //Modification on 26-Dec-2016 for changing the payment details enhanacement
        private void dgvPayments_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                btnEdit.Enabled = false;

                if (dgvPayments.Rows.Count > 0)
                {
                    if (dgvPayments.Rows[e.RowIndex].Cells["IsEdit"].Value.ToString() == "Y")
                    {
                        btnEdit.Enabled = true;
                    }
                }
            }

            catch(Exception ex)
            {
                log.Error("Error occured while enabling btnEdit", ex);
            }
            log.LogMethodExit(null);
        }
        //end Modification on 26-Dec-2016 for changing the payment details enhanacement
    }
}
