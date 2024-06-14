/********************************************************************************************
 * Class Name - RedemptionUtils                                                                         
 * Description - Common UI Display
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Semnox.Parafait.Redemption
{
    public class CommonUIDisplay
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Utilities Utilities;
        public static ParafaitEnv ParafaitEnv;
        //public static ParafaitUtils.MessageUtils MessageUtils; 

        public static System.Drawing.Color SkinColor = System.Drawing.Color.White;
        public static List<string> openForms = new List<string>();

       


        public static void setupVisuals(Control c)
        {
            log.LogMethodEntry();
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
                dg.BackgroundColor = SkinColor;//dg.BackColor = SkinColor;
            }
            log.LogMethodExit();
        }

        public static void setupButtonVisuals(Button b)
        {
            log.LogMethodEntry();
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseDownBackColor =
            b.FlatAppearance.MouseOverBackColor =
            b.BackColor = System.Drawing.Color.Transparent;
            b.Font = new System.Drawing.Font("arial", 8.5f);
            b.ForeColor = System.Drawing.Color.Black;
            //b.Size = new System.Drawing.Size(90, 25);
            b.BackgroundImageLayout = ImageLayout.Stretch;
            b.BackgroundImage = global::Semnox.Parafait.Redemption.Properties.Resources.normal3;

            b.MouseDown += b_MouseDown;
            b.MouseUp += b_MouseUp;
            log.LogMethodExit();
        }

        static void b_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.BackgroundImage = global::Semnox.Parafait.Redemption.Properties.Resources.normal3;
            b.ForeColor = System.Drawing.Color.Black;
            log.LogMethodExit();
        }

        static void b_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.BackgroundImage = global::Semnox.Parafait.Redemption.Properties.Resources.pressed3;
            b.ForeColor = System.Drawing.Color.White;
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
            catch(Exception ex)
            {
                log.Error("Error while executing setupGrid()" + ex.Message);
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
                    MessageBox.Show(ex.Message, "Table: " + dt.TableName);
                }

                if (Utilities.ParafaitEnv.IsCorporate)
                {
                    try
                    {
                        dt.Columns["site_id"].DefaultValue = Utilities.ParafaitEnv.SiteId;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Table: " + dt.TableName);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing bs_AddingNew()" + ex.Message);
            }
            log.LogMethodExit();
        }

        public static void setSiteIdFilter(BindingSource bs)
        {
            log.LogMethodEntry();
            try
            {
                if (Utilities.ParafaitEnv.IsCorporate) // corporate db
                {
                    if (bs.Filter == null || bs.Filter == "")
                        bs.Filter = "site_id = " + Utilities.ParafaitEnv.SiteId.ToString();
                    else
                        bs.Filter = "(" + bs.Filter + ") and site_id = " + Utilities.ParafaitEnv.SiteId.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing setSiteIdFilter()" + ex.Message);
                MessageBox.Show(ex.Message, "Binding Source: " + bs.DataMember);
            }
            log.LogMethodExit();
        }
    }
}
