using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Semnox.Parafait.GenericUI
{
    /// <summary>
    /// AdvanceSearch class
    /// </summary>
    public partial class AdvancedSearch : Form
    {
        string tableName, alias;
        ParafaitUtils.Utilities utilities;
        /// <summary>
        /// AdvancedSearch
        /// </summary>
        /// <param name="_Utilities"> utilities</param>
        /// <param name="pTableName"> table name</param>
        /// <param name="pAlias"> alias</param>
        public AdvancedSearch(ParafaitUtils.Utilities _Utilities, string pTableName, string pAlias)
        {
            InitializeComponent();

            utilities = _Utilities;
            utilities.setLanguage(this);
            tableName = pTableName;
            alias = pAlias;
            if (alias != "")
                alias += ".";

            //CommonFuncs.setupVisuals(this);
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

            //lblHeading.Text = "Custom Attribute Data for " + Applicability;
            //lblHeading.Text = textInfo.ToTitleCase(lblHeading.Text.ToLower());
        }

        private void AdvancedSearch_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'redemptionDataSet.Product' table. You can move, or remove it, as needed.
            SqlCommand cmd = utilities.getCommand();
            cmd.CommandText = @"select sd.*, datasourcetype, segmentdefinitionsourceid 
                                from Segment_Definition sd, Segment_Definition_Source_Mapping sm 
                                where sd.segmentdefinitionid = sm.segmentdefinitionid 
                                    and ApplicableEntity = @Applicability 
                                    and (sd.site_id = @site_id or @site_id = -1) 
                                    and sm.isactive = 'Y'
                                    and sd.isactive = 'Y'
                                order by sequenceorder";
            cmd.Parameters.AddWithValue("@Applicability", tableName);
            cmd.Parameters.AddWithValue("@site_id", utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId : -1); ;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbSegments.DataSource = dt;
            cmbSegments.DisplayMember = "SegmentName";
            cmbSegments.ValueMember = "SegmentName";

            cmbCriteria.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string criteria, value, condition;

            condition = cmbCriteria.SelectedItem.ToString();

            DataRowView dr = cmbSegments.SelectedItem as DataRowView;

            if (condition.ToLower().Contains("contain"))
                value = "'%" + txtValue.Text + "%'";
            else if (condition.ToLower().Contains("null"))
                value = "";
            else
                value = "'" + txtValue.Text + "'";

            if (string.IsNullOrEmpty(value) && !condition.ToLower().Contains("null"))
                value = "''";
            criteria = " segmentname = '" + cmbSegments.SelectedValue.ToString() + "' and ";
            if (dr["datasourcetype"].ToString().Contains("date"))
                criteria += "replace(convert(varchar(11), ValueChar, 106), ' ', '-') " + condition + " " + value;
            else
                criteria += "ValueChar " + condition + " " + value;

            if (dgvCriteria.Rows.Count > 0)
            {
                criteria = " " + (rbAnd.Checked ? "AND" : "OR") + " " + criteria;
            }

            dgvCriteria.Rows.Add(new object[] { criteria, 'X', dr["segmentname"], dr["datasourcetype"], condition, value, (rbAnd.Checked ? "AND" : "OR") });

            dgvCriteria.FirstDisplayedScrollingRowIndex = dgvCriteria.Rows.Count - 1;
            utilities.setLanguage(dgvCriteria);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
        /// <summary>
        /// searchCriteria
        /// </summary>
        public string searchCriteria;
        /// <summary>
        /// searchString
        /// </summary>
        public string searchString;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string criteria, condition;
            string query = "select * from " + tableName;

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

                //Updated string 11-Aug-2016
                criteria = " (" + dr.Cells["field"].Value.ToString() + " " + condition + dr.Cells["value"].Value.ToString() + ")";

                if (dr.Index == 0)
                    searchCriteria += criteria;
                else
                    searchCriteria += " " + dr.Cells["AndOr"].Value + " " + criteria;
            }

            this.DialogResult = DialogResult.OK;
            Close();
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
