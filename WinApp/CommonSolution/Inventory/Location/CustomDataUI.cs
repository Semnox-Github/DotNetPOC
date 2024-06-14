/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
 *1.00        29-Apr-2016            Soumya                     Added screen to enable editing
 *                                                              custom attribute
 *2.70.2        13-Aug-2019            Deeksha                    Added logger methods.
 *********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class CustomDataUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string Applicability;
        string PrimaryKeyColumn, TableName;
        int PrimaryKey, CustomDataSetId = -1;
        Utilities Utilities;

        public CustomDataUI(string pApplicability, int pPrimaryKey, string pTableName, Utilities _Utilities)
        {
            log.LogMethodEntry(pApplicability, pPrimaryKey, pTableName, _Utilities);
            InitializeComponent();
            Utilities = _Utilities;
            Utilities.setLanguage(this);
            Applicability = pApplicability;
            PrimaryKey = pPrimaryKey;
            TableName = pTableName;
            //CommonFuncs.Utilities.setupVisuals(this);
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

            lblHeading.Text = "Custom Attribute Data for " + Applicability;
            lblHeading.Text = textInfo.ToTitleCase(lblHeading.Text.ToLower());
            log.LogMethodExit();
        }

        private void PopulateData()
        {
            log.LogMethodEntry();
            switch (Applicability)
            {
                case "INVPRODUCT": TableName = "Product";
                    PrimaryKeyColumn = "Productid";
                    break;
                case "LOCATION": TableName = "Location";
                    PrimaryKeyColumn = "LocationId";
                    break;
                default: return;
            }
            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select CustomDataSetId from " + TableName + " where " + PrimaryKeyColumn + " = " + PrimaryKey.ToString();
            object o = cmd.ExecuteScalar();
            if (o == null)
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(613) + PrimaryKey.ToString());
                Close();
            }

            if (o != DBNull.Value)
            {
                CustomDataSetId = Convert.ToInt32(o);
                cmd.CommandText = "select cd.ValueId, " +
                                         "CustomDataText, " +
                                         "CustomDataNumber, " +
                                         "CustomDataDate, " +
                                         "c.CustomAttributeId, c.Type " +
                                    "from CustomAttributes c, CustomData cd " +
                                    "where cd.CustomAttributeId = c.CustomAttributeId " +
                                    "and cd.CustomDataSetId = @CustomDataSetId";
                cmd.Parameters.AddWithValue("@CustomDataSetId", CustomDataSetId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (Control c in flpAttributes.Controls)
                    {
                        if (c.Tag != null && c.Tag.ToString() == dt.Rows[i]["CustomAttributeId"].ToString())
                        {
                            string dataType = dt.Rows[i]["Type"].ToString();
                            switch (dataType)
                            {
                                case "TEXT":
                                    {
                                        (c as TextBox).Text = dt.Rows[i]["CustomDataText"].ToString();
                                        break;
                                    }
 
                                case "NUMBER":
                                    {
                                        (c as TextBox).Text = dt.Rows[i]["CustomDataNumber"].ToString();
                                        break;
                                    } 

                                case "DATE":
                                    {
                                        if (dt.Rows[i]["CustomDataDate"] != DBNull.Value)
                                        {
                                            if (c.GetType().ToString().Contains("TextBox"))
                                                c.Text = Convert.ToDateTime(dt.Rows[i]["CustomDataDate"]).ToString(Utilities.ParafaitEnv.DATE_FORMAT);
                                            else
                                                (c as DateTimePicker).Value = Convert.ToDateTime(dt.Rows[i]["CustomDataDate"]);
                                        }
                                    } break;

                                case "LIST":
                                    {
                                        (c as ComboBox).SelectedValue = dt.Rows[i]["ValueId"];
                                        break;
                                    }
                                default:
                                    {
                                        (c as TextBox).Text = dt.Rows[i]["CustomDataValue"].ToString();
                                    } break;
                            }
                            if (dataType != "Date") // date type needs 2 iterations. hence don't break the loop
                                break;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        void createUI()
        {
            log.LogMethodEntry();
            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select * from CustomAttributes where Applicability = @Applicability and (site_id = @site_id or @site_id = -1) order by sequence";

            cmd.Parameters.AddWithValue("@Applicability", Applicability);
            cmd.Parameters.AddWithValue("@site_id", Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1); ;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(559, Applicability));
                Close();
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = dt.Rows[i]["Name"].ToString();

                System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

                lbl.Text = lbl.Text.Replace('_', ' ');
                //lbl.Text = textInfo.ToTitleCase(lbl.Text.ToLower());
                lbl.Text += ":";

                FlowLayoutPanel fpLabel = new FlowLayoutPanel();
                fpLabel.FlowDirection = FlowDirection.RightToLeft;
                fpLabel.Width = 230;
                fpLabel.Height = 30;
                flpAttributes.Controls.Add(fpLabel);
                fpLabel.Controls.Add(lbl);

                string dataType = dt.Rows[i]["Type"].ToString();
                switch (dataType)
                {
                    case "TEXT":
                        {
                            TextBox txt = new TextBox();
                            txt.Width = 120;
                            txt.Tag = dt.Rows[i]["CustomAttributeId"];
                            flpAttributes.Controls.Add(txt);
                        } break;

                    case "NUMBER":
                        {
                            TextBox txtdecimal = new TextBox();
                            txtdecimal.Width = 60;
                            txtdecimal.TextAlign = HorizontalAlignment.Right;
                            txtdecimal.Tag = dt.Rows[i]["CustomAttributeId"];
                            
                            txtdecimal.KeyPress += new KeyPressEventHandler(txtdecimal_KeyPress);
                            flpAttributes.Controls.Add(txtdecimal);
                        } break;

                    case "DATE":
                        {
                            TextBox txt = new TextBox();
                            txt.Width = 120;
                            txt.Tag = dt.Rows[i]["CustomAttributeId"];
                            flpAttributes.Controls.Add(txt);

                            DateTimePicker dtp = new DateTimePicker();
                            dtp.Width = 25;
                            dtp.Tag = txt.Tag;
                            dtp.ValueChanged +=new EventHandler(dtp_ValueChanged);
                            flpAttributes.Controls.Add(dtp);
                        } break;

                    case "LIST":
                        {
                            ComboBox cmbCustom = new ComboBox();
                            cmbCustom.Width = 150;
                            cmbCustom.Tag = dt.Rows[i]["CustomAttributeId"];
                            SqlCommand cmdValues = Utilities.getCommand();
                            cmdValues.CommandText = "select null Value, '' Display union all " + 
                                                    "select ValueId, Value " +
                                                "from CustomAttributeValueList " +
                                                " where CustomAttributeId = @id";
                            cmdValues.Parameters.AddWithValue("@id", dt.Rows[i]["CustomAttributeId"]);
                            SqlDataAdapter daCustom = new SqlDataAdapter(cmdValues);
                            DataTable dtCustom = new DataTable();
                            daCustom.Fill(dtCustom);
                            
                            cmbCustom.DataSource = dtCustom;
                            cmbCustom.ValueMember = "Value";
                            cmbCustom.DisplayMember = "Display";
                            cmbCustom.DropDownStyle = ComboBoxStyle.DropDownList;
                            flpAttributes.Controls.Add(cmbCustom);
                        }
                        break;

                    default:
                        {
                            TextBox txt = new TextBox();
                            txt.Width = 120;
                            txt.Tag = dt.Rows[i]["CustomAttributeId"];
                            flpAttributes.Controls.Add(txt);
                        } break;
                }
            }
            log.LogMethodExit();
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DateTimePicker dtp = sender as DateTimePicker;
            foreach (Control c in flpAttributes.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == dtp.Tag.ToString() && c.GetType().ToString().Contains("TextBox"))
                {
                    c.Text = dtp.Value.ToString(Utilities.ParafaitEnv.DATE_FORMAT);
                    break;
                }
            }
            log.LogMethodExit();
        }

        private void txtdecimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            char decimalChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar) && !(e.KeyChar == decimalChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void CustomDataUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            createUI();
            PopulateData();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateData();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(flpAttributes.Tag) == false)
            //    return;

            log.LogMethodEntry();
            SqlCommand cmdDMLSql = Utilities.getCommand();
            SqlTransaction SQLTrx = cmdDMLSql.Connection.BeginTransaction();
            cmdDMLSql.Transaction = SQLTrx;
            SqlCommand cmdSql = Utilities.getCommand();
            cmdSql.Transaction = SQLTrx;
            if (CustomDataSetId == -1)
            {
                cmdSql.CommandText = "insert into CustomDataSet (dummy, site_id) values(null, @site_id); select @@identity";
                if (Utilities.ParafaitEnv.IsCorporate)
                    cmdSql.Parameters.AddWithValue("@site_id", Utilities.ParafaitEnv.SiteId);
                else
                    cmdSql.Parameters.AddWithValue("@site_id", DBNull.Value);

                CustomDataSetId = Convert.ToInt32(cmdSql.ExecuteScalar());
                cmdSql.CommandText = "update " + TableName + " set CustomDataSetId = " + CustomDataSetId.ToString() + " where " + PrimaryKeyColumn + " = " + PrimaryKey.ToString();
                cmdSql.ExecuteNonQuery();                 
            }

            try
            {
                cmdSql.CommandText = "select type from CustomAttributes where CustomAttributeId = @CustomAttributeId";
                cmdSql.Parameters.Clear();
                cmdSql.Parameters.Add("@CustomAttributeId", SqlDbType.Int);
                foreach (Control c in flpAttributes.Controls)
                {
                    if (c.Tag != null)
                    {
                        if (c.GetType().ToString().Contains("TextBox"))
                        {
                            cmdSql.Parameters["@CustomAttributeId"].Value = c.Tag;
                            string val = c.Text.Trim();
                            cmdDMLSql.Parameters.Clear();
                            if (val == "")
                            {
                                cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                                cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                                cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                            }
                            else
                            {
                                string type = cmdSql.ExecuteScalar().ToString();
                                if (type == "TEXT")
                                {
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataText", val);
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                                }
                                else if (type == "NUMBER")
                                {
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                                    try
                                    {
                                        cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", Convert.ToDecimal(val));
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show(Utilities.MessageUtils.getMessage(14) + val);
                                        SQLTrx.Rollback();
                                        log.Error("Error while executing btnSave_Click() when type=Number" + ex.Message);
                                        log.LogMethodExit();
                                        return;
                                    }
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                                }
                                else if (type == "DATE")
                                {
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                                    try
                                    {
                                        cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", Convert.ToDateTime(val));
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show(Utilities.MessageUtils.getMessage(15, " :") + val);
                                        SQLTrx.Rollback();
                                        log.Error("Error while executing btnSave_Click() when type=Date" + ex.Message);
                                        log.LogMethodExit();
                                        return;
                                    }
                                }
                                else
                                {
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataText", val);
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                                    cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                                }
                            }
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataSetId", CustomDataSetId);
                            cmdDMLSql.Parameters.AddWithValue("@CustomAttributeId", c.Tag);
                            cmdDMLSql.Parameters.AddWithValue("@ValueId", DBNull.Value);

                            cmdDMLSql.CommandText = "update CustomData set ValueId = @ValueId, CustomDataText = @CustomDataText, CustomDataNumber = @CustomDataNumber, CustomDataDate = @CustomDataDate " +
                                            "where CustomDataSetId = @CustomDataSetId and CustomAttributeId = @CustomAttributeId";

                            if (cmdDMLSql.ExecuteNonQuery() == 0)
                            {
                                cmdDMLSql.CommandText = "insert into CustomData (CustomDataSetId, CustomAttributeId, ValueId, CustomDataText, CustomDataNumber, CustomDataDate, site_id) " +
                                            "values (@CustomDataSetId, @CustomAttributeId, @ValueId, @CustomDataText, @CustomDataNumber, @CustomDataDate, @site_id)";

                                if (Utilities.ParafaitEnv.IsCorporate)
                                    cmdDMLSql.Parameters.AddWithValue("@site_id", Utilities.ParafaitEnv.SiteId);
                                else
                                    cmdDMLSql.Parameters.AddWithValue("@site_id", DBNull.Value);

                                cmdDMLSql.ExecuteNonQuery();
                            }         
                        }
                        else if (c.GetType().ToString().Contains("ComboBox"))
                        {
                            cmdDMLSql.Parameters.Clear();
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataSetId", CustomDataSetId);
                            cmdDMLSql.Parameters.AddWithValue("@CustomAttributeId", c.Tag);
                            if ((c as ComboBox).SelectedIndex <= 0)
                            {
                                cmdDMLSql.Parameters.AddWithValue("@ValueId", DBNull.Value);
                            }
                            else
                            {
                                cmdDMLSql.Parameters.AddWithValue("@ValueId", (c as ComboBox).SelectedValue);
                            }
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);

                            cmdDMLSql.CommandText = "update CustomData set ValueId = @ValueId, CustomDataText = @CustomDataText, CustomDataNumber = @CustomDataNumber, CustomDataDate = @CustomDataDate " +
                                            "where CustomDataSetId = @CustomDataSetId and CustomAttributeId = @CustomAttributeId";

                            if (cmdDMLSql.ExecuteNonQuery() == 0)
                            {
                                cmdDMLSql.CommandText = "insert into CustomData (CustomDataSetId, CustomAttributeId, ValueId, CustomDataText, CustomDataNumber, CustomDataDate, site_id) " +
                                            "values (@CustomDataSetId, @CustomAttributeId, @ValueId, @CustomDataText, @CustomDataNumber, @CustomDataDate, @site_id)";
                                if (Utilities.ParafaitEnv.IsCorporate)
                                    cmdDMLSql.Parameters.AddWithValue("@site_id", Utilities.ParafaitEnv.SiteId);
                                else
                                    cmdDMLSql.Parameters.AddWithValue("@site_id", DBNull.Value);

                                cmdDMLSql.ExecuteNonQuery();
                            }            
                        }
                    }
                }
                SQLTrx.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SQLTrx.Rollback();
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
    }
}
