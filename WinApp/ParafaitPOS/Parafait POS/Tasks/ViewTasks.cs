using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    public partial class ViewTasks : Form
    {
        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        TaskProcs TaskProcs = POSStatic.TaskProcs;
        ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;

        string[] ReversibleTasks = { "LOADBONUS", "LOADTICKETS" };
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public ViewTasks()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-ViewTasks()");//Added for logger function on 08-Mar-2016
            Utilities.setLanguage();
            InitializeComponent();
            log.Debug("Ends-ViewTasks()");//Added for logger function on 08-Mar-2016
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click()");//Added for logger function on 08-Mar-2016
            Close();
            log.Debug("Ends-btnClose_Click()");//Added for logger function on 08-Mar-2016
        }

        private void ViewTasks_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-ViewTasks_Load()");//Added for logger function on 08-Mar-2016
            refresh();
            Utilities.setLanguage(this);
            log.Debug("Ends-ViewTasks_Load()");//Added for logger function on 08-Mar-2016
        }

        void refresh()
        {
            log.Debug("Starts-refresh()");//Added for logger function on 08-Mar-2016
            DataTable dtTasks = Utilities.executeDataTable(@"select top 100 task_date, task_type_name, c1.card_number,
                                                                           value_loaded, c2.card_number transferred_to, pos_machine, 
                                                                           u.username, t.remarks, ty.task_type, t.task_id
                                                                           from tasks t left outer join cards c2 on c2.card_id = t.transfer_to_card_id,
                                                                                task_type ty, cards c1, users u
                                                                          where t.task_type_id = ty.task_type_id
                                                                            and t.card_id = c1.card_id
                                                                            and t.user_id = u.user_id
                                                                            and u.user_id = @userId
                                                                            order by task_date desc",
                                    new SqlParameter[] { new SqlParameter("userId", ParafaitEnv.User_Id) });
            dgvTasks.DataSource = dtTasks;
            Utilities.setupDataGridProperties(ref dgvTasks);
            dgvTasks.Columns[dgvTasks.Columns.Count - 1].Visible = false;
            dgvTasks.Columns[dgvTasks.Columns.Count - 2].Visible = false;
            dgvTasks.Columns["task_date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvTasks.Columns["value_loaded"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            log.Debug("Ends-refresh()");//Added for logger function on 08-Mar-2016
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvTasks_CellContentClick()");//Added for logger function on 08-Mar-2016
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0 || e.ColumnIndex > 0)
				{
					log.Info("Ends-dgvTasks_CellContentClick() as e.ColumnIndex < 0 || e.RowIndex < 0 || e.ColumnIndex > 0");//Added for logger function on 08-Mar-2016
                    return;
				}

                if (ReversibleTasks.Contains(dgvTasks["task_type", e.RowIndex].Value.ToString()))
                {
                    if (Convert.ToDecimal(dgvTasks["value_loaded", e.RowIndex].Value) < 0)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(311), "Task Reversal");
                        log.Info("Ends-dgvTasks_CellContentClick() as Cannot reverse a reversed Task");//Added for logger function on 08-Mar-2016
                        return;
                    }

                    if (Utilities.executeScalar("select top 1 1 from tasks where remarks like '%" + dgvTasks["task_id", e.RowIndex].Value.ToString() + "]'", new SqlParameter[] { }) != null)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(312), "Task Reversal");
                        log.Info("Ends-dgvTasks_CellContentClick() as Task already reversed");//Added for logger function on 08-Mar-2016
                        return;
                    }

                    int mgrId = -1;
                    if (!Authenticate.Manager(ref mgrId))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(313), "Task Reversal");
                        log.Info("Ends-dgvTasks_CellContentClick() as Manager approval required for Task reversal");//Added for logger function on 08-Mar-2016
                        return;
                    }

                    string TrxRemarks = "";
                    GenericDataEntry trxRemarks = new GenericDataEntry(1);
                    trxRemarks.Text = MessageUtils.getMessage(201);
                    trxRemarks.DataEntryObjects[0].mandatory = true;
                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage(132);
                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TrxRemarks = trxRemarks.DataEntryObjects[0].data + " [" + dgvTasks["task_id", e.RowIndex].Value.ToString() + "]";
                    }
                    else
                    {
						log.Info("Ends-dgvTasks_CellContentClick() as Transaction Remarks dialog was cancelled");//Added for logger function on 08-Mar-2016                        
                        return;
                    }

                    string message = "";
                    if (!TaskProcs.ReverseTask(Convert.ToInt32(dgvTasks["task_id", e.RowIndex].Value), TrxRemarks, ref message))
					{
                        POSUtils.ParafaitMessageBox(message, "Task Reversal Error");
						log.Error("dgvTasks_CellContentClick() - Task Reversal Error");//Added for logger function on 08-Mar-2016                        
					}
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(314), "Task Reversal");
                        log.Info("dgvTasks_CellContentClick() - Task Reversed successfully");//Added for logger function on 08-Mar-2016
                        refresh();
                    }
                }
                else
                {
                    string tasks = "";
                    foreach (string s in ReversibleTasks)
                        tasks += s + ", ";
                    tasks = tasks.TrimEnd(',', ' ');

                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(316, dgvTasks["task_type", e.RowIndex].Value) + Environment.NewLine + Environment.NewLine +
                                    MessageUtils.getMessage(317, "") + Environment.NewLine + tasks, "Task Reversal");
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-dgvTasks_CellContentClick() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-dgvTasks_CellContentClick() ");//Added for logger function on 08-Mar-2016
        }
    }
}
