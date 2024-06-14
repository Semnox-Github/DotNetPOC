/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Confirm Order UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.4.0       11-Sep-2018      Archana              Modified for device container change
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Redemption;
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

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskConfirmOrderScreen : frmRedemptionKioskBaseForm
    {
        Utilities utilities = Common.utils;
        public frmRedemptionKioskConfirmOrderScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void FrmConfirmOrder_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                base.RenderPanelContent(_screenModel, flpPanels, 1);
                btnOrderNo.Text = "";
                utilities.setLanguage(this);
                if (RedemptionBL.IsCardRequiredForRedemption(Common.utils.ExecutionContext) && !redemptionOrder.RedemptionHasCards())
                {
                    btnClose.Enabled = false;
                    string message = "";
                    using (frmRedemptionKioskTapCard frm = new frmRedemptionKioskTapCard())
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            if (frm.CardNumber != null)
                            {
                                try
                                {
                                    message = redemptionOrder.AddCard(frm.CardNumber);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    Common.ShowMessage(ex.Message);
                                    log.LogMethodExit();
                                    this.BeginInvoke(new MethodInvoker(this.Close));
                                    return;
                                }
                            }
                            else
                            {
                                if (frm != null)
                                {
                                    frm.Close();
                                }
                                btnClose.Enabled = true;
                                log.Debug(Common.utils.MessageUtils.getMessage(1613));
                                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1613));
                                //Card is required to complete the order
                                this.BeginInvoke(new MethodInvoker(this.Close));
                                return;
                            }
                        }
                        else
                        {
                            if (frm != null)
                            {
                                frm.Close();
                            }
                            log.Debug(Common.utils.MessageUtils.getMessage(1613));
                            Common.ShowMessage(Common.utils.MessageUtils.getMessage(1613));
                            //Card is required to complete the order
                            this.BeginInvoke(new MethodInvoker(this.Close));
                            return;
                        }
                    }
                }
                ConfirmOrder();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.ToString());
                this.BeginInvoke(new MethodInvoker(this.Close));
                return;
            }
            log.LogMethodExit();
        }

        internal override bool ValidateAction(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            if (element.ActionScreenId > 0)
            {
                ScreenModel screen = new ScreenModel(element.ActionScreenId);
                if (screen.CodeObjectName == "frmStartOver")
                {
                    ClearOrderProcess();
                    Common.GoHome();
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }


        void ConfirmOrder()
        {
            log.LogMethodEntry();

            if (redemptionOrder.RedemptionTransactionNeedsManagerApproval())
            {
                log.Error(utilities.MessageUtils.getMessage(268) + utilities.MessageUtils.getMessage(441));
                throw new Exception(utilities.MessageUtils.getMessage(268) + utilities.MessageUtils.getMessage(441));
            }
            SqlCommand cmd = utilities.getCommand(utilities.createConnection().BeginTransaction());
            SqlTransaction cmdTrx = cmd.Transaction;
            try
            {
                bool managerApprovalReceived = (utilities.ParafaitEnv.ManagerId != -1);
                string orderNo = redemptionOrder.PlaceRedemptionOrder(managerApprovalReceived, "Redemption Kiosk", cmdTrx);
                btnOrderNo.Text = orderNo;
                int balanceTickets = redemptionOrder.GetBalanceTicketsRemaining();
                LoadOrPrintRemainingTickets(utilities, balanceTickets, cmdTrx);
                PrintNonRDSReceipt(cmdTrx);

                cmdTrx.Commit();
                cmdTrx.Dispose();
                PrintReceipt();
                SetKioskTimerSecondsValue(15);

            }
            catch (ValidationException ex)
            {
                log.Error(ex.Message);
                List<ValidationError> redemptionValidationErrorList = ex.ValidationErrorList;
                if (redemptionValidationErrorList != null)
                {
                    string validationErrorMessage = "";
                    foreach (ValidationError vErrorEntry in redemptionValidationErrorList)
                    {
                        validationErrorMessage = validationErrorMessage + vErrorEntry.Message + Environment.NewLine;
                    }
                    Common.ShowMessage(validationErrorMessage);
                }
                else
                {
                    Common.ShowMessage(ex.Message);
                }
                cmdTrx.Rollback();
                cmdTrx.Dispose(); ;
                this.Close();
                log.LogMethodExit("Error while validating Ordered Gift");

            }
            catch (Exception ex)
            {
                log.Error(ex);

                Common.ShowMessage(ex.Message);
                cmdTrx.Rollback();
                cmdTrx.Dispose(); ;
                ClearOrderProcess();
                Common.GoHome();
                log.LogMethodExit("Error while saving Redemption Order");
                return;
            }
        }
        void PrintReceipt()
        {
            log.LogMethodEntry();
            int redemptionReceiptTemplateId = -1;
            try
            {
                redemptionReceiptTemplateId = Convert.ToInt32(utilities.getParafaitDefaults("REDEMPTION_RECEIPT_TEMPLATE"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                redemptionReceiptTemplateId = -1;
            }
            PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(utilities.ExecutionContext, utilities);
            POSMachines posMachine = new POSMachines(utilities.ExecutionContext, Common.utils.ParafaitEnv.POSMachineId);
            List<POSPrinterDTO> posPrinterDTOList = posMachine.PopulatePrinterDetails();
            List<POSPrinterDTO> posReceiptPrinterDTOList;
            if (posPrinterDTOList != null && posPrinterDTOList.Count > 0)
            {
                posReceiptPrinterDTOList = posPrinterDTOList.Where(printerEntry => printerEntry.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter).ToList();
                if (posReceiptPrinterDTOList != null && posReceiptPrinterDTOList.Any())
                {
                    int printerReceiptTemplateIdForRedemption = -1;
                    for (int i = 0; i < posReceiptPrinterDTOList.Count; i++)
                    {
                        try
                        {
                            if (posReceiptPrinterDTOList[i].ReceiptPrintTemplateHeaderDTO != null)
                            {
                                printerReceiptTemplateIdForRedemption = posReceiptPrinterDTOList[i].ReceiptPrintTemplateHeaderDTO.TemplateId;
                            } 
                            printerReceiptTemplateIdForRedemption = (printerReceiptTemplateIdForRedemption > -1 ? printerReceiptTemplateIdForRedemption : redemptionReceiptTemplateId);
                            printRedemptionReceipt.PrintRedemption(redemptionOrder, printerReceiptTemplateIdForRedemption, posReceiptPrinterDTOList[i].PrinterDTO);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            Common.ShowMessage(utilities.MessageUtils.getMessage(1637, posReceiptPrinterDTOList[i].PrinterDTO.PrinterName + ": " + ex.Message));
                        }
                    }
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2413, Environment.MachineName));
                }
            }
        }

        void LoadOrPrintRemainingTickets(Utilities utilities, int balanceTickets, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(utilities, balanceTickets, sqlTrx);
            string message = "";
            if (balanceTickets > 0)
            {
                bool managerApprovalReceived = (utilities.ParafaitEnv.ManagerId != -1);
                redemptionOrder.LoadTicketLimitCheck(managerApprovalReceived, balanceTickets);
                if (redemptionOrder.RedemptionHasCards())
                {
                    if ((utilities.getParafaitDefaults("AUTO_LOAD_BALANCE_TICKETS_TO_CARD").Equals("Y")
                        ||
                        Common.ShowDialog(Common.utils.MessageUtils.getMessage(1614)) == System.Windows.Forms.DialogResult.Yes))
                    {
                        TaskProcs tp = new TaskProcs(utilities);
                        if (!tp.loadTickets(redemptionOrder.GetRedemptionPrimaryCard(sqlTrx), balanceTickets, "Redemption Kiosk balance tickets", redemptionOrder.RedemptionDTO.RedemptionId, ref message, sqlTrx))
                        {
                            log.Error(message);
                            throw new Exception(utilities.MessageUtils.getMessage(1500));
                        }
                    }
                    else
                    {
                        PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(utilities.ExecutionContext, utilities);
                        printRedemptionReceipt.CreateManualTicketReceipt(balanceTickets, redemptionOrder, sqlTrx);
                    }
                }
                else
                {
                    PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(utilities.ExecutionContext, utilities);
                    printRedemptionReceipt.CreateManualTicketReceipt(balanceTickets, redemptionOrder, sqlTrx);
                }
            }
            log.LogMethodExit();
        }

        void PrintNonRDSReceipt(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            //if (Common.utils.getParafaitDefaults("ENABLE_RDS_SYSTEM").Equals("N"))
            {

                POSMachines posMachine = new POSMachines(utilities.ExecutionContext, Common.utils.ParafaitEnv.POSMachineId);
                List<POSPrinterDTO> posPrinterDTOList = posMachine.PopulatePrinterDetails();
                List<POSPrinterDTO> posRDSPrinterDTOList;
                if (posPrinterDTOList != null && posPrinterDTOList.Count > 0)
                {
                    posRDSPrinterDTOList = posPrinterDTOList.Where(printerEntry => printerEntry.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RDSPrinter).ToList();
                    int rdsReceiptTemplateId = -1;
                    if (posRDSPrinterDTOList != null && posRDSPrinterDTOList.Any())
                    {
                        try
                        {
                            DeviceContainer.PrinterStatus(Common.utils.ExecutionContext);
                            for (int i = 0; i < posRDSPrinterDTOList.Count; i++)
                            {
                                log.Info("Printer name: " + posRDSPrinterDTOList[i].PrinterDTO.PrinterName);
                                rdsReceiptTemplateId = -1;
                                if (posRDSPrinterDTOList[i].ReceiptPrintTemplateHeaderDTO != null)
                                {
                                    rdsReceiptTemplateId = posRDSPrinterDTOList[i].ReceiptPrintTemplateHeaderDTO.TemplateId;
                                }
                                PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(utilities.ExecutionContext, utilities);
                                printRedemptionReceipt.PrintRedemption(redemptionOrder, rdsReceiptTemplateId, posRDSPrinterDTOList[i].PrinterDTO, null, false, sqlTrx);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            throw new Exception(utilities.MessageUtils.getMessage(1637, ex.Message));
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        internal override void InactivityTimer_Tick(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining <= 60)
            {
                tickSecondsRemaining = tickSecondsRemaining - 1;
            }
            SetKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 0)
            {
                ClearOrderProcess();
                log.Debug("Call GoHome");
                Common.GoHome();
            }
            // log.LogMethodExit();
        }
    }
}
