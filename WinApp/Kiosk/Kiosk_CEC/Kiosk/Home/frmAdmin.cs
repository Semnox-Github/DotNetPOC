/********************************************************************************************
 * Project Name - Parafait_Kiosk - frmAdmin
 * Description  - frmAdmin.cs 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.100.0     10-Aug-2020      Guru S A           Kiosk activity log changes
 *2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements
 *2.130.0     30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.140.0     22-Oct-2021      Sathyavathi        CEC changes - Kiosk Activity Print Summary Report
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Parafait_Kiosk
{
    public partial class frmAdmin : BaseFormKiosk
    {
        private bool managerCardFlag = false;
        ExecutionContext executionContext = null;
        public frmAdmin(ExecutionContext executionContext,  bool isManagerCard = false)
        {
            log.LogMethodEntry(isManagerCard);
            this.executionContext = executionContext;
            InitializeComponent();
            managerCardFlag = isManagerCard;
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);
            this.BackgroundImage = KioskStatic.CurrentTheme.AdminBackgroundImage;
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cancel pressed");
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        { 
            log.LogMethodEntry();
            StopKioskTimer();
            ResetKioskTimer();
            KioskStatic.logToFile("SetUp pressed");
            SetUp sf = new SetUp();
            sf.ShowDialog(); 
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Exit pressed");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnPrintSummary_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            { 
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Print Summary pressed");

                DataTable dt = KioskStatic.Utilities.executeDataTable(@"select 
                                                                    isnull(sum(Amount * CashRatio), 0) Cash, 
                                                                    isnull(sum(Amount * CreditCardRatio), 0) CreditCard, 
                                                                    isnull(sum(Amount * GameCardRatio), 0) GameCard, 
																	--isnull(aborted.aborted, 0) Aborted,
                                                                    --isnull(sum(Amount), 0) + isnull(aborted.aborted, 0) Total,
                                                                    isnull(sum(Amount), 0) Total,
                                                                    count(distinct trxId) [TotalOps],
                                                                    count(distinct new_card_id) [NewCards]
                                                                  from TransactionView v --, 
                                                                   -- (select sum(value) aborted
                                                                   --     from KioskActivityLog 
                                                                   --     where Activity in ( 'ABORT' ,'ABORT_CC', 'ABORT_GAMECARD') 
                                                                   --     and TimeStamp >= case when datepart(hh, getdate()) < 6
                                                                   --                     then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                   --                     else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                   --     and KioskName = @pos 
                                                                   --  ) aborted
                                                                  where trxdate >= case when datepart(hh, getdate()) < 6
                                                                                        then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                                        else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                  and pos_machine = @pos  
--and not exists (select 1 from bookings b where b.trxId = v.TrxId)
																  --group by aborted.aborted",
                                                                      new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                DataTable dtAborted = KioskStatic.Utilities.executeDataTable(@"select Activity, sum(value) aborted
                                                                                 from KioskActivityLog 
                                                                                where Activity in ( 'ABORT' ,'ABORT_CC', 'ABORT_GAMECARD') 
                                                                                  and TimeStamp >= case when datepart(hh, getdate()) < 6
                                                                                        then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                                        else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                                  and KioskName = @pos
                                                                                group by Activity",
                                                                      new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                DataTable dtMoney = KioskStatic.Utilities.executeDataTable(@"select NoteCoinFlag, Value, sum(value) Total, count(value) Quantity 
                                                                         from kioskactivitylog
                                                                         where NoteCoinFlag is not null
                                                                           and TimeStamp >= case when datepart(hh, getdate()) < 6
                                                                                        then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                                        else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                           and KioskName = @pos
                                                                           group by NoteCoinFlag, value
                                                                           order by NoteCoinFlag, value",
                                                                      new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                DataTable dtRefund = KioskStatic.Utilities.executeDataTable(@" select ISNULL(sum(value),0) Refunded
                                                                                from KioskActivityLog 
                                                                                where Activity = 'REFUND'
                                                                                and TimeStamp >= case when datepart(hh, getdate()) < 6
                                                                                                then DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate() - 1), 0)) 
                                                                                                else DATEADD(HH, 6, DATEADD(D, datediff(D, 0, getdate()), 0)) end
                                                                                and KioskName = @pos ",
                                                                     new SqlParameter("@pos", KioskStatic.Utilities.ParafaitEnv.POSMachine));

                string formattedReceipt = "*---KIOSK Activity Summary---*" + Environment.NewLine;
                formattedReceipt += KioskStatic.Utilities.ParafaitEnv.SiteName + Environment.NewLine + Environment.NewLine;
                formattedReceipt += "Date: " + DateTime.Now.ToString(KioskStatic.Utilities.ParafaitEnv.DATETIME_FORMAT) + Environment.NewLine;
                formattedReceipt += "Kiosk: " + KioskStatic.Utilities.ParafaitEnv.POSMachine + Environment.NewLine + Environment.NewLine;

                decimal refundedAmount = 0;
                if (dtRefund.Rows.Count > 0)
                {
                    refundedAmount = Convert.ToDecimal(dtRefund.Rows[0]["Refunded"]);
                }

                decimal abortedCash = 0;
                decimal abortedCreditCard = 0;
                decimal abortedGameCard = 0;
                if (dtAborted.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAborted.Rows.Count; i++)
                    {
                        if (dtAborted.Rows[i]["Activity"].ToString() == KioskStatic.ACTIVITY_TYPE_ABORT.ToString())
                        {
                            abortedCash = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskStatic.ACTIVITY_TYPE_ABORT_CC.ToString())
                        {
                            abortedCreditCard = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                        else if (dtAborted.Rows[i]["Activity"].ToString() == KioskStatic.ACTIVITY_TYPE_ABORT_GAMECARD.ToString())
                        {
                            abortedGameCard = Convert.ToDecimal(dtAborted.Rows[0]["Aborted"]);
                        }
                    }
                }
                decimal cashAmount = 0;
                decimal cardAmount = 0;
                decimal gameCardAmount = 0;
                decimal totalTrxAmount = 0;
                decimal totalOps = 0;
                decimal newCards = 0;
                if (dt.Rows.Count > 0)
                {
                    cashAmount = Convert.ToDecimal(dt.Rows[0]["Cash"]);
                    cardAmount = Convert.ToDecimal(dt.Rows[0]["CreditCard"]);
                    gameCardAmount = Convert.ToDecimal(dt.Rows[0]["GameCard"]);
                    totalTrxAmount = Convert.ToDecimal(dt.Rows[0]["Total"]);
                    totalOps = Convert.ToDecimal(dt.Rows[0]["TotalOps"]);
                    newCards = Convert.ToDecimal(dt.Rows[0]["NewCards"]); 
                }
                formattedReceipt += "Cash: " + cashAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                formattedReceipt += "Credit Card: " + cardAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                if (gameCardAmount > 0)
                {
                    formattedReceipt += "Game Card: " + gameCardAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                }

                formattedReceipt += "Aborted Cash: " + abortedCash.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                formattedReceipt += "Aborted Credit: " + abortedCreditCard.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                if (abortedGameCard > 0)
                {
                    formattedReceipt += "Aborted Game Card: " + abortedGameCard.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                }

                formattedReceipt += "Refunded Credit: " + refundedAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;

                formattedReceipt += "Total Trx: " + totalTrxAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine + Environment.NewLine;
                formattedReceipt += "#Ops: " + totalOps.ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT) + Environment.NewLine;
                formattedReceipt += "#New Cards: " + newCards.ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT) + Environment.NewLine + Environment.NewLine;


                decimal insertedTotal = 0;
                foreach (DataRow dr in dtMoney.Rows)
                {
                    if (dr["NoteCoinFlag"].ToString().Equals("T"))
                    {
                        formattedReceipt += ("  " + "Token").PadRight(12) + " #" + dr["Quantity"].ToString() + Environment.NewLine;
                    }
                    else
                    {
                        insertedTotal += Convert.ToDecimal(dr["Total"]);
                        formattedReceipt += (KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL + " " + Convert.ToDouble(dr["Value"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + " " + (dr["NoteCoinFlag"].ToString().Equals("N") ? "Bill" : "Coin")).PadRight(12) + " #" + dr["Quantity"].ToString() + ", " + Convert.ToDecimal(dr["Total"]).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                    }
                }
                formattedReceipt += "Total Inserted: " + insertedTotal.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;

                formattedReceipt += Environment.NewLine;
                formattedReceipt += "-".PadRight(10, '-').PadLeft(20, ' ');
                log.LogVariableState("formattedReceipt", formattedReceipt);
                PrintDocument printDocument = new PrintDocument();
                PrinterDTO printerDTO = null;
                if (KioskStatic.POSMachineDTO == null || KioskStatic.POSMachineDTO.PosPrinterDtoList == null || KioskStatic.POSMachineDTO.PosPrinterDtoList.Any() == false)
                {
                    POSMachines posMachine = new POSMachines(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                    KioskStatic.POSMachineDTO.PosPrinterDtoList = posMachine.PopulatePrinterDetails();
                }
                List<POSPrinterDTO> POSPrintersDTOList = new List<POSPrinterDTO>(KioskStatic.POSMachineDTO.PosPrinterDtoList);
                if (POSPrintersDTOList != null && POSPrintersDTOList.Any() &&
                    POSPrintersDTOList.Exists(pp => pp.PrinterDTO != null
                                      && pp.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter))
                {
                    printerDTO = POSPrintersDTOList.Find(pp => pp.PrinterDTO != null
                                      && pp.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter).PrinterDTO;
                }
                else
                {
                    printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1, -1);
                }
                log.LogVariableState("printerDTO", printerDTO);
                printDocument.PrinterSettings.PrinterName = String.IsNullOrWhiteSpace(printerDTO.PrinterLocation) ? printerDTO.PrinterName : printerDTO.PrinterLocation;
                KioskStatic.logToFile("Summary Print assigned to Printer: " + printDocument.PrinterSettings.PrinterName);
                log.LogVariableState("Printer: ", printDocument.PrinterSettings.PrinterName);
                PrinterBL printerBL = new PrinterBL(KioskStatic.Utilities.ExecutionContext, printerDTO);
                PrinterBuildBL printerBuildBL = new PrinterBuildBL(KioskStatic.Utilities.ExecutionContext);
                if (printerBuildBL.SetUpPrinting(printDocument, false, "", printerDTO))
                {
                    printDocument.PrintPage += (s, args) =>
                    {
                        args.Graphics.DrawString(formattedReceipt, new System.Drawing.Font("Courier New", 9, FontStyle.Bold), System.Drawing.Brushes.Black, 12, 20);
                    };
                    using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                    {
                        //code to send print document to the printer
                        printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error printing Kiosk Summary: " + ex.Message);
                using (frmOKMsg frm = new frmOKMsg(ex.Message, true))
                {
                    frm.ShowDialog();
                }
            }
            finally
            { 
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
         
        private void frmAdmin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                flowLayoutPanel1.SuspendLayout();
                this.SuspendLayout();
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_LOAD_BONUS_IN_ADMIN_SCREEN", false))
                {
                    btnLoadBonus.Enabled = true;
                }
                else
                {
                    btnLoadBonus.Enabled = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_TRX_VIEW_IN_ADMIN_SCREEN", false))
                {
                    btnTrxView.Enabled = true;
                }
                else
                {
                    btnTrxView.Enabled = false;
                }
                flowLayoutPanel1.ResumeLayout(true);
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error loading Admin form: " + ex.Message);
            }
            KioskStatic.logToFile("Admin Screen opened"); 
            this.FormClosed += frmAdmin_FormClosed; 
            displaybtnPrev(false);
            displaybtnCancel(true);
            log.LogMethodExit();
        }

        void frmAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(); 
            KioskStatic.logToFile("frmAdmin_FormClosed");
            log.LogMethodExit();
        }
         
        private void btnLoadBonus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            { 
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Load Bonus pressed");
                frmTapCard ftc = new frmTapCard();
                ftc.ShowDialog();
                if (ftc.Card == null)
                {
                    ftc.Dispose();
                    log.LogMethodExit();
                    return;
                }
                ftc.Dispose();

                if (ftc.Card.technician_card == 'Y')
                {
                    frmLoadBonus flb = new frmLoadBonus(ftc.Card);
                    flb.ShowDialog();
                }
                else
                {
                    frmOKMsg fok = new frmOKMsg("Please tap your staff card");
                    fok.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
            }
            finally
            { 
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining < 10)
            {
                KioskStatic.logToFile("Time-out exit");
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void btnKioskActivity_Click(object sender, EventArgs e)//25-06-2015:Starts
        {
            log.LogMethodEntry(); 
            ResetKioskTimer();
            KioskStatic.logToFile("KioskActivity pressed"); 
            KioskActivityDetails kioskactivity = new KioskActivityDetails(executionContext, managerCardFlag);
            kioskactivity.Show();
            log.LogMethodExit();
        }

        private void btnRebootComputer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnRebootComputer_Click");
            System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 00");
            log.LogMethodExit();
        }

        private void btnTrxView_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Transaction View pressed");
                using (frmKioskTransactionView flb = new frmKioskTransactionView())
                {
                    flb.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnCancelTextForeColor;//#AdminScreenBtnCancelForeColor
                this.btnExit.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnExitTextForeColor;//#AdminScreenBtnExitForeColor
                this.btnSetup.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnSetupTextForeColor;//#AdminScreenBtnSetupForeColor
                this.btnLoadBonus.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnLoadBonusTextForeColor;//AdminScreenBtnLoadBonusForeColor
                this.btnPrintSummary.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnPrintSummaryTextForeColor;//AdminScreenBtnPrintSummaryForeColor
                this.btnKioskActivity.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnKioskActivityTextForeColor;//AdminScreenBtnKioskActivityForeColor
                this.btnRebootComputer.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnRebootComputerTextForeColor;//AdminScreenBtnRebootComputerForeColor
                this.btnTrxView.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnTrxViewTextForeColor;//AdminScreenBtnRebootComputerForeColor
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
