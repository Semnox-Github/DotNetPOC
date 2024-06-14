/********************************************************************************************
 * Project Name - ParafaitPOS
 * Description  - CardActivity zoom form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     07-Jul-2021   Fiona                   to call new Load bonus task screen
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.TransactionUI;
using Semnox.Parafait.User;
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
using System.Windows.Forms.Integration;
using System.Windows.Interop;

namespace Parafait_POS
{
    public partial class frmCardActivityZoom : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmCardActivityZoom(DataTable dgvGamePlay, DataTable dgvPurchases)
        {
            InitializeComponent();
            dataGridViewGamePlay.DataSource = dgvGamePlay;
            dataGridViewPurchases.DataSource = dgvPurchases;

            POSStatic.Utilities.setupDataGridProperties(ref dataGridViewGamePlay);
            POSStatic.Utilities.setupDataGridProperties(ref dataGridViewPurchases);
            POSStatic.Utilities.setLanguage(this);
        }

        private void dataGridViewPurchases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                if (dataGridViewPurchases.Columns[e.ColumnIndex].Equals(dcBtnCardActivityTrxPrint))
                {
                    if (dataGridViewPurchases["ActivityType", e.RowIndex].Value.ToString() == "TRANSACTION"
                    || dataGridViewPurchases["ActivityType", e.RowIndex].Value.ToString() == "PAYMENT")
                    {
                        object TrxId = dataGridViewPurchases["RefId", e.RowIndex].Value;
                        if (TrxId != null && TrxId != DBNull.Value)
                        {
                            if (POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(490), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                List<int> trxIdList = new List<int>();

                                trxIdList.Add((int)TrxId);

                                string message = "";

                                PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(POSStatic.Utilities);
                                if (!printMultipleTransactions.Print(trxIdList, true, ref message))
                                {
                                    POSUtils.ParafaitMessageBox(message);
                                    log.Warn("PrintSpecificTransaction(" + TrxId + ",rePrint) - Unable to Print Transaction error: " + message);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
        }

        private void dataGridViewGamePlay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dataGridViewGamePlay_CellClick()");
            if (e.RowIndex <= 0 || e.ColumnIndex < 0)
            {
                log.Info("Ends-dataGridViewGamePlay_CellClick() as e.RowIndex <= 0 || e.ColumnIndex < 0");
                return;
            }

            try
            {
                if (dataGridViewGamePlay.Columns[e.ColumnIndex].Name == "ReverseGamePlay")
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "ENABLE_GAMEPLAY_REVERSAL_IN_POS") == "N")
                    {
                        log.Info("Ends-dataGridViewGamePlay_CellClick() as ENABLE_GAMEPLAY_REVERSAL_IN_POS is disabled");
                        return;
                    }
                    if (POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(519), "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        log.Info("Ends-dataGridViewGamePlay_CellClick() as Confirm dialog No is Clicked");
                        return;
                    }
                    if ((ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "DISABLE_REVERSAL_OF_GAMEPLAY_PAST_DAYS")).Equals("Y"))
                    {
                        DateTime bussStartTime = POSStatic.Utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                        DateTime bussEndTime = bussStartTime.AddDays(1);
                        if (POSStatic.Utilities.getServerTime() < bussStartTime)
                        {
                            bussStartTime = bussStartTime.AddDays(-1);
                            bussEndTime = bussStartTime.AddDays(1);
                        }
                        object gamePlayDate = POSStatic.Utilities.executeScalar(@"select top 1 play_date 
                                                                          from gameplay
								                                         where GamePlay_Id = @gameplayId",
                                                new SqlParameter("@gameplayId", dataGridViewGamePlay["gameplay_id", e.RowIndex].Value));
                        if (gamePlayDate != null && gamePlayDate != DBNull.Value
                            && Convert.ToDateTime(gamePlayDate) < bussStartTime)
                        {
                            string Message = POSStatic.Utilities.MessageUtils.getMessage(5088, (int)(bussStartTime - Convert.ToDateTime(gamePlayDate)).TotalDays, 1);
                            POSUtils.ParafaitMessageBox(Message, "Reverse Game Play");
                            log.LogVariableState("Message ", Message);
                            log.LogMethodExit(false);
                            return;
                        }
                    }

                    if (POSStatic.Utilities.executeScalar(@"select top 1 1 
                                                     from trx_lines tl, TransactionLineGamePlayMapping tlg
								where tl.trxid = tlg.trxid
								  and tl.lineid = tlg.lineid
								  and tlg.isactive = 1
                                  and tlg.GamePlayId = @gameplayId
								  and  exists (select 1 
								                    from products ppin
								   			       where ppin.product_name = 'Load Bonus Task'
												     and ppin.product_id = tl.product_id)",
                                                new SqlParameter("@gameplayId", dataGridViewGamePlay["gameplay_id", e.RowIndex].Value)) != null)
                    {
                        POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(522));
                        log.Info("Ends-dataGridViewGamePlay_CellClick() as Gameplay already refunded");
                        return;
                    }
                    DataTable dt = POSStatic.Utilities.executeDataTable(@"select card_id, 
                                                                credits + courtesy + bonus + time + CardGame + CPCardBalance + CPCredits + CPBonus,
                                                                isnull(g.play_credits, isnull(gpr.play_credits, 0)), gameplay_id, m.machine_name
                                                                from gameplay gp, machines m, games g, game_profile gpr
                                                                where gameplay_id = @gameplayId
                                                                and gp.machine_id = m.machine_id
                                                                and g.game_id = m.game_id
                                                                and g.game_profile_id = gpr.game_profile_id",
                                                                new SqlParameter("@gameplayId", dataGridViewGamePlay["gameplay_id", e.RowIndex].Value));

                    decimal price = Convert.ToDecimal(dt.Rows[0][1]);
                    if (price == 0)
                        price = Convert.ToDecimal(dt.Rows[0][2]);
                    Card card = new Card((int)dt.Rows[0][0],
                                                 POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);

                    object[] pars = { card.CardNumber, price, dt.Rows[0][3], dt.Rows[0][4] };
                    TaskLoadBonusView taskLoadBonusView = null;
                    TaskLoadBonusVM taskLoadBonusVM = null;
                    try
                    {
                        ParafaitPOS.App.machineUserContext = POSStatic.Utilities.ParafaitEnv.ExecutionContext;
                        this.Cursor = Cursors.WaitCursor;
                        try
                        {
                            taskLoadBonusVM = new TaskLoadBonusVM(POSStatic.Utilities.ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader, pars);
                        }
                        catch (UserAuthenticationException ue)
                        {
                            POSUtils.ParafaitMessageBox(Semnox.Parafait.Languages.MessageContainerList.GetMessage(POSStatic.Utilities.ParafaitEnv.ExecutionContext, 2927, System.Configuration.ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                            throw new UnauthorizedException(ue.Message);
                        }
                        this.Cursor = Cursors.Default;
                        ParafaitPOS.App.EnsureApplicationResources();
                        taskLoadBonusView = new TaskLoadBonusView();
                        taskLoadBonusView.DataContext = taskLoadBonusVM;
                        ElementHost.EnableModelessKeyboardInterop(taskLoadBonusView);
                        WindowInteropHelper helper = new WindowInteropHelper(taskLoadBonusView);
                        helper.Owner = this.Handle;
                        bool? dialogResult = taskLoadBonusView.ShowDialog();
                        if (dialogResult == false)
                        {
                            POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(269) );
                            log.Debug(POSStatic.Utilities.MessageUtils.getMessage(269) + "Game play reversal");
                        }
                    }
                    catch (Exception ex)
                    {
                        taskLoadBonusView.Close();
                        throw ex;
                    }
                    finally
                    {
                        (Application.OpenForms["POS"] as Parafait_POS.POS).lastTrxActivityTime = DateTime.Now;
                    }
                    (Application.OpenForms["POS"] as Parafait_POS.POS).lastTrxActivityTime = DateTime.Now;
                    (Application.OpenForms["POS"] as Parafait_POS.POS).reconnectReaders();
                    if (!string.IsNullOrWhiteSpace(taskLoadBonusVM.ErrorMessage))
                    {
                        POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(taskLoadBonusVM.ErrorMessage));
                    }
                    if (!string.IsNullOrWhiteSpace(taskLoadBonusVM.SuccessMessage))
                    {
                        POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(taskLoadBonusVM.SuccessMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-dataGridViewGamePlay_CellClick() due to exception " + ex.Message);
            }
            log.Debug("Ends-dataGridViewGamePlay_CellClick()");
        }
    }
}
