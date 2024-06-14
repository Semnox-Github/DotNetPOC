/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - Parafait Queue Manager Common Solution
* 
**************
**Version Log
**************
*Version     Date             Modified By    Remarks          
*********************************************************************************************
*2.80        11-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
//using CompletIT.Windows.Forms.Export.Excel;

namespace ParafaitQueueManagement
{
    static class Common
    {
        public static Utilities Utilities;
        public static ParafaitEnv ParafaitEnv;

        public static System.Drawing.Color SkinColor = System.Drawing.Color.White;

        public static List<string> openForms = new List<string>();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static void openForm(Form parentForm, string childFormName, object[] Params, bool Reuse, bool modal)
        //{
        //    Form form;
        //    if (Reuse)
        //    {
        //        foreach (Form f in Application.OpenForms)
        //        {
        //            if (f.Name == childFormName)
        //            {
        //                int pos = -1;
        //                for (int i = 0; i < openForms.Count; i++)
        //                {
        //                    if (openForms[i] == f.Name)
        //                        pos = i;
        //                }
        //                if (pos >= 0)
        //                {
        //                    openForms.RemoveAt(pos);
        //                }
        //                openForms.Add(f.Name);
        //                f.Show();
        //                f.Activate();
        //                f.Focus();
        //                return;
        //            }
        //        }
        //    }

        //    switch (childFormName)
        //    {
        //        case "ViewCards": form = new ViewCards(); break;
        //        case "NewCard":
        //        case "UpdateCard": form = new IssueCard((System.Data.DataRow)Params[0]); break;
        //        case "CardActivity": form = new CardActivity((long)Params[0], (string)Params[1]); break;
        //        case "Masters": form = new MasterData(); form.Text = "Hub Setup"; form.Name = childFormName; break;
        //        case "Game Profile": form = new MasterData(); form.Text = "Game Profile"; form.Name = childFormName; break;
        //        case "Games": form = new MasterData(); form.Text = "Game Setup"; form.Name = childFormName; break;
        //        case "Machines": form = new MasterData(); form.Text = "Machine Setup"; form.Name = childFormName; break;
        //        case "Customers": form = new Customers((long)Params[0], (long)Params[1]); break;
        //        case "Products": form = new ProductSetup((string)Params[0]); form.Text = "Product Setup"; form.Name = childFormName; break;
        //        case "Product Type": form = new ProductSetup((string)Params[0]); form.Text = "Product Type"; form.Name = childFormName; break;
        //        case "ProductSetupMenu": form = new ProductSetupMenu(); form.Text = "Product Setup Menu"; form.Name = childFormName; break;
        //        case "Discounts": form = new Product.DiscountsSetup(); form.Name = childFormName; break;
        //        case "Tax": form = new ProductSetup(""); form.Text = "Tax Setup"; form.Name = childFormName; break;
        //        case "Site": form = new Site(); break;
        //        case "TaskTypes": form = new TaskTypes(); break;
        //        case "User Roles":
        //        case "Users": form = new Users(); form.Name = childFormName; break;
        //        case "Promotions": form = new PromotionCalendar(); form.Name = childFormName; break;
        //        case "ParafaitDefaults": form = new ParafaitDefaults(); break;
        //        case "ViewTrx": form = new ViewTransactions(); form.Name = childFormName; break;
        //        case "CardInventory": form = new frm_cardMaintenance(); form.Name = childFormName; break;
        //        case "ProductKey": form = new Product_Key(); break;
        //        case "PurgeData": form = new ManualPurge(); break;
        //        case "CardTypeRule": form = new CardTypeRule((int)Params[0], (int)Params[1], (int)Params[2], (string)Params[3]); form.Name = childFormName; break;
        //        case "ParafaitDashboard": form = new Parafait_Dashboard(); form.Name = childFormName; break;
        //        case "POSManagement": form = new POSManagement(); form.Name = childFormName; break;
        //        case "WirelessDashBoard": form = new WirelessDashboard(); form.Name = childFormName; break;
        //        case "ProductDetails": form = new ProductDetails((long)Params[0], (string)Params[1]); form.Name = childFormName; break;
        //        case "ProductCalendar": form = new ProductCalendar((long)Params[0]); form.Name = childFormName; break;
        //        case "ChangePassword": form = new ChangePassword(); form.Name = childFormName; break;
        //        case "ReservationSetup": form = new BookingClass(); form.Name = childFormName; break;
        //        case "Loyalty": if (Params.Length > 1)
        //            {
        //                form = new Loyalty((string)Params[0], (int)Params[1]);
        //                form.Name = childFormName;
        //            }
        //            else
        //            {
        //                form = new Loyalty((string)Params[0]);
        //                form.Name = childFormName;
        //            }
        //            break;
        //        case "LoyaltyManagement": form = new LoyaltyManagement(); form.Name = childFormName; break;
        //        case "PrintSetup": form = new PrintReceiptTemplate(); form.Name = childFormName; break;
        //        case "Partners": form = new Partners(); form.Name = childFormName; break;
        //        case "MachineGroups": form = new MachineGroups(); form.Name = childFormName; break;
        //        case "AdManagement": form = new AdManagement(); form.Name = childFormName; break;
        //        case "CustomAttributes": form = new CustomAttributes(); form.Name = childFormName; break;
        //        case "CardType": form = new Membership(); form.Name = childFormName; break;
        //        case "Lookups": form = new Lookups(); form.Name = childFormName; break;
        //        case "ProductCategory": form = new ProductCategory(); form.Name = childFormName; break;
        //        case "PriceList": form = new PriceList(); form.Name = childFormName; break;
        //        case "ReaderConfig": form = new frmReaderConfiguration(-1, -1, -1); form.Name = childFormName; break;
        //        case "KioskSetup": form = new KioskSetup(); form.Name = childFormName; break;
        //        case "Messages": form = new frmMessages(); form.Name = childFormName; break;
        //        case "FacilitySeatLayout": form = new FacilitySeatLayout(Params[0]); form.Name = childFormName; break;
        //        case "EventViewer": form = new Reports.frmEventViewer(); form.Name = childFormName; break;
        //        case "TableLayout": form = new Site_Setup.frmFacilityTables(); form.Name = childFormName; break;
        //        case "Campaigns": form = new frmCampaigns(); form.Name = childFormName; break;
        //        case "HRSetup": form = new HRSetup(); form.Name = childFormName; break;
        //        case "LeaveMgmt": form = new LeaveCycle(); form.Name = childFormName; break;
        //        case "ApplyLeave": form = new ApplyLeave(); form.Name = childFormName; break;
        //        case "HRTasks": form = new HRTasks(); form.Name = childFormName; break;
        //        case "Attendance": form = new HR.Attendance(); form.Name = childFormName; break;
        //        case "TrxProfiles": form = new Site_Setup.TrxProfiles(); form.Name = childFormName; break;
        //        case "MaintenanceRequests": form = new Tools.MaintenanceRequests(); form.Name = childFormName; break;
        //        case "GamePerformance": form = new GameReports(); form.Name = childFormName; break;
        //        default: form = new Form(); form.Name = "Blank"; break;
        //    }

        //    try
        //    {
        //        setupVisuals(form);

        //        try
        //        {
        //            form.Icon = new System.Drawing.Icon(Environment.CurrentDirectory + "\\Resources\\Parafait icon.ico");
        //        }
        //        catch { }

        //        if (modal)
        //        {
        //            form.ShowDialog();
        //        }
        //        else
        //        {
        //            int navFormWidth = 0;
        //            foreach (Form f in Application.OpenForms)
        //            {
        //                if (f.Name == "Navigation")
        //                {
        //                    navFormWidth = f.Width;
        //                    break;
        //                }
        //            }
        //            form.Location = new System.Drawing.Point(navFormWidth, 0);
        //            if (form.FormBorderStyle != FormBorderStyle.FixedToolWindow)
        //            {
        //                form.FormBorderStyle = FormBorderStyle.Sizable;
        //                form.StartPosition = FormStartPosition.Manual;
        //                form.Width = SystemInformation.WorkingArea.Width - navFormWidth - 10;
        //                form.Height = SystemInformation.WorkingArea.Height - 60;
        //            }
        //            form.MdiParent = parentForm;
        //            form.AutoScroll = true;
        //            int pos = -1;
        //            for (int i = 0; i < openForms.Count; i++)
        //            {
        //                if (openForms[i] == form.Name)
        //                    pos = i;
        //            }
        //            if (pos >= 0)
        //            {
        //                openForms.RemoveAt(pos);
        //            }
        //            openForms.Add(form.Name);
        //            form.Show();
        //            form.Activate();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("FATAL ERROR. Unable to open Form: " + form.Name + "\n" + ex.Message, "Management Studio");
        //        MessageBox.Show(ex.StackTrace, "Management Studio");
        //    }
        //}

        public static void setupVisuals(Control c)
        {
            log.LogMethodEntry(c);
            string type = c.GetType().ToString().ToLower();

            if (c.HasChildren)
            {
                c.BackColor = SkinColor;
                foreach (Control cc in c.Controls)
                {
                    setupVisuals(cc);
                }
            }
            if (type.Contains("radiobutton"))
            {
                ;
            }
            else if (type.Contains("forms.button"))
            {
                setupButtonVisuals((Button)c);
            }
            else if (type.Contains("tabpage"))
            {
                TabPage tp = (TabPage)c;
                tp.BackColor = SkinColor;
            }
            else if (type == "system.windows.forms.datagridview")
            {
                DataGridView dg = (DataGridView)c;
                dg.BackColor = SkinColor;
            }
            log.LogMethodExit();
        }

        public static void setupButtonVisuals(Button b)
        {
            log.LogMethodEntry(b);
            b.BackColor = System.Drawing.Color.LightBlue;
            b.Font = new System.Drawing.Font("arial", 9f);
            b.ForeColor = System.Drawing.Color.Black;
            b.FlatAppearance.BorderColor = b.BackColor;
            b.FlatStyle = FlatStyle.Popup;
            b.Size = new System.Drawing.Size(90, 25);
            log.LogMethodExit();
        }

        public static void setupGrid(ref DataGridView dataGrid)
        {
            log.LogMethodEntry(dataGrid);
            dataGrid.BackgroundColor = SkinColor;
            dataGrid.RowHeadersDefaultCellStyle.BackColor = SkinColor;
            try
            {
                BindingSource bs = dataGrid.DataSource as BindingSource;
                if (bs != null)
                {
                    bs.AddingNew += new System.ComponentModel.AddingNewEventHandler(bs_AddingNew);
                    setSiteIdFilter(bs);
                }
            }
            catch (Exception ex)
            {
                if (Common.ParafaitEnv.IsCorporate)
                {
                    MessageBox.Show(ex.Message, "Corporate Site");
                    MessageBox.Show("Data Grid Name: " + dataGrid.Name);
                }
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        static void bs_AddingNew(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                BindingSource bs = sender as BindingSource;
                DataSet ds = bs.DataSource as DataSet;
                string tableName = (bs.List as System.Data.DataView).Table.TableName;
                if (ds == null) // dgv based on relationship
                {
                    bs = bs.DataSource as BindingSource;
                    ds = bs.DataSource as DataSet;
                    if (ds == null) // dgv based on relationship
                    {
                        bs = bs.DataSource as BindingSource;
                        ds = bs.DataSource as DataSet;
                    }
                }
                DataTable dt = ds.Tables[tableName];
                try
                {
                    dt.Columns["guid"].DefaultValue = Guid.NewGuid();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    MessageBox.Show(ex.Message, "Table: " + dt.TableName);
                }

                if (Common.ParafaitEnv.IsCorporate)
                {
                    try
                    {
                        dt.Columns["site_id"].DefaultValue = Common.ParafaitEnv.SiteId;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        MessageBox.Show(ex.Message, "Table: " + dt.TableName);
                    }
                }
                log.LogMethodExit();
            }
            catch { }
        }

        public static void setSiteIdFilter(BindingSource bs)
        {
            log.LogMethodEntry(bs);
            try
            {
                if (Common.ParafaitEnv.IsCorporate) // corporate db
                {
                    if (bs.Filter == null || bs.Filter == "")
                        bs.Filter = "site_id = " + Common.ParafaitEnv.SiteId.ToString();
                    else
                        bs.Filter = "(" + bs.Filter + ") and site_id = " + Common.ParafaitEnv.SiteId.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Binding Source: " + bs.DataMember);
            }
            log.LogMethodExit();
        }

        public static void displayInfo(string info) // display in info label of toolstrip
        {
            log.LogMethodEntry(info);
            try
            {
                Form f = Application.OpenForms["Main"];
                if (f != null)
                {
                    Control[] ctrls = f.Controls.Find("statusStrip", true);
                    if (ctrls.Length > 0)
                    {
                        StatusStrip st = ctrls[0] as StatusStrip;
                        ToolStripItem lbl = st.Items["toolInfo"];
                        lbl.Text = info;
                    }
                }
            }
            catch { }
            log.LogMethodExit();
        }

        public static object getSiteid()
        {
            log.LogMethodEntry();
            if (Common.ParafaitEnv.IsCorporate)
            {
                log.LogMethodExit(Common.ParafaitEnv.SiteId);
                return Common.ParafaitEnv.SiteId;
            }
            else
            {
                log.LogMethodExit(DBNull.Value);
                return DBNull.Value;
            }
        }

        public static bool ContinueWithoutSaving(DataSet ds)
        {
            log.LogMethodEntry(ds);
            if (ds.HasChanges())
            {
                if (MessageBox.Show("There are unsaved changes. Do you want to continue without saving?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        public static void DGVDataError(string Name, DataGridView dgv, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(Name, dgv);
            try
            {
                MessageBox.Show("Error in " + Name + " data at row " + (e.RowIndex + 1).ToString() + " column " + dgv.Columns[e.ColumnIndex].DataPropertyName +
                    ": " + e.Exception.Message, "Data Error");
            }
            catch { }
            e.Cancel = true;
            log.LogMethodExit();
        }
    }
}
