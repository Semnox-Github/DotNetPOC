/********************************************************************************************
 * Project Name - KOTPrintProducts
 * Description  - KOT Print Products
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
 *2.70         26-Mar-2019     Guru S A       Booking phase 2 enhancement changes  
 *2.80         18-Dec-2019     Jinto Thomas   Added parameter execution context for userbl declaration with userid 
********************************************************************************************/
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class KOTPrintProducts : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Transaction Transaction;
        public KOTPrintProducts(Transaction pTrx)
        {
            InitializeComponent();
            Transaction = pTrx;
            txtMessage.Text = "";
        }

        private void KOTPrintProducts_Load(object sender, EventArgs e)
        {
            load();
        }

        void load()
        {
            log.LogMethodEntry();
            SqlCommand cmd = POSStatic.Utilities.getCommand();
            cmd.CommandText = "select product_name Product, Quantity, remarks, LineId, KOTPrintCount KOT_Print_# " +
                            " from trx_lines l, products p " +
                            "where p.product_id = l.product_Id " +
                            "and TrxId = -1";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (Transaction.TransactionLine tl in Transaction.TrxLines)
            {
                if (tl.LineValid && tl.DBLineId > 0 && IsEligibleForKotPrint(tl))
                {
                    dt.Rows.Add(tl.ProductName, tl.quantity, tl.Remarks, tl.DBLineId, tl.KOTPrintCount);
                }
            }
            dgvOrderLines.DataSource = dt;

            POSStatic.Utilities.setupDataGridProperties(ref dgvOrderLines);
            dgvOrderLines.Columns["LineId"].Visible = false;
            dgvOrderLines.BackgroundColor = this.BackColor;
            dgvOrderLines.Columns["Quantity"].DefaultCellStyle =
            dgvOrderLines.Columns["KOT_Print_#"].DefaultCellStyle = POSStatic.Utilities.gridViewNumericCellStyle();
            for (int i = 0; i < dgvOrderLines.Rows.Count; i++)
            {
                if (dgvOrderLines["KOT_Print_#", i].Value.ToString() == "")
                    dgvOrderLines["PrintKOT", i].Value = "Y";
                else
                    dgvOrderLines["PrintKOT", i].Value = "N";
            }

            dgvOrderLines.Columns["Product"].ReadOnly = true;
            dgvOrderLines.Columns["Quantity"].ReadOnly = true;
            dgvOrderLines.Columns["remarks"].ReadOnly = true;
            dgvOrderLines.Columns["PrintKOT"].ReadOnly = false;
            dgvOrderLines.Columns["KOT_Print_#"].ReadOnly = true; // 14-mar-2016
            log.LogMethodExit();
        }

        private bool IsEligibleForKotPrint(Transaction.TransactionLine line)
        {
            log.LogMethodEntry(line);
            bool result = POSPrinterListBL.EligibleForKOTPrint(line.ProductID, POSStatic.POSPrintersDTOList);
            if (result == false && line.ParentLine != null)
            {
                result = IsEligibleForKotPrint(line.ParentLine);
            }
            log.LogMethodExit(result);
            return result;
        }



        private void btnPrintKOT_Click(object sender, EventArgs e)
        {
            bool reprintApproved = false;
            log.LogMethodEntry();
            string message = "";
            int managerId = 0;
            Users approveUser = null;
            bool hasSomethingToPrint = false;
            for (int j = 0; j < Transaction.TrxLines.Count; j++)
            {
                if (Transaction.TrxLines[j].PrintKOT 
                    && (Transaction.TrxLines[j].KOTPrintCount == DBNull.Value
                       || (Transaction.TrxLines[j].KOTPrintCount != DBNull.Value && Convert.ToInt32(Transaction.TrxLines[j].KOTPrintCount) == 0)))
                {
                    Transaction.TrxLines[j].PrintKOT = false;
                }

                for (int i = 0; i < dgvOrderLines.Rows.Count; i++)
                {
                    if (Transaction.TrxLines[j].DBLineId == Convert.ToInt32(dgvOrderLines.Rows[i].Cells["lineId"].Value))
                    {
                        if (dgvOrderLines.Rows[i].Cells["PrintKOT"].Value.ToString() == "Y"
                            && Transaction.TrxLines[j].DBLineId == Convert.ToInt32(dgvOrderLines.Rows[i].Cells["lineId"].Value))
                        {
                            if (!hasSomethingToPrint)
                                hasSomethingToPrint = true;
                            Transaction.TrxLines[j].PrintKOT = true;
                            Transaction.TrxLines[j].ReceiptPrinted = false;
                            Transaction.TrxLines[j].KDSSent = false;
                            try
                            {
                                if (Transaction.TrxLines[j].KOTPrintCount != DBNull.Value)
                                {
                                    if (!reprintApproved)
                                    {
                                        if (!Authenticate.Manager(ref managerId))
                                        {
                                            POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(268), "Reprint KOT");
                                            log.Info("Ends-ctxOrderContextTableMenu_ItemClicked() as Manager Approval Required for reprinting KOT");//Added for logger function on 08-Mar-2016
                                            return;
                                        }
                                        reprintApproved = true;
                                        approveUser = new Users(POSStatic.Utilities.ExecutionContext, managerId);
                                    }
                                    Transaction.InsertTrxLogs(Transaction.Trx_id, Transaction.TrxLines[j].DBLineId, Transaction.Utilities.ParafaitEnv.LoginID, "REPRINT", "KOT Re-Print through View Orders", approverId: approveUser.UserDTO.LoginId, approvalTime: Transaction.Utilities.getServerTime());
                                }
                            }
                            catch
                            {
                                log.Error("Error inserting Trx user Log within KOT Print Products");
                            }
                            break;
                        }
                    }
                }

            }
            if (hasSomethingToPrint == false)
            {
                POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(145), "Reprint KOT");
                log.Info("Ends-ctxOrderContextTableMenu_ItemClicked() as Nothing is selected to print");//Added for logger function on 08-Mar-2016
                return;
            }

            PrintTransaction pt = new PrintTransaction(POSStatic.POSPrintersDTOList);

            if (!pt.PrintKOT(Transaction, ref message))
            {
                txtMessage.Text = message;
            }
            else
            {
                txtMessage.Text = POSStatic.MessageUtils.getMessage(170);
                load();
            }
            log.LogMethodExit();
        }
    }
}
