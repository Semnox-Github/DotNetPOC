/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - AdvancedSearch 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// AdvanceSearch class
    /// </summary>
    public partial class AdvancedSearch : Form
    {
        string tableName, alias;
        private Semnox.Core.Utilities.Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// AdvancedSearch
        /// </summary>
        /// <param name="_Utilities"> utilities</param>
        /// <param name="pTableName"> table name</param>
        /// <param name="pAlias"> alias</param>
        public AdvancedSearch(Semnox.Core.Utilities.Utilities _Utilities, string pTableName, string pAlias)
        {
            log.LogMethodEntry(_Utilities, pTableName, pAlias);
            InitializeComponent();

            utilities = _Utilities;
            utilities.setLanguage(this);
            tableName = pTableName;
            alias = pAlias;
            if (alias != "")
                alias += ".";

            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            log.LogMethodExit();

        }

        private void AdvancedSearch_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'redemptionDataSet.Product' table. You can move, or remove it, as needed.
            log.LogMethodEntry();
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
            log.LogMethodExit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string criteria, value, condition;

            condition = cmbCriteria.SelectedItem.ToString();

            DataRowView dr = cmbSegments.SelectedItem as DataRowView;

            if (dr != null)
            {
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
                log.LogMethodExit();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
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
            log.LogMethodEntry();
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
                criteria = " (" + "[" + dr.Cells["field"].Value.ToString() + "]" + " " + condition + dr.Cells["value"].Value.ToString() + ")";

                if (dr.Index == 0)
                    searchCriteria += criteria;
                else
                    searchCriteria += " " + dr.Cells["AndOr"].Value + " " + criteria;
            }

            this.DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void dgvCriteria_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == 1)
            {
                dgvCriteria.Rows.RemoveAt(e.RowIndex);
                if (e.RowIndex == 0 && dgvCriteria.Rows.Count > 0)
                {
                    if (dgvCriteria[0, 0].Value.ToString().StartsWith(" AND"))
                        dgvCriteria[0, 0].Value = dgvCriteria[0, 0].Value.ToString().Substring(5);
                }
            }
            log.LogMethodExit();
        }

        private void lnkDateLookup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
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
            log.LogMethodExit();
        }

        void dtp_CloseUp(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DateTimePicker dtp = sender as DateTimePicker;
            this.Controls.Remove(dtp);
            log.LogMethodExit();
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DateTimePicker dtp = sender as DateTimePicker;
            txtValue.Text = dtp.Value.ToString("dd-MMM-yyyy");
            this.Controls.Remove(dtp);
            log.LogMethodExit();
        }
    }
}
