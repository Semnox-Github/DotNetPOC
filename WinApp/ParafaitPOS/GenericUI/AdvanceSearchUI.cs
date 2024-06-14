/********************************************************************************************
 * Project Name - Segment Definition Source Map UI
 * Description  - User interface for segment definition source map UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Apr-2016   Raghuveera          Created 
 ********************************************************************************************/
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

namespace Semnox.Parafait.GenericUI
{
    /// <summary>
    /// Advance search UI
    /// </summary>
    public partial class AdvanceSearchUI : Form
    {
        string tableName, alias;
        ParafaitUtils.Utilities utilities;
        /// <summary>
        /// contains the genarated search condition
        /// </summary>
        public string searchCriteria;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities">utilities passed from environment from where this file is called</param>
        /// <param name="pTableName">Database table name</param>
        /// <param name="pAlias"> Alias for the table</param>
        public AdvanceSearchUI(ParafaitUtils.Utilities _Utilities, string pTableName, string pAlias)
        {
            InitializeComponent();
            utilities=_Utilities;
            utilities.setLanguage(this);
            tableName = pTableName;
            alias = pAlias;
            if (alias != "")
                alias += ".";
        }

        private void CustomerSearch_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = utilities.getCommand();
            cmd.CommandText = "select c.name, ty.name datatype " +
                                "from sys.columns c, sys.tables t, sys.types ty  " +
                                "where t.name = '" + tableName + "'" +
                                "and t.object_id = c.object_id " +
                                "and ty.user_type_id = c.user_type_id " +
                                "order by 1 ";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbColumns.DataSource = dt;
            cmbColumns.DisplayMember = "name";
            cmbColumns.ValueMember = "name";

            cmbCriteria.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(searchCriteria))
            setCriteria(searchCriteria);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string criteria, value, condition;

            condition = cmbCriteria.SelectedItem.ToString();

            DataRowView dr = cmbColumns.SelectedItem as DataRowView;

            if (condition.ToLower().Contains("contain"))
                value = "'%" + txtValue.Text + "%'";
            else if (condition.ToLower().Contains("null"))
                value = "";
            else
            {
                if (dr["datatype"].ToString().Contains("char") || dr["datatype"].ToString().Contains("time"))
                    value = "'" + txtValue.Text + "'";
                else
                    value = txtValue.Text;
            }

            if (string.IsNullOrEmpty(value) && !condition.ToLower().Contains("null"))
                value = "''";
            criteria = cmbColumns.SelectedValue.ToString() + " " + condition + " " + value;

            if (dgvCriteria.Rows.Count > 0)
            {
                criteria = " " + (rbAnd.Checked ? "AND" : "OR") + " " + criteria;
            }

            dgvCriteria.Rows.Add(new object[] { criteria, 'X', dr["name"], dr["datatype"], condition, value, (rbAnd.Checked ? "AND" : "OR") });

            dgvCriteria.FirstDisplayedScrollingRowIndex = dgvCriteria.Rows.Count - 1;
            utilities.setLanguage(dgvCriteria);

        }
        private void setCriteria(string searchCriteria)
        {
            string[] criteria;
            string condition, value;
            string field;
            string andOr;
            string[] param = new string[] { "AND", "OR" };
            criteria = searchCriteria.Split(param, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < criteria.Length; i++)
            {
                criteria[i] = criteria[i].Trim();
                if (criteria[i].Contains("not like"))
                {
                    condition = "not like";
                }
                else if (criteria[i].Contains("like"))
                {
                    condition = "like";
                }
                else if (criteria[i].Contains("="))
                {
                    condition = "=";
                }
                else if (criteria[i].Contains(">"))
                {
                    condition = ">";
                }
                else if (criteria[i].Contains(">="))
                {
                    condition = ">=";
                }
                else if (criteria[i].Contains("<"))
                {
                    condition = "<";
                }
                else if (criteria[i].Contains("<="))
                {
                    condition = "<=";
                }
                else if (criteria[i].Contains("is null"))
                {
                    condition = "is null";
                }
                else if (criteria[i].Contains("is not null"))
                {
                    condition = "is not null";
                }
                else
                {
                    continue;
                }
                field = criteria[i].Substring(alias.Length , criteria[i].IndexOf(condition) - alias.Length-1 );
                if (criteria[i].ToLower().Contains("convert"))
                {
                    int sIndex = 0;
                    sIndex = field.IndexOf(alias);
                    field = field.Substring(sIndex + alias.Length, field.IndexOf(',', sIndex + alias.Length) - (sIndex + alias.Length));
                }
                cmbColumns.SelectedIndex = cmbColumns.FindStringExact(field.Trim());
                //cmbColumns.SelectedText = field.Trim();
                int startIndex=searchCriteria.IndexOf(criteria[(i==0)?0:i-1]);
                int len = searchCriteria.IndexOf(criteria[i]) + criteria[i].Length;                
                andOr = searchCriteria.Substring(startIndex, len - startIndex);
                andOr = (andOr.Contains("AND") ? "AND" : andOr.Contains("OR") ? "OR" : "");
                value = criteria[i].Substring(criteria[i].IndexOf(condition) + condition.Length);

                DataRowView dr = cmbColumns.SelectedItem as DataRowView;
                dgvCriteria.Rows.Add(new object[] { andOr + " " +  (criteria[i].Contains("not like") ? criteria[i].Replace("not like", "Does not contain") : criteria[i].Contains("like") ? criteria[i].Replace("like", "Contains") : criteria[i]), 'X', dr["name"], dr["datatype"], condition, value, andOr.Trim() });
            }
            dgvCriteria.FirstDisplayedScrollingRowIndex = dgvCriteria.Rows.Count - 1;
            utilities.setLanguage(dgvCriteria);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string criteria, condition;

            searchCriteria = "";

            foreach (DataGridViewRow dr in dgvCriteria.Rows)
            {
                condition = dr.Cells["operatOr"].Value.ToString();
                switch (condition)
                {
                    case "Contains": condition = "like"; break;
                    case "Does not contain": condition = "not like"; break;
                    default: break;
                }

                if (!dr.Cells["datatype"].Value.ToString().ToLower().Contains("char") && condition.Contains("like"))
                    criteria = "convert("+ alias + dr.Cells["field"].Value.ToString() + ", 'System.String')" + " " + condition + " " + dr.Cells["value"].Value.ToString();
                else
                    criteria = alias + dr.Cells["field"].Value.ToString() + " " + condition + " " + dr.Cells["value"].Value.ToString();

                if (dr.Index == 0)
                    searchCriteria += criteria;
                else
                    searchCriteria += " " + dr.Cells["AndOr"].Value + " " + criteria;
            }

            if (searchCriteria != "")
            {
                SqlCommand cmd = utilities.getCommand();                
                cmd.CommandText = "select top 1 1 from " + tableName + " " + alias.Replace(".", "") + " where " + searchCriteria.Replace("convert(", "").Replace(", 'System.String')", "");
                try
                {
                    cmd.ExecuteScalar();
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                catch (SqlException ex)
                {
                  MessageBox.Show(utilities.MessageUtils.getMessage(716) + Environment.NewLine + ex.Message + Environment.NewLine + utilities.MessageUtils.getMessage(645));
                  MessageBox.Show(utilities.MessageUtils.getMessage("Effective criteria was: ") + searchCriteria);
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void dgvCriteria_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                dgvCriteria.Rows.RemoveAt(e.RowIndex);
                if (e.RowIndex == 0 && dgvCriteria.Rows.Count > 0)
                {
                    if (dgvCriteria[0, 0].Value.ToString().StartsWith(" AND"))
                        dgvCriteria[0, 0].Value = dgvCriteria[0, 0].Value.ToString().Substring(5);
                }
            }
        }

        private void lnkDateLookup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DateTimePicker dtp = null;
            try
            {
                dtp = this.Controls.Find("dtp", true)[0] as DateTimePicker;
            }
            catch { }
            if (dtp == null)
            {
                dtp = new DateTimePicker();
                dtp.Name = "dtp";
                this.Controls.Add(dtp);
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd-MMM-yyyy";
                dtp.Location = new Point(txtValue.Location.X, txtValue.Location.Y + 16);
                dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                dtp.CloseUp += new EventHandler(dtp_CloseUp);
            }

            dtp.BringToFront();
            dtp.Focus();
            SendKeys.Send("{F4}");
        }

        void dtp_CloseUp(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            this.Controls.Remove(dtp);
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            txtValue.Text = dtp.Value.ToString("dd-MMM-yyyy");
            this.Controls.Remove(dtp);
        }
    }
}
