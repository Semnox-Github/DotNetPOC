using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_FnB_Kiosk
{
    public static class Common
    {
        internal static Utilities utils;
        internal static ParafaitEnv parafaitEnv;

        internal static System.Drawing.Color PrimaryForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
        internal static System.Drawing.Color RedColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(31)))), ((int)(((byte)(54)))));
        internal static System.Drawing.Color GreenColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));

        internal static Semnox.Parafait.logging.Logger log = null;

        internal static int TimeOutDuration = 60;

        internal static void OpenScreen(ScreenModel.UIPanelElement callingElement)
        {
            logEnter();

            try
            {
                ScreenModel screen = new ScreenModel(callingElement.ActionScreenId);

                BaseForm f = (BaseForm)Assembly.GetExecutingAssembly().CreateInstance("Parafait_FnB_Kiosk." + screen.CodeObjectName);
                f.SetScreenModel(screen, callingElement);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                logException(ex);
                ShowMessage(ex.Message);
            }

            logExit();
        }

        internal static void initEnv()
        {
            logEnter();

            try
            {
                utils = new Utilities();
                parafaitEnv = utils.ParafaitEnv;
                parafaitEnv.SetPOSMachine("", Environment.MachineName);
                parafaitEnv.Initialize();
                ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
                if (utils.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(utils.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserPKId(utils.ParafaitEnv.ExternalPOSUserId);
                machineUserContext.SetMachineId(utils.ParafaitEnv.POSMachineId);
                utils.ParafaitEnv.User_Id = utils.ParafaitEnv.ExternalPOSUserId;
                machineUserContext.SetUserId(utils.ParafaitEnv.LoginID);
                KioskStatic.Utilities = utils;
                KioskStatic.get_config();
                int moneyScreenTimeout = 0;
                try
                {
                    moneyScreenTimeout = Convert.ToInt32(utils.getParafaitDefaults("MONEY_SCREEN_TIMEOUT"));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.Info("MONEY_SCREEN_TIMEOUT not defined");
                }
                TimeOutDuration = Math.Max(60, moneyScreenTimeout);
            }
            catch (Exception ex)
            {
                try
                {
                    logToFile(ex.Message);
                    Parafait_Kiosk.SetUp sf = new Parafait_Kiosk.SetUp();
                    sf.StartPosition = FormStartPosition.CenterScreen;
                    sf.ShowDialog();
                    Environment.Exit(0);
                }
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message);
                    Environment.Exit(0);
                }
            }

            logExit();
        }

        internal static DialogResult ShowDialog(string Message)
        {
            logToFile(Message);
            return (new frmMessage(Message, true)).ShowDialog();
        }

        internal static DialogResult ShowMessage(string Message)
        {
            logToFile(Message);
            return (new frmMessage(Message)).ShowDialog();
        }

        internal static DialogResult ShowAlert(string Message)
        {
            logToFile(Message);
            return (new frmAlert(Message)).ShowDialog();
        }

        internal static void logException(Exception ex)
        {
            log.Debug(new System.Diagnostics.StackFrame(1, false).GetMethod().Name + "(): Exception");
            log.Debug(ex.Message + Environment.NewLine + ex.StackTrace);
        }

        internal static void logEnter()
        {
            log.Debug(new System.Diagnostics.StackFrame(1, false).GetMethod().Name + "(): Enter");
        }

        internal static void logExit()
        {
            log.Debug(new System.Diagnostics.StackFrame(1, false).GetMethod().Name + "(): Exit");
        }

        internal static void GoHome()
        {
            logEnter();

            if (UserTransaction.OrderDetails.transactionPaymentsDTO.GatewayPaymentProcessed)
            {
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1115, UserTransaction.OrderDetails.transactionPaymentsDTO.Amount.ToString(utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                logExit();
                return;
            }

            UserTransaction.NewOrder(-1);

            foreach (Form f in Application.OpenForms)
            {
                if (f is frmHome || f is frmSplash)
                {
                    ;
                }
                else
                {
                    f.Close();
                }
            }

            logExit();
        }

        internal static void logToFile(string message)
        {
            log.Debug(new System.Diagnostics.StackFrame(1, false).GetMethod().Name +"(): " + message);
        }
    }
}
