using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class CustomAttributes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        FlowLayoutPanel flpAttributes;
        string PrimaryKeyColumn, TableName;
        string _Applicability;

        public class Applicability
        {
            public const string CUSTOMER = "CUSTOMER";
            public const string PRODUCT = "PRODUCT";
            public const string MACHINE = "MACHINE";
        }

        Semnox.Core.Utilities.Utilities Utilities;
        public CustomAttributes(string pApplicability, Semnox.Core.Utilities.Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(pApplicability, ParafaitUtilities);
            Utilities = ParafaitUtilities;
            _Applicability = pApplicability;
            switch (_Applicability)
            {
                case Applicability.CUSTOMER: TableName = "Customers";
                    PrimaryKeyColumn = "customer_id";
                    break;
                case Applicability.PRODUCT: TableName = "Products";
                    PrimaryKeyColumn = "product_id";
                    break;
                case Applicability.MACHINE: TableName = "Machines";
                    PrimaryKeyColumn = "machine_id";
                    break;
                default: return;
            }
            log.LogMethodExit(null);
        }

        public void createUI(FlowLayoutPanel pflpAttributes)
        {
            log.LogMethodEntry(pflpAttributes);
            flpAttributes = pflpAttributes;

            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select * from CustomAttributes where Applicability = @Applicability order by sequence";
            cmd.Parameters.AddWithValue("@Applicability", _Applicability);
            log.LogVariableState("@Applicability", _Applicability);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                flpAttributes.Tag = false;
                flpAttributes.Parent.Visible = false;
                log.LogMethodExit(null);
                return;
            }
            flpAttributes.Tag = true;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = dt.Rows[i]["Name"].ToString();

                System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

                lbl.Text = lbl.Text.Replace('_', ' ');
                lbl.Text = textInfo.ToTitleCase(lbl.Text.ToLower());
                lbl.Text += ":";

                lbl.AutoSize = false;
                lbl.Width = flpAttributes.Width - 10;
                lbl.Height = 15;

                flpAttributes.Controls.Add(lbl);

                string dataType = dt.Rows[i]["Type"].ToString();
                switch (dataType)
                {
                    case "TEXT":
                        {
                            TextBox txt = new TextBox();
                            txt.Width = 120;
                            txt.Tag = dt.Rows[i]["Name"];
                            flpAttributes.Controls.Add(txt);
                        } break;

                    case "NUMBER":
                        {
                            TextBox txtdecimal = new TextBox();
                            txtdecimal.Width = 60;
                            txtdecimal.TextAlign = HorizontalAlignment.Right;
                            txtdecimal.Tag = dt.Rows[i]["Name"];

                            txtdecimal.KeyPress += new KeyPressEventHandler(txtdecimal_KeyPress);
                            flpAttributes.Controls.Add(txtdecimal);
                        } break;

                    case "DATE":
                        {
                            TextBox txt = new TextBox();
                            txt.Width = 120;
                            txt.Tag = dt.Rows[i]["Name"];
                            flpAttributes.Controls.Add(txt);

                            DateTimePicker dtp = new DateTimePicker();
                            dtp.Width = 22;
                            dtp.Tag = txt.Tag;
                            dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                            flpAttributes.Controls.Add(dtp);
                        } break;

                    case "LIST":
                        {
                            SqlCommand cmdValues = Utilities.getCommand();
                            cmdValues.CommandText = "select null Value, '' Display union all " +
                                                    "select ValueId, Value " +
                                                "from CustomAttributeValueList " +
                                                " where CustomAttributeId = @id";
                            cmdValues.Parameters.AddWithValue("@id", dt.Rows[i]["CustomAttributeId"]);
                            log.LogVariableState("@id", dt.Rows[i]["CustomAttributeId"]);
                            SqlDataAdapter daCustom = new SqlDataAdapter(cmdValues);
                            DataTable dtCustom = new DataTable();
                            daCustom.Fill(dtCustom);

                            if (dtCustom.Rows.Count == 3
                                && (new string[] { "0", "1" }).Contains(dtCustom.Rows[1][1].ToString())
                                && (new string[] { "0", "1" }).Contains(dtCustom.Rows[2][1].ToString()))
                            {
                                dtCustom.Rows.RemoveAt(0);
                                CheckBox chkCustom = new CheckBox();
                                chkCustom.AutoSize = true;
                                chkCustom.Tag = dt.Rows[i]["Name"];
                                chkCustom.Text = dt.Rows[i]["Name"].ToString();
                                flpAttributes.Controls.Add(chkCustom);
                                Control label = flpAttributes.GetNextControl(chkCustom, false);
                                if (label != null)
                                    label.ForeColor = label.BackColor;

                                cmdValues.CommandText = @"select Value
                                                    from CustomAttributeValueList
                                                    where CustomAttributeId = @id and isDefault = 'Y'";
                                object o = cmdValues.ExecuteScalar();
                                if (o != null)
                                    chkCustom.Checked = o.ToString().Equals("1");
                                else
                                    chkCustom.Checked = false;
                            }
                            else if (dtCustom.Rows.Count <= 4) // upto 3 values. use radio buttons
                            {
                                dtCustom.Rows.RemoveAt(0);
                                foreach (DataRow dr in dtCustom.Rows)
                                {
                                    RadioButton rbCustom = new RadioButton();
                                    rbCustom.AutoSize = true;
                                    rbCustom.Tag = dt.Rows[i]["Name"];
                                    rbCustom.Text = dr["Display"].ToString();
                                    rbCustom.Name = dt.Rows[i]["Name"].ToString() + dr["Value"].ToString();
                                    flpAttributes.Controls.Add(rbCustom);
                                }

                                cmdValues.CommandText = @"select Value
                                                    from CustomAttributeValueList
                                                    where CustomAttributeId = @id and isDefault = 'Y'";
                                object o = cmdValues.ExecuteScalar();
                                if (o != null)
                                {
                                    foreach (Control c in flpAttributes.Controls)
                                    {
                                        if (c.GetType().ToString().ToLower().Contains("radio")
                                            && c.Tag.Equals(dt.Rows[i]["Name"])
                                            && c.Text.Equals(o.ToString()))
                                        {
                                            (c as RadioButton).Checked = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (Control c in flpAttributes.Controls)
                                    {
                                        if (c.GetType().ToString().ToLower().Contains("radio")
                                            && c.Tag.Equals(dt.Rows[i]["Name"]))
                                        {
                                            (c as RadioButton).Checked = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else // more than 3 values specified. use combo box
                            {
                                ComboBox cmbCustom = new ComboBox();
                                cmbCustom.Width = 150;
                                cmbCustom.Tag = dt.Rows[i]["Name"];
                                cmbCustom.DataSource = dtCustom;
                                cmbCustom.ValueMember = "Value";
                                cmbCustom.DisplayMember = "Display";
                                cmbCustom.DropDownStyle = ComboBoxStyle.DropDownList;
                                flpAttributes.Controls.Add(cmbCustom);

                                cmdValues.CommandText = @"select ValueId
                                                    from CustomAttributeValueList
                                                    where CustomAttributeId = @id and isDefault = 'Y'";
                                object o = cmdValues.ExecuteScalar();
                                if (o != null)
                                {
                                    dtCustom.Rows.RemoveAt(0);
                                    cmbCustom.SelectedValue = o;
                                }
                            }
                        }
                        break;

                    default:
                        {
                            TextBox txt = new TextBox();
                            txt.Width = 120;
                            txt.Tag = dt.Rows[i]["Name"];
                            flpAttributes.Controls.Add(txt);
                        } break;
                }
            }
            log.LogMethodExit(null);
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DateTimePicker dtp = sender as DateTimePicker;
            foreach (Control c in flpAttributes.Controls)
            {
                if (c.Tag != null && c.Tag.Equals(dtp.Tag) && c.GetType().ToString().Contains("TextBox"))
                {
                    c.Text = dtp.Value.ToString(Utilities.ParafaitEnv.DATE_FORMAT);
                    break;
                }
            }
            log.LogMethodExit(null);
        }

        private void txtdecimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar) && !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
            log.LogMethodExit(null);
        }

        public void clearData()
        {
            log.LogMethodEntry();
            foreach (Control c in flpAttributes.Controls)
            {
                if (c.Tag != null)
                {
                    if (c.GetType().ToString().Contains("TextBox"))
                    {
                        c.Text = "";
                    }
                    else if (c.GetType().ToString().Contains("ComboBox") 
                        || (c.GetType().ToString().Contains("Radio"))
                        || (c.GetType().ToString().Contains("Check")))
                    {
                        SqlCommand cmdValues = Utilities.getCommand();
                        cmdValues.CommandText = @"select ValueId, Value
                                                    from CustomAttributeValueList cv, CustomAttributes c
                                                    where cv.CustomAttributeId = c.CustomAttributeId 
                                                    and c.Name = @CName 
                                                    and c.Applicability = @Applicability
                                                    and isDefault = 'Y'";
                        cmdValues.Parameters.AddWithValue("@CName", c.Tag);
                        cmdValues.Parameters.AddWithValue("@Applicability", _Applicability);
                        log.LogVariableState("@CName", c.Tag);
                        log.LogVariableState("@Applicability", _Applicability);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmdValues);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (c.GetType().ToString().Contains("ComboBox"))
                                (c as ComboBox).SelectedValue = dt.Rows[0][0];
                            else if (c.GetType().ToString().Contains("Radio"))
                            {
                                if (c.Name.Substring(c.Tag.ToString().Length).Equals(dt.Rows[0][0].ToString()))
                                    (c as RadioButton).Checked = true;
                            }
                            else
                                (c as CheckBox).Checked = dt.Rows[0][1].ToString().Equals("1");
                        }
                        else
                        {
                            if (c.GetType().ToString().Contains("ComboBox"))
                                (c as ComboBox).SelectedIndex = -1;
                            else if (c.GetType().ToString().Contains("Radio"))
                                (c as RadioButton).Checked = false; //Modified
                            else
                                (c as CheckBox).Checked = false;
                        }
                    }
                }
            }
            log.LogMethodExit(null);
        }

        public void PopulateData(int PrimaryKey, int CustomDataSetId)
        {
            log.LogMethodEntry(PrimaryKey, CustomDataSetId);
            if (CustomDataSetId == -1 || Convert.ToBoolean(flpAttributes.Tag) == false)
            {
                clearData();
                log.LogMethodExit(null);
                return;
            }

            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select CustomDataSetId from " + TableName + " where " + PrimaryKeyColumn + " = " + PrimaryKey.ToString();
            object o = cmd.ExecuteScalar();
            if (o == null)
            {
                log.LogMethodExit(null);
                return;
            }

            if (o != DBNull.Value)
            {
                CustomDataSetId = Convert.ToInt32(o);
                cmd.CommandText = @"select cd.ValueId,
                                         CustomDataText, 
                                         CustomDataNumber, 
                                         CustomDataDate, 
                                         c.CustomAttributeId, c.Type, c.Name, cv.Value
                                    from CustomAttributes c, CustomData cd 
			                            left outer join CustomAttributeValueList cv
			                            on cv.ValueId = cd.ValueId
                                    where cd.CustomAttributeId = c.CustomAttributeId 
                                    and cd.CustomDataSetId = @CustomDataSetId";
                cmd.Parameters.AddWithValue("@CustomDataSetId", CustomDataSetId);
                log.LogVariableState("@CustomDataSetId", CustomDataSetId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (Control c in flpAttributes.Controls)
                    {
                        if (c.Tag != null && c.Tag.Equals(dt.Rows[i]["Name"]))
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
                                        if (c.GetType().ToString().ToLower().Contains("combobox"))
                                            (c as ComboBox).SelectedValue = dt.Rows[i]["ValueId"];
                                        else if (c.GetType().ToString().ToLower().Contains("radio"))
                                        {
                                            foreach (Control rb in flpAttributes.Controls)
                                            {
                                                if (rb.GetType().ToString().ToLower().Contains("radio")
                                                    && rb.Name.Substring(rb.Tag.ToString().Length).Equals(dt.Rows[i]["ValueId"].ToString()))
                                                {
                                                    (rb as RadioButton).Checked = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            (c as CheckBox).Checked = dt.Rows[i]["Value"].ToString().Equals("1");
                                        }
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
            log.LogMethodExit(null);
        }

        public bool Save(int PrimaryKey, ref int CustomDataSetId, List<KeyValuePair<object, object>> CustomDataList, ref string Message)
        {
            log.LogMethodEntry(PrimaryKey, CustomDataSetId, CustomDataList, Message);
            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand cmdDMLSql = Utilities.getCommand(SQLTrx);
            SqlCommand cmdSql = Utilities.getCommand(SQLTrx);

            object CustomAttributeId;
            if (CustomDataSetId == -1)
            {
                cmdSql.CommandText = "insert into CustomDataSet (dummy) values(null); select @@identity";
                CustomDataSetId = Convert.ToInt32(cmdSql.ExecuteScalar());
                cmdSql.CommandText = "update " + TableName + " set CustomDataSetId = " + CustomDataSetId.ToString() + " where " + PrimaryKeyColumn + " = " + PrimaryKey.ToString();
                cmdSql.ExecuteNonQuery();
            }

            try
            {
                cmdSql.CommandText = "select type, CustomAttributeId from CustomAttributes where Name = @CustomAttribute and Applicability = @Applicability";
                cmdSql.Parameters.Clear();
                cmdSql.Parameters.Add("@CustomAttribute", SqlDbType.NVarChar);
                cmdSql.Parameters.AddWithValue("@Applicability", _Applicability);
                log.LogVariableState("@Applicability", _Applicability);
                SqlDataAdapter ds = new SqlDataAdapter();

                foreach (KeyValuePair<object, object> c in CustomDataList)
                {
                    cmdSql.Parameters["@CustomAttribute"].Value = c.Key;
                    ds.SelectCommand = cmdSql;
                    DataTable dt = new DataTable();
                    ds.Fill(dt);
                    string type = dt.Rows[0][0].ToString();
                    CustomAttributeId = dt.Rows[0][1];

                    object val = c.Value;
                    cmdDMLSql.Parameters.Clear();
                    if (val == null || val == DBNull.Value || val.ToString() == "")
                    {
                        cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                        cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                        cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                        cmdDMLSql.Parameters.AddWithValue("@ValueId", DBNull.Value);
                        log.LogVariableState("@CustomDataText", DBNull.Value);
                        log.LogVariableState("@CustomDataNumber", DBNull.Value);
                        log.LogVariableState("@CustomDataDate", DBNull.Value);
                        log.LogVariableState("@ValueId", DBNull.Value);
                    }
                    else
                    {
                        if (type == "TEXT")
                        {
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataText", val);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@ValueId", DBNull.Value);
                            log.LogVariableState("@CustomDataText", val);
                            log.LogVariableState("@CustomDataNumber", DBNull.Value);
                            log.LogVariableState("@CustomDataDate", DBNull.Value);
                            log.LogVariableState("@ValueId", DBNull.Value);
                        }
                        else if (type == "NUMBER")
                        {
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                            try
                            {
                                cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", Convert.ToDecimal(val));
                                log.LogVariableState("@CustomDataNumber", Convert.ToDecimal(val));
                            }
                            catch(Exception ex)
                            {
                                log.Error("Error occured while adding values to parameters", ex);
                                Message = Utilities.MessageUtils.getMessage(14, val);
                                SQLTrx.Rollback();
                                log.LogVariableState("CustomDataSetId", CustomDataSetId);
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(false);
                                return false;
                            }
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@ValueId", DBNull.Value);
                        }
                        else if (type == "DATE")
                        {
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                            try
                            {
                                DateTime datetime = new DateTime();
                                DateTime.TryParseExact(val as string, Utilities.ParafaitEnv.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out datetime);
                                cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", datetime);
                                log.LogVariableState("@CustomDataNumber", datetime);
                            }
                            catch(Exception ex)
                            {
                                log.Error("Error occured while adding values to parameters", ex);
                                Message = Utilities.MessageUtils.getMessage(15, val);
                                SQLTrx.Rollback();
                                log.LogVariableState("CustomDataSetId", CustomDataSetId);
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(false);
                                return false;
                            }
                            cmdDMLSql.Parameters.AddWithValue("@ValueId", DBNull.Value);
                        }
                        else
                        {
                            cmdDMLSql.Parameters.AddWithValue("@ValueId", c.Value);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataText", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataNumber", DBNull.Value);
                            cmdDMLSql.Parameters.AddWithValue("@CustomDataDate", DBNull.Value);
                        }
                    }

                    cmdDMLSql.Parameters.AddWithValue("@CustomDataSetId", CustomDataSetId);
                    cmdDMLSql.Parameters.AddWithValue("@CustomAttributeId", CustomAttributeId);
                    cmdDMLSql.CommandText = "update CustomData set ValueId = @ValueId, CustomDataText = @CustomDataText, CustomDataNumber = @CustomDataNumber, CustomDataDate = @CustomDataDate " +
                                    "where CustomDataSetId = @CustomDataSetId and CustomAttributeId = @CustomAttributeId";
                    log.LogVariableState("@CustomDataSetId", CustomDataSetId);
                    log.LogVariableState("@CustomAttributeId", CustomAttributeId);
                    if (cmdDMLSql.ExecuteNonQuery() == 0)
                    {
                        cmdDMLSql.CommandText = "insert into CustomData (CustomDataSetId, CustomAttributeId, ValueId, CustomDataText, CustomDataNumber, CustomDataDate) " +
                                    "values (@CustomDataSetId, @CustomAttributeId, @ValueId, @CustomDataText, @CustomDataNumber, @CustomDataDate)";
                        cmdDMLSql.ExecuteNonQuery();
                    }
                }
                SQLTrx.Commit();
                log.LogVariableState("CustomDataSetId", CustomDataSetId);
                log.LogVariableState("Message", Message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured during saving", ex);
                Message = ex.Message;
                SQLTrx.Rollback();
                log.LogVariableState("CustomDataSetId", CustomDataSetId);
                log.LogVariableState("Message", Message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                cnn.Close();
            }
        }

        public bool Save(int PrimaryKey, ref int CustomDataSetId)
        {
            log.LogMethodEntry(PrimaryKey, CustomDataSetId);
            if (Convert.ToBoolean(flpAttributes.Tag) == false)
            {
                log.LogVariableState("CustomDataSetId", CustomDataSetId);
                log.LogMethodExit(true);
                return true;
            }
            List<KeyValuePair<object, object>> customDataList = new List<KeyValuePair<object, object>>();
            foreach (Control c in flpAttributes.Controls)
            {
                if (c.Tag != null)
                {
                    KeyValuePair<object, object> customData;
                    if (c.GetType().ToString().Contains("ComboBox"))
                    {
                        if ((c as ComboBox).SelectedItem == DBNull.Value || (c as ComboBox).SelectedItem == null)
                        {
                            customData = new KeyValuePair<object, object>(c.Tag, DBNull.Value);
                        }
                        else
                        {
                            customData = new KeyValuePair<object, object>(c.Tag, (c as ComboBox).SelectedValue);
                        }
                        customDataList.Add(customData);
                    }
                    else if (c.GetType().ToString().Contains("Radio"))
                    {
                        if ((c as RadioButton).Checked)
                        {
                            customData = new KeyValuePair<object, object>(c.Tag, c.Name.Substring(c.Tag.ToString().Length));
                            customDataList.Add(customData);
                        }
                    }
                    else if (c.GetType().ToString().Contains("TextBox"))
                    {
                        if (string.IsNullOrEmpty(c.Text.Trim()))
                            customData = new KeyValuePair<object, object>(c.Tag, DBNull.Value);
                        else
                            customData = new KeyValuePair<object, object>(c.Tag, c.Text.Trim());
                        customDataList.Add(customData);
                    }
                    else if (c.GetType().ToString().Contains("Check"))
                    {
                        SqlCommand cmdValues = Utilities.getCommand();
                        cmdValues.CommandText = @"select ValueId
                                                    from CustomAttributeValueList cv, CustomAttributes c
                                                    where cv.CustomAttributeId = c.CustomAttributeId 
                                                    and c.Name = @CName 
                                                    and c.Applicability = @Applicability
                                                    and cv.Value = @value";
                        cmdValues.Parameters.AddWithValue("@CName", c.Tag);
                        cmdValues.Parameters.AddWithValue("@Applicability", _Applicability);
                        cmdValues.Parameters.AddWithValue("@value", (c as CheckBox).Checked ? "1" : "0");
                        log.LogVariableState("@CName", c.Tag);
                        log.LogVariableState("@Applicability", _Applicability);
                        log.LogVariableState("@value", (c as CheckBox).Checked ? "1" : "0");
                        customData = new KeyValuePair<object, object>(c.Tag, cmdValues.ExecuteScalar());
                        customDataList.Add(customData);
                    }
                }
            }
            string message = "";
            bool ret = Save(PrimaryKey, ref CustomDataSetId, customDataList, ref message);
            if (ret == false)
                MessageBox.Show(message, "Custom Attribute Save");
            log.LogVariableState("CustomDataSetId", CustomDataSetId);
            log.LogMethodExit(ret);
            return ret;
        }

        public string FieldDisplayOption(object AttributeName)
        {
            log.LogMethodEntry(AttributeName);
            object o = Utilities.executeScalar(@"select isnull(pos.optionvalue, pd.default_value) value
                                                from parafait_defaults pd
                                                left outer join ParafaitOptionValues pos 
                                                    on pd.default_value_id = pos.optionId 
                                                    and POSMachineId = @POSMachineId
                                                    and pos.activeFlag = 'Y',
                                                CustomAttributes cu
                                                where cu.Name = pd.default_value_name
                                                and cu.Applicability = @Applicability
                                                and cu.Name = @Name
                                                and pd.active_flag = 'Y'
                                                and screen_group = 'customer'",
                                                new SqlParameter("@POSMachineId", Utilities.ParafaitEnv.POSMachineId),
                                                new SqlParameter("@Applicability", _Applicability),
                                                new SqlParameter("@Name", AttributeName.ToString()));
            log.LogVariableState("@POSMachineId", Utilities.ParafaitEnv.POSMachineId);
            log.LogVariableState("@Applicability", _Applicability);
            log.LogVariableState("@Name", AttributeName.ToString());

            if (o == null)
            {
                log.LogMethodExit(null);
                return null;
            }
           
            else
            {
                log.LogMethodExit(o.ToString());
                return o.ToString();
            }
        }
    }
}
