//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Data;

//namespace Semnox.Core.Utilities
//{

//    /// <summary>
//    /// This is a common class intheolder solutions. 
//    /// This is a  static class, hence creating static class in the  solution as CommonStaticClass
//    /// This  has to bedsicussed withtheteam
//    /// </summary>
//    public static class CommonStaticClass
//    {
//        public static Utilities Utilities;
//        public static System.Drawing.Color SkinColor = System.Drawing.Color.White;
//        public static int UIProfile = 1;
//        public static void setupVisuals(Control c)
//        {
//            string type = c.GetType().ToString().ToLower();

//            if (c.HasChildren)
//            {
//                c.BackColor = SkinColor;
//                foreach (Control cc in c.Controls)
//                {
//                    setupVisuals(cc);
//                }
//            }
//            if (type.Contains("radiobutton"))
//            {
//                ;
//            }
//            else if (type.Contains("forms.button"))
//            {
//                setupButtonVisuals((Button)c);
//            }
//            else if (type.Contains("tabpage"))
//            {
//                TabPage tp = (TabPage)c;
//                tp.BackColor = SkinColor;
//            }
//            else if (type == "system.windows.forms.datagridview")
//            {
//                DataGridView dg = (DataGridView)c;
//                dg.BackgroundColor = SkinColor;
//            }
//        }

//        public static void setupButtonVisuals(Button b)
//        {
//            b.FlatStyle = FlatStyle.Flat;
//            b.FlatAppearance.BorderSize = 0;
//            b.FlatAppearance.MouseDownBackColor =
//            b.FlatAppearance.MouseOverBackColor =
//            b.BackColor = System.Drawing.Color.Transparent;
//            b.Font = new System.Drawing.Font("arial", 8.5f);
//            b.ForeColor = System.Drawing.Color.Black;
//            if (b.Width < 100)
//                b.Width = 90;
//            b.Height = 25;
//            b.BackgroundImageLayout = ImageLayout.Stretch;
//            if (UIProfile == 2)
//                b.BackgroundImage = Properties.Resources.ButtonNav2Normal;
//            else
//                b.BackgroundImage = Properties.Resources.normal3;

//            b.MouseDown += b_MouseDown;
//            b.MouseUp += b_MouseUp;
//        }
//        static void b_MouseUp(object sender, MouseEventArgs e)
//        {
//            Button b = sender as Button;
//            if (UIProfile == 2)
//                b.BackgroundImage = Properties.Resources.ButtonNav2Normal;
//            else
//                b.BackgroundImage = Properties.Resources.normal3;
//            b.ForeColor = System.Drawing.Color.Black;
//        }

//        static void b_MouseDown(object sender, MouseEventArgs e)
//        {
//            Button b = sender as Button;
//            if (UIProfile == 2)
//                b.BackgroundImage = Properties.Resources.ButtonNav2Pressed;
//            else
//                b.BackgroundImage = Properties.Resources.pressed3;
//            b.ForeColor = System.Drawing.Color.White;
//        }

//        static void bs_AddingNew(object sender, System.ComponentModel.AddingNewEventArgs e)
//        {
//            try
//            {
//                BindingSource bs = sender as BindingSource;
//                DataSet ds = bs.DataSource as DataSet;
//                string tableName = (bs.List as System.Data.DataView).Table.TableName;
//                if (ds == null) // dgv based on relationship
//                {
//                    bs = bs.DataSource as BindingSource;
//                    ds = bs.DataSource as DataSet;
//                    if (ds == null) // dgv based on relationship
//                    {
//                        bs = bs.DataSource as BindingSource;
//                        ds = bs.DataSource as DataSet;
//                    }
//                }
//                DataTable dt = ds.Tables[tableName];
//                try
//                {
//                    dt.Columns["guid"].DefaultValue = Guid.NewGuid();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "Table: " + dt.TableName);
//                }

//                if (Common.ParafaitEnv.IsCorporate)
//                {
//                    try
//                    {
//                        dt.Columns["site_id"].DefaultValue = Common.ParafaitEnv.SiteId;
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show(ex.Message, "Table: " + dt.TableName);
//                    }
//                }
//            }
//            catch { }
//        }
//        public static void setupGrid(ref DataGridView dataGrid)
//        {
//            dataGrid.BackgroundColor = SkinColor;
//            dataGrid.RowHeadersDefaultCellStyle.BackColor = SkinColor;
//            try
//            {
//                BindingSource bs = dataGrid.DataSource as BindingSource;
//                if (bs != null)
//                {
//                    bs.AddingNew += new System.ComponentModel.AddingNewEventHandler(bs_AddingNew);
//                    setSiteIdFilter(bs);
//                }
//            }
//            catch (Exception ex)
//            {
//                if (Common.ParafaitEnv.IsCorporate)
//                {
//                    MessageBox.Show(ex.Message, "Corporate Site");
//                    MessageBox.Show("Data Grid Name: " + dataGrid.Name);
//                }
//            }
//        }

//        public static void setSiteIdFilter(BindingSource bs)
//        {
//            try
//            {
//                if (Common.ParafaitEnv.IsCorporate) // corporate db
//                {
//                    if (bs.Filter == null || bs.Filter == "")
//                    {
//                        if (Common.ParafaitEnv.IsMasterSite)
//                            bs.Filter = "(site_id is null or site_id = " + Common.ParafaitEnv.SiteId.ToString() + ")";
//                        else
//                            bs.Filter = "site_id = " + Common.ParafaitEnv.SiteId.ToString();
//                    }
//                    else
//                    {
//                        if (Common.ParafaitEnv.IsMasterSite)
//                            bs.Filter = "(" + bs.Filter + ") and (site_id is null or site_id = " + Common.ParafaitEnv.SiteId.ToString() + ")";
//                        else
//                            bs.Filter = "(" + bs.Filter + ") and site_id = " + Common.ParafaitEnv.SiteId.ToString();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "Binding Source: " + bs.DataMember);
//            }
//        }
//    }
//}
