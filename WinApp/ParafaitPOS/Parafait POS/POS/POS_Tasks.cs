/********************************************************************************************************************************************
 * Project Name - POS_Tasks
 * Description  - UI for various Task Types
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************************************************************
 *2.120.0      02-Apr-2021     Prashanth      moving to new POS UI 
********************************************************************/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.TransactionUI;
namespace Parafait_POS
{
    public partial class POS
    {
        private void functionButtionMouseUp(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            if (b.Tag == null) // task button tag hold the name of the task - set at design time
                return;

            string task = b.Tag.ToString();

            Image bg = null;
            switch (task)
            {
                case TaskProcs.TRANSFERCARD: bg = Properties.Resources.TransferCard; break;
                case TaskProcs.EXCHANGETOKENFORCREDIT: bg = Properties.Resources.ExchangeTokenForCredit; break;
                case TaskProcs.EXCHANGECREDITFORTOKEN: bg = Properties.Resources.ExchangeCreditForToken; break;
                case TaskProcs.LOADTICKETS: bg = Properties.Resources.LoadTickets; break;
                case TaskProcs.CONSOLIDATE: bg = Properties.Resources.Consolidate; break;
                case TaskProcs.LOADMULTIPLE: bg = Properties.Resources.LoadMultiple; break;
                case TaskProcs.REALETICKET: bg = Properties.Resources.RealETicket; break;
                case TaskProcs.REFUNDCARD: bg = Properties.Resources.RefundCard_Normal; break;
                case TaskProcs.REFUNDCARD + "TASK": bg = Properties.Resources.refund_card_task_normal; break;
                case TaskProcs.LOADBONUS: bg = Properties.Resources.LoadBonus; break;
                case TaskProcs.DISCOUNT: bg = Properties.Resources.ApplyDiscount; break;
                case TaskProcs.REDEEMLOYALTY: bg = Properties.Resources.RedeemLoyalty; break;
                case TaskProcs.SPECIALPRICING: bg = Properties.Resources.SpecialPricing; break;
                case TaskProcs.REDEEMTICKETSFORBONUS: bg = Properties.Resources.RedeemTickets; break;
                case TaskProcs.BALANCETRANSFER: bg = Properties.Resources.transfer_balance_normal; break;
                case TaskProcs.SALESRETURNEXCHANGE: bg = Properties.Resources.return_normal; break; //Added button for return/exchange process 10-Jun-2016
                case TaskProcs.REDEEMBONUSFORTICKET: bg = Properties.Resources.DollarToTicket_normal; break; //Added button for redeem bonus for tickets process 16-Jun-2017
                case TaskProcs.EXCHANGECREDITFORTIME: bg = Properties.Resources.Points_to_Time_normal; break;
                case TaskProcs.EXCHANGETIMEFORCREDIT: bg = Properties.Resources.Time_to_Points_Normal; break;
                case TaskProcs.PAUSETIMEENTITLEMENT: bg = Properties.Resources.Pause_Time_normal; break;
                case TaskProcs.HOLDENTITLEMENTS: bg = Properties.Resources.HoldEntitlement; break;
                case TaskProcs.REDEEMVIRTUALPOINTS: bg = Properties.Resources.Virtual_loyalty_points; break;

                default: break;
            }
            b.BackgroundImage = bg;
        }

        private void functionButtonMouseDown(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            if (b.Tag == null) // task button tag hold the name of the task - set at design time
                return;

            string task = b.Tag.ToString();

            Image bg = null;
            switch (task)
            {
                case TaskProcs.TRANSFERCARD: bg = Properties.Resources.TransferCardPressed; break;
                case TaskProcs.EXCHANGETOKENFORCREDIT: bg = Properties.Resources.ExchangeTokenForCreditPressed; break;
                case TaskProcs.EXCHANGECREDITFORTOKEN: bg = Properties.Resources.ExchangeCreditForTokenPressed; break;
                case TaskProcs.LOADTICKETS: bg = Properties.Resources.LoadTicketsPressed; break;
                case TaskProcs.CONSOLIDATE: bg = Properties.Resources.ConsolidatePressed; break;
                case TaskProcs.LOADMULTIPLE: bg = Properties.Resources.LoadMultiplePressed; break;
                case TaskProcs.REALETICKET: bg = Properties.Resources.RealETicketPressed; break;
                case TaskProcs.REFUNDCARD: bg = Properties.Resources.RefundCard_Pressed; break;
                case TaskProcs.REFUNDCARD + "TASK": bg = Properties.Resources.refund_card_task_pressed; break;
                case TaskProcs.LOADBONUS: bg = Properties.Resources.LoadBonusPressed; break;
                case TaskProcs.DISCOUNT: bg = Properties.Resources.ApplyDiscountPressed; break;
                case TaskProcs.REDEEMLOYALTY: bg = Properties.Resources.RedeemLoyaltyPressed; break;
                case TaskProcs.SPECIALPRICING: bg = Properties.Resources.SpecialPricingPressed; break;
                case TaskProcs.REDEEMTICKETSFORBONUS: bg = Properties.Resources.RedeemTicketsPressed; break;
                case TaskProcs.BALANCETRANSFER: bg = Properties.Resources.transfer_balance_pressed; break;
                case TaskProcs.SALESRETURNEXCHANGE: bg = Properties.Resources.return_pressed; break; //Added button for return/exchange process 10-Jun-2016
                case TaskProcs.REDEEMBONUSFORTICKET: bg = Properties.Resources.DollarToTicket_Pressed; break; //Added button for redeem bonus for tickets process 16-Jun-2017
                case TaskProcs.EXCHANGECREDITFORTIME: bg = Properties.Resources.Points_to_Time_pressed; break;
                case TaskProcs.EXCHANGETIMEFORCREDIT: bg = Properties.Resources.Time_to_Points_Pressed; break;
                case TaskProcs.PAUSETIMEENTITLEMENT: bg = Properties.Resources.Pause_Time_pressed; break;
                case TaskProcs.HOLDENTITLEMENTS: bg = Properties.Resources.HoldEntitlementPressed; break;
                case TaskProcs.REDEEMVIRTUALPOINTS: bg = Properties.Resources.Virtual_loyalty_points; break;
                default: break;
            }
            b.BackgroundImage = bg;

            b.FlatAppearance.BorderColor = POSBackColor;
        }
        private void CallNewTask(object Task, EventArgs e)
        {
            log.LogMethodEntry(Task, e);
            if (!checkLoginClockIn())
                return;
            if (!CheckCashdrawerAssignment())
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer")); // New Message 13 y Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer
                return;
            }
            Button TaskId = (Button)Task;
            lastTrxActivityTime = DateTime.Now;
            if (TaskId.Tag == null) // task button tag hold the name of the task - set at design time
                return;
            string task = TaskId.Tag.ToString();
            Application.DoEvents();
            log.Debug("Starts-" + task);
            if (!string.IsNullOrWhiteSpace(task))
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                    ParafaitPOS.App.SerialPortNumber = Properties.Settings.Default.PoleDisplayCOMPort;
                    ParafaitPOS.App.PoleDisplayPort = PoleDisplay.GetPoleDisplayPort();
                    ParafaitPOS.App.EnsureApplicationResources();
                    timerClock.Stop();
                    displayMessageLine("", MESSAGE);
                    Semnox.Parafait.CommonUI.PoleDisplay.writeLines(TaskId.Text, "");
                    switch (task)
                    {
                        case "REALETICKET":
                            CallRealETicket();
                            break;
                        case "EXCHANGETOKENFORCREDIT":
                            CallExchangeToken(TaskType.EXCHANGETOKENFORCREDIT);
                            break;
                        case "EXCHANGECREDITFORTOKEN":
                            CallExchangeToken(TaskType.EXCHANGECREDITFORTOKEN);
                            break;
                        case "BALANCETRANSFER":
                            CallBalanceTransfer();
                            break;
                        case "EXCHANGETIMEFORCREDIT":
                            CallExchangeCredits(TaskType.EXCHANGETIMEFORCREDIT);
                            break;
                        case "EXCHANGECREDITFORTIME":
                            CallExchangeCredits(TaskType.EXCHANGECREDITFORTIME);
                            break;
                        case "REDEEMBONUSFORTICKET":
                            CallRedeemEntitilements(TaskType.REDEEMBONUSFORTICKET);
                            break;
                        case "REDEEMTICKETSFORBONUS":
                            CallRedeemEntitilements(TaskType.REDEEMTICKETSFORBONUS);
                            break;
                        case "LOADBONUS":
                            CallLoadBonus();
                            break;
                        case "PAUSETIMEENTITLEMENT":
                            CallPauseTime();
                            break;
                        case "REDEEMLOYALTY":
                            CallRedeemLoyalty();
                            break;
                        case "LOADTICKETS":
                            CallLoadTickets();
                            break;
                    }
                }
                catch (UnauthorizedException ex)
                {
                    this.Cursor = Cursors.Default;
                    (Application.OpenForms["POS"] as Parafait_POS.POS).logOutUser();
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(ex.Message, "ERROR");
                }
                finally
                {
                    lastTrxActivityTime = DateTime.Now;
                    timerClock.Start();
                }
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
            lastTrxActivityTime = DateTime.Now;
            timerClock.Start();
            log.Debug("Ends-" + task);
            if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                logOutUser();
            log.LogMethodExit();
        }

        private void CallLoadTickets()
        {
            log.Debug("Starts-CallLoadTickets()");
            TaskLoadTicketView taskLoadTicketView = null;
            TaskLoadTicketVM taskLoadTicketVM = null;
            try
            {
                taskLoadTicketVM = new TaskLoadTicketVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader, Common.Devices.PrimaryBarcodeScanner);
                taskLoadTicketView = new TaskLoadTicketView();
                taskLoadTicketView.DataContext = taskLoadTicketVM;
                ElementHost.EnableModelessKeyboardInterop(taskLoadTicketView);
                taskLoadTicketView.ShowDialog();
                if (!string.IsNullOrWhiteSpace(taskLoadTicketVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskLoadTicketVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskLoadTicketVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskLoadTicketVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskLoadTicketVM.PerformClose(taskLoadTicketView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-CallLoadTickets");
        }

        private void CallRedeemLoyalty()
        {
            log.Debug("Starts-CallRedeemLoyalty()");
            TaskRedeemLoyaltyView taskRedeemLoyaltyView = null;
            TaskRedeemLoyaltyVM taskRedeemLoyaltyVM = null;
            try
            {
                taskRedeemLoyaltyVM = new TaskRedeemLoyaltyVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                taskRedeemLoyaltyView = new TaskRedeemLoyaltyView();
                taskRedeemLoyaltyView.DataContext = taskRedeemLoyaltyVM;
                ElementHost.EnableModelessKeyboardInterop(taskRedeemLoyaltyView);
                taskRedeemLoyaltyView.ShowDialog();
                if (!string.IsNullOrWhiteSpace(taskRedeemLoyaltyVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskRedeemLoyaltyVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskRedeemLoyaltyVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskRedeemLoyaltyVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskRedeemLoyaltyVM.PerformClose(taskRedeemLoyaltyView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-CallRedeemLoyalty()");
        }

        private void CallRealETicket()
        {
            log.Debug("Starts-callChangeTicketMode()");
            TaskChangeTicketModeView taskChangeTicketModeView = null;
            TaskChangeTicketModeVM taskChangeTicketModeVM = null;
            try
            {
                taskChangeTicketModeVM = new TaskChangeTicketModeVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                taskChangeTicketModeView = new Semnox.Parafait.TransactionUI.TaskChangeTicketModeView();
                taskChangeTicketModeView.DataContext = taskChangeTicketModeVM;
                ElementHost.EnableModelessKeyboardInterop(taskChangeTicketModeView);
                WindowInteropHelper helper = new WindowInteropHelper(taskChangeTicketModeView);
                helper.Owner = this.Handle;
                bool? dialogResult = taskChangeTicketModeView.ShowDialog();
                if (dialogResult == false)
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
                if (!string.IsNullOrWhiteSpace(taskChangeTicketModeVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskChangeTicketModeVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskChangeTicketModeVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskChangeTicketModeVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskChangeTicketModeVM.PerformClose(taskChangeTicketModeView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-callChangeTicketMode()");
        }
        private void CallExchangeToken(TaskType taskType)
        {
            log.Debug("Starts-callExchangeToken()");
            TaskExchangeCreditTokenView taskExchangeCreditTokenView = null;
            TaskExchangeCreditTokenVM taskExchangeCreditTokenVM = null;
            try
            {
                taskExchangeCreditTokenVM = new TaskExchangeCreditTokenVM(ParafaitEnv.ExecutionContext, taskType, Common.Devices.PrimaryCardReader);
                taskExchangeCreditTokenView = new TaskExchangeCreditTokenView();
                taskExchangeCreditTokenView.DataContext = taskExchangeCreditTokenVM;
                ElementHost.EnableModelessKeyboardInterop(taskExchangeCreditTokenView);
                WindowInteropHelper helper = new WindowInteropHelper(taskExchangeCreditTokenView);
                helper.Owner = this.Handle;
                bool? dialogResult = taskExchangeCreditTokenView.ShowDialog();
                if (dialogResult == false)
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
                if (!string.IsNullOrWhiteSpace(taskExchangeCreditTokenVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskExchangeCreditTokenVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskExchangeCreditTokenVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskExchangeCreditTokenVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskExchangeCreditTokenVM.PerformClose(taskExchangeCreditTokenView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-callExchangeToken()");
        }
        private void CallExchangeCredits(TaskType taskType)
        {
            log.Debug("Starts-callExchangeCredits()");
            TaskConvertPointsTimeView taskConvertPointsTimeView = null;
            TaskConvertPointsTimeVM taskConvertPointsTimeVM = null;
            try
            {
                taskConvertPointsTimeVM = new TaskConvertPointsTimeVM(ParafaitEnv.ExecutionContext, taskType, Common.Devices.PrimaryCardReader);
                taskConvertPointsTimeView = new TaskConvertPointsTimeView();
                taskConvertPointsTimeView.DataContext = taskConvertPointsTimeVM;
                ElementHost.EnableModelessKeyboardInterop(taskConvertPointsTimeView);
                WindowInteropHelper helper = new WindowInteropHelper(taskConvertPointsTimeView);
                helper.Owner = this.Handle;
                bool? dialogResult = taskConvertPointsTimeView.ShowDialog();
                if (dialogResult == false)
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
                if (!string.IsNullOrWhiteSpace(taskConvertPointsTimeVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskConvertPointsTimeVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskConvertPointsTimeVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskConvertPointsTimeVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskConvertPointsTimeVM.PerformClose(taskConvertPointsTimeView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-callExchangeCredits()");
        }


        private void CallRedeemEntitilements(TaskType taskType)
        {
            log.Debug("Starts-CallRedeemEntitilements()");
            TaskRedeemBonusForTicketsView taskRedeemBonusForTicketsView = null;
            TaskRedeemBonusForTicketsVM taskRedeemBonusForTicketsVM = null;
            try
            {
                taskRedeemBonusForTicketsVM = new TaskRedeemBonusForTicketsVM(ParafaitEnv.ExecutionContext, taskType, Common.Devices.PrimaryCardReader);
                taskRedeemBonusForTicketsView = new TaskRedeemBonusForTicketsView();
                taskRedeemBonusForTicketsView.DataContext = taskRedeemBonusForTicketsVM;
                ElementHost.EnableModelessKeyboardInterop(taskRedeemBonusForTicketsView);
                WindowInteropHelper helper = new WindowInteropHelper(taskRedeemBonusForTicketsView);
                helper.Owner = this.Handle;
                bool? dialogResult = taskRedeemBonusForTicketsView.ShowDialog();
                if (dialogResult == false)
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
                if (!string.IsNullOrWhiteSpace(taskRedeemBonusForTicketsVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskRedeemBonusForTicketsVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskRedeemBonusForTicketsVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskRedeemBonusForTicketsVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskRedeemBonusForTicketsVM.PerformClose(taskRedeemBonusForTicketsView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-CallRedeemEntitilements()");
        }
        private void CallBalanceTransfer()
        {
            log.Debug("Starts-CallBalanceTransfer()");
            TaskTransferBalanceView taskTransferBalanceView = null;
            TaskTransferBalanceVM taskTransferBalanceVM = null;
            try
            {
                taskTransferBalanceVM = new TaskTransferBalanceVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                taskTransferBalanceView = new TaskTransferBalanceView();
                taskTransferBalanceView.DataContext = taskTransferBalanceVM;
                ElementHost.EnableModelessKeyboardInterop(taskTransferBalanceView);
                WindowInteropHelper helper = new WindowInteropHelper(taskTransferBalanceView);
                helper.Owner = this.Handle;
                bool? dialogResult = taskTransferBalanceView.ShowDialog();
                if (dialogResult == false)
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
                if (!string.IsNullOrWhiteSpace(taskTransferBalanceVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskTransferBalanceVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskTransferBalanceVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskTransferBalanceVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskTransferBalanceVM.PerformClose(taskTransferBalanceView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-callTransferBalance()");
        }
        private void CallLoadBonus()
        {
            log.Debug("Starts-CallLoadBonus()");
            TaskLoadBonusView taskLoadBonusView = null;
            TaskLoadBonusVM taskLoadBonusVM = null;
            try
            {
                taskLoadBonusVM = new TaskLoadBonusVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                taskLoadBonusView = new TaskLoadBonusView();
                taskLoadBonusView.DataContext = taskLoadBonusVM;
                ElementHost.EnableModelessKeyboardInterop(taskLoadBonusView);
                WindowInteropHelper helper = new WindowInteropHelper(taskLoadBonusView);
                helper.Owner = this.Handle;
                bool? dialogResult = taskLoadBonusView.ShowDialog();
                if (dialogResult == false)
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
                if (!string.IsNullOrWhiteSpace(taskLoadBonusVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskLoadBonusVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskLoadBonusVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskLoadBonusVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskLoadBonusVM.PerformClose(taskLoadBonusView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-CallLoadBonus()");
        }
        private void CallPauseTime()
        {
            log.Debug("Starts-callPauseTime()");
            TaskPauseTimeView taskPauseTimeView = null;
            TaskPauseTimeVM taskPauseTimeVM = null;
            try
            {
                taskPauseTimeVM = new TaskPauseTimeVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                taskPauseTimeView = new TaskPauseTimeView();
                taskPauseTimeView.DataContext = taskPauseTimeVM;
                ElementHost.EnableModelessKeyboardInterop(taskPauseTimeView);
                WindowInteropHelper helper = new WindowInteropHelper(taskPauseTimeView);
                helper.Owner = this.Handle;
                taskPauseTimeView.ShowDialog();
                if (!string.IsNullOrWhiteSpace(taskPauseTimeVM.ErrorMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskPauseTimeVM.ErrorMessage, "ERROR");
                }
                if (!string.IsNullOrWhiteSpace(taskPauseTimeVM.SuccessMessage))
                {
                    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskPauseTimeVM.SuccessMessage, "MESSAGE");
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    taskPauseTimeVM.PerformClose(taskPauseTimeView);
                }
                catch (Exception)
                {
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.Debug("Ends-callPauseTime()");
        }
        private void CallTask(object Task, EventArgs e)
        {
            if (!checkLoginClockIn())
                return;
            if (!CheckCashdrawerAssignment())
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer")); // New Message 13 y Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer
                return;
            }
            Button TaskId = (Button)Task;
            lastTrxActivityTime = DateTime.Now;

            if (TaskId.Tag == null) // task button tag hold the name of the task - set at design time
                return;

            Application.DoEvents();

            string task = TaskId.Tag.ToString();
            if (task.StartsWith(TaskProcs.REFUNDCARD))
                task = TaskProcs.REFUNDCARD;
            //Start update 10-Jun-2016
            // Updated for Sales return task
            if (task.StartsWith(TaskProcs.SALESRETURNEXCHANGE))
                task = TaskProcs.SALESRETURNEXCHANGE;
            //End update 10-Jun-2016

            //double mgrApprovalLimit = 0;
            object parameter = null;
            switch (task)
            {
                case TaskProcs.TRANSFERCARD:
                    {
                        break;
                    }
                case TaskProcs.EXCHANGETOKENFORCREDIT:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(256), WARNING);
                            return;
                        }
                        //mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("TOKEN_FOR_CREDIT_LIMIT_FOR_MANAGER_APPROVAL"));
                        break;
                    }
                case TaskProcs.EXCHANGECREDITFORTOKEN:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(258), WARNING);
                            return;
                        }
                        break;
                    }
                case TaskProcs.LOADTICKETS:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(259), WARNING);
                            return;
                        }
                        //mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
                        break;
                    }
                case TaskProcs.CONSOLIDATE:
                    {
                        break;
                    }
                case TaskProcs.BALANCETRANSFER:
                    {
                        break;
                    }
                case TaskProcs.LOADMULTIPLE:
                    {
                        if (NewTrx != null) // check if there is unsaved transaction
                        {
                            DialogResult DResult = POSUtils.ParafaitMessageBox(MessageUtils.getMessage(203), MessageUtils.getMessage(204), MessageBoxButtons.YesNo);
                            if (DResult == DialogResult.No)
                                return;
                            else
                            {
                                if (!cancelTransaction())
                                {
                                    return;
                                }
                            }
                        }
                        break;
                    }
                case TaskProcs.REALETICKET:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(260), WARNING);
                            return;
                        }
                        break;
                    }
                case TaskProcs.REFUNDCARD:
                    {
                        if (NewTrx != null)
                        {
                            displayMessageLine(MessageUtils.getMessage(261), ERROR);
                            return;
                        }
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(262), WARNING);
                            return;
                        }
                        //mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("REFUND_AMOUNT_LIMIT_FOR_MANAGER_APPROVAL"));
                        break;
                    }
                case TaskProcs.LOADBONUS:
                    {
                        if (POSStatic.ALLOW_MANUAL_CARD_IN_LOAD_BONUS == false)
                        {
                            if (CurrentCard == null)
                            {
                                displayMessageLine(MessageUtils.getMessage(257), WARNING);
                                return;
                            }
                        }

                        if (CurrentCard != null && CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(263), WARNING);
                            return;
                        }
                        //mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL"));
                        break;
                    }
                case TaskProcs.DISCOUNT:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(264), WARNING);
                            return;
                        }
                        break;
                    }
                case TaskProcs.REDEEMLOYALTY:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(265), WARNING);
                            return;
                        }
                        //mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("REDEEM_LOYALTY_LIMIT_FOR_MANAGER_APPROVAL"));
                        break;
                    }
                case TaskProcs.SPECIALPRICING:
                    {
                        parameter = NewTrx;
                        break;
                    }
                case TaskProcs.REDEEMTICKETSFORBONUS:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(267), WARNING);
                            return;
                        }
                        //mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
                        break;
                    }
                //Start update 10-Jun-2016
                case TaskProcs.SALESRETURNEXCHANGE:
                    {
                        if (NewTrx != null)
                        {
                            displayMessageLine(MessageUtils.getMessage(261), ERROR);
                            return;
                        }
                        break;
                    }
                //End update 10-Jun-2016
                //Start update 16-Jun-2017
                case TaskProcs.REDEEMBONUSFORTICKET:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(1193), WARNING);
                            return;
                        }
                        //mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("REDEEM_BONUS_LIMIT_FOR_MANAGER_APPROVAL"));
                        break;
                    }
                case TaskProcs.PAUSETIMEENTITLEMENT:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(459), WARNING);
                            return;
                        }
                        break;
                    }
                case TaskProcs.EXCHANGECREDITFORTIME:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(459), WARNING);
                            return;
                        }
                        break;
                    }
                case TaskProcs.EXCHANGETIMEFORCREDIT:
                    {
                        if (CurrentCard == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(459), WARNING);
                            return;
                        }
                        break;
                    }
                case TaskProcs.HOLDENTITLEMENTS:
                    {
                        break;
                    }
                case TaskProcs.REDEEMVIRTUALPOINTS:
                    {
                        log.Debug("REDEEMVIRTUALLOYALTY");
                        if (CurrentCard == null)
                        {
                            log.Debug("CurrentCard is null");
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            return;
                        }
                        if (CurrentCard.CardStatus == "NEW")
                        {
                            log.Debug("CurrentCard is NEW");
                            displayMessageLine(MessageUtils.getMessage(265), WARNING);
                            return;
                        }
                        break;
                    }
                //End update 16-Jun-2017
                default: break;
            }

            SqlCommand cmd = Utilities.getCommand();

            cmd.CommandText = "select requires_manager_approval from task_type where task_type = @task_type";
            cmd.Parameters.AddWithValue("@task_type", task);
            if (cmd.ExecuteScalar().ToString() == "Y")
            {
                if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageUtils.getMessage(268), WARNING);
                    return;
                }
            }
            displayMessageLine("", MESSAGE);
            formatAndWritePole(TaskId.Text, "");
            if (task == TaskProcs.SALESRETURNEXCHANGE)
            {
                SalesReturn.frmSalesReturn frmSalesReturn;
                frmSalesReturn = new SalesReturn.frmSalesReturn();
                DialogResult DR = frmSalesReturn.ShowDialog();
                if (DR == DialogResult.Cancel)
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                displayMessageLine(frmSalesReturn.ReturnMessage, MESSAGE);
            }
            else
            {
                FormCardTasks taskForm = new FormCardTasks(task, CurrentCard, Utilities, parameter); // open the tasks form in dialog mode
                DialogResult DR = taskForm.ShowDialog();
                if (DR != DialogResult.Cancel)
                {
                    if (task == TaskProcs.EXCHANGECREDITFORTOKEN ||
                        task == TaskProcs.EXCHANGETOKENFORCREDIT ||
                        task == TaskProcs.LOADTICKETS ||
                        task == TaskProcs.LOADBONUS ||
                        task == TaskProcs.REALETICKET ||
                        task == TaskProcs.DISCOUNT ||
                        task == TaskProcs.REDEEMLOYALTY ||
                        task == TaskProcs.REDEEMVIRTUALPOINTS ||
                        task == TaskProcs.REDEEMTICKETSFORBONUS ||
                        task == TaskProcs.REDEEMBONUSFORTICKET ||               // Added on 16-jun-2017
                        task == TaskProcs.EXCHANGECREDITFORTIME ||
                        task == TaskProcs.PAUSETIMEENTITLEMENT ||
                        task == TaskProcs.EXCHANGETIMEFORCREDIT
                        )
                    {
                        if (CurrentCard == null)
                            CurrentCard = taskForm.CurrentCard;
                        if (CurrentCard != null)
                            CurrentCard.getCardDetails(CurrentCard.CardNumber);
                    }
                    else if (task == TaskProcs.REFUNDCARD ||
                             task == TaskProcs.HOLDENTITLEMENTS ||
                             task == TaskProcs.TRANSFERCARD ||
                             task == TaskProcs.CONSOLIDATE ||
                             task == TaskProcs.LOADMULTIPLE ||
                             task == TaskProcs.SPECIALPRICING)
                    {
                        CurrentCard = null;
                    }

                    displayCardDetails();
                    CurrentCard = null;

                    switch (task)
                    {
                        case TaskProcs.LOADMULTIPLE: LoadMultiple(taskForm.LoadMultipleCards.ToArray(), taskForm.LoadMultipleProducts); break;
                        case TaskProcs.SPECIALPRICING: specialPricing(); displayMessageLine(taskForm.ReturnMessage, MESSAGE); break;
                        default: displayMessageLine(taskForm.ReturnMessage, MESSAGE); break;
                    }

                    if (task == TaskProcs.REFUNDCARD)
                    {
                        //Begin Modification: Added a condition to prevent printing when games is refunded as credits/card balance/ Bonus
                        if (TaskProcs.TransactionId != -1)
                        {
                            if (Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "Y")
                            {
                                if (POSStatic.USE_FISCAL_PRINTER != "Y")
                                    PrintSpecificTransaction(TaskProcs.TransactionId, false);
                                else
                                {
                                    FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                                    string _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                                    FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);
                                    try
                                    {
                                        if (fiscalPrinter.PrintReceipt(TaskProcs.TransactionId, ref Message) == false)
                                        {
                                            if (fiscalPrinter != null && Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                                            {
                                                // Non fiscal type for type 'D' taxed products
                                                PrintSpecificTransaction(TaskProcs.TransactionId, false);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        POSUtils.ParafaitMessageBox(ex.Message);
                                    }
                                    endPrintAction();
                                }

                                TaskProcs.TransactionId = -1;
                            }
                            else if (Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "A")
                            {
                                if (POSStatic.USE_FISCAL_PRINTER == "Y")
                                {
                                    FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                                    string _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                                    FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);
                                    try
                                    {
                                        if (fiscalPrinter.PrintReceipt(TaskProcs.TransactionId, ref Message) == false)
                                        {
                                            if (fiscalPrinter != null && Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                                            {
                                                // Non fiscal type for type 'D' taxed products
                                                PrintSpecificTransaction(TaskProcs.TransactionId, false);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        POSUtils.ParafaitMessageBox(ex.Message);
                                    }
                                    endPrintAction();
                                }
                                else
                                {
                                    if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(205), "Refund Print", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                        PrintSpecificTransaction(TaskProcs.TransactionId, false);
                                }
                                TaskProcs.TransactionId = -1;
                            }
                            else
                                displayMessageLine(taskForm.ReturnMessage + ". " + MessageUtils.getMessage(206), MESSAGE);
                        }
                        //End: Added a condition to prevent printing when games is refunded as credits/card balance/ Bonus
                    }
                    else if (task == TaskProcs.REDEEMLOYALTY)    // Print 
                    {
                        if (Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "Y")
                        {
                            if (POSStatic.USE_FISCAL_PRINTER != "Y")
                                PrintSpecificTransaction(TaskProcs.TransactionId, false);

                            TaskProcs.TransactionId = -1;
                        }
                        else if (Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "A")
                        {
                            if (POSStatic.USE_FISCAL_PRINTER != "Y")
                            {
                                if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(207), "Redeem Print", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                    PrintSpecificTransaction(TaskProcs.TransactionId, false);
                            }
                            TaskProcs.TransactionId = -1;
                        }
                        else
                            displayMessageLine(taskForm.ReturnMessage + ". " + MessageUtils.getMessage(206), MESSAGE);
                    }
                    else if (task == TaskProcs.TRANSFERCARD)
                    {
                        if (taskForm.NewTrx != null && taskForm.NewTrx.TransactionDTO.TransactionId <= 0)
                        {
                            NewTrx = taskForm.NewTrx;
                            RefreshTrxDataGrid(taskForm.NewTrx);
                            nudQuantity.Value = 1;
                            displayButtonTexts();
                            buttonCancelLine.Enabled = false;
                            lastTrxActivityTime = DateTime.Now;
                            transferCardOTPApprovals = taskForm.TransferCardOTPApprovals;
                            isTransferCardTrx = true;
                            transferCardType = taskForm.TransferCardType;
                        }
                    }
                    else { TaskProcs.TransactionId = -1; }
                }
                else
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
            }
            if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                logOutUser();
        }

        //Begin Modification - 24-Feb-2016 - Modified approach to fix issues with Special pricing
        //We will not create temp trx but update new price for existing Newtrx and then update amounts
        void specialPricing()
        {
            if (NewTrx != null)
            {

                for (int i = 0; i < NewTrx.TrxLines.Count; i++)
                {
                    Transaction.TransactionLine line = NewTrx.TrxLines[i];
                    if (line.LineValid)
                    {
                        DataRow spProduct = NewTrx.getProductDetails(line.ProductID);
                        if (!line.UserPrice && line.Price != 0 && line.ProductTypeCode != "CARDDEPOSIT")
                        {
                            line.Price = Convert.ToDouble(spProduct["Price"]);
                        }
                    }
                }
                //NewTrx = tempTrx;
                NewTrx.updateAmounts();
                RefreshTrxDataGrid(NewTrx);
                PoleDisplay.writeSecondLine(("Total: " + NewTrx.Net_Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)).PadLeft(20));
            }
        }

        private void LoadMultiple(Card[] pLoadMultipleCards, int[] pLoadMultipleProducts)
        {
            if (NewTrx == null)
                NewTrx = new Transaction(POSStatic.POSPrintersDTOList, Utilities);
            string message = "";
            displayMessageLine(MessageUtils.getMessage(270), MESSAGE);
            for (int i = 0; i < pLoadMultipleCards.Length; i++)
            {
                if (pLoadMultipleCards[i] == null)
                    break;
                CurrentCard = pLoadMultipleCards[i];
                for (int j = 0; j < pLoadMultipleProducts.Length; j++)
                {
                    if (pLoadMultipleProducts[j] == -1)
                        break;
                    message = "";
                    decimal qty = 2; // set to > 1 so that ui is not updated for each card
                    CreateProduct(pLoadMultipleProducts[j], NewTrx.getProductDetails(pLoadMultipleProducts[j]), ref qty);
                }
            }

            RefreshTrxDataGrid(NewTrx);
            displayButtonTexts();
            if (message != "")
                displayMessageLine(message, ERROR);

            if (NewTrx != null && NewTrx.TrxLines.Count == 0)
            {
                NewTrx = null;
                cleanUpOnNullTrx();
            }
        }
    }
}
