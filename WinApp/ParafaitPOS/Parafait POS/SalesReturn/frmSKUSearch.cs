using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.SalesReturn
{
    public partial class frmSKUSearch : Form
    {
        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        TaskProcs TaskProcs = POSStatic.TaskProcs;
        ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string alias;
        string productCode;

        public frmSKUSearch(string pcode)
        {
            log.Debug("Starts-frmSKUSearch()");
            InitializeComponent();

            Utilities = POSStatic.Utilities;
            ParafaitEnv = Utilities.ParafaitEnv;
            MessageUtils = Utilities.MessageUtils;
            Utilities.setLanguage(this);

            if (alias != "")
                alias += ".";

            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            productCode = pcode;
            log.Debug("End-frmSKUSearch()");
        }

        private void frmSKUSearch_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmSKUSearch_Load()");
            try
            {
                SqlCommand cmd = Utilities.getCommand();
                cmd.CommandText = "select sd.*, datasourcetype, segmentdefinitionsourceid from Segment_Definition sd, Segment_Definition_Source_Mapping sm where sd.segmentdefinitionid = sm.segmentdefinitionid and ApplicableEntity = 'Product' order by sequenceorder";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbSegments.DataSource = dt;
                cmbSegments.DisplayMember = "SegmentName";
                cmbSegments.ValueMember = "SegmentName";

                cmbCriteria.SelectedIndex = 0;
            }
            catch { }
            log.Debug("Ends-frmSKUSearch_Load()");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnAdd_Click()");
            string criteria, value, condition;
            try
            {
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
                Utilities.setLanguage(dgvCriteria);
            }
            catch { }
            log.Debug("Ends-btnAdd_Click()");
        }

        public string searchCriteria;
        public string searchString;
        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            string criteria, condition;
            searchCriteria = "";

            log.Debug("Starts-btnProductSearch_Click()");
            try
            {
                foreach (DataGridViewRow dr in dgvCriteria.Rows)
                {
                    condition = dr.Cells["operatOr"].Value.ToString();
                    switch (condition)
                    {
                        case "Contains": condition = "like"; break;
                        case "Does not contain": condition = "not like"; break;
                        default: break;
                    }

                    criteria = " (segmentname = '" + dr.Cells["field"].Value.ToString() + "' and ValueChar " + condition + dr.Cells["value"].Value.ToString() + ")";
                    
                    if (dr.Index == 0)
                        searchCriteria += criteria;
                    else
                        searchCriteria += " " + dr.Cells["AndOr"].Value + " " + criteria;
                }

                if (searchCriteria != "")
                {
                    SqlCommand cmd = Utilities.getCommand();
                    cmd.CommandText = "select ','+ cast(SegmentCategoryId as varchar(10)) from SegmentDataView where " + searchCriteria + " and ApplicableEntity = 'Product'  FOR XML PATH('')";
                    try
                    {
                        object ob = cmd.ExecuteScalar();
                        if (ob == DBNull.Value || ob == null)
                            searchString = "-1";
                        else
                            searchString = ob.ToString().Substring(1);
                        this.DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(Utilities.MessageUtils.getMessage(716) + Environment.NewLine + ex.Message + Environment.NewLine + Utilities.MessageUtils.getMessage(645));
                        MessageBox.Show(Utilities.MessageUtils.getMessage("Effective criteria was: ") + searchCriteria);
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch { }
            log.Debug("Ends-btnProductSearch_Click()");
        }

        private void dgvCriteria_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvCriteria_CellClick()");
            try
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
            catch
            {

            }
            log.Debug("Ends-dgvCriteria_CellClick()");
        }

        private void lnkDateLookup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DateTimePicker dtp = null;
            log.Debug("Starts-lnkDateLookup_LinkClicked()");
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
            log.Debug("Ends-lnkDateLookup_LinkClicked()");
        }

        void dtp_CloseUp(object sender, EventArgs e)
        {
            log.Debug("Starts-dtp_CloseUp()");
            DateTimePicker dtp = sender as DateTimePicker;
            this.Controls.Remove(dtp);
            log.Debug("Ends-dtp_CloseUp()");
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtp_ValueChanged()");
            DateTimePicker dtp = sender as DateTimePicker;
            txtValue.Text = dtp.Value.ToString("dd-MMM-yyyy");
            this.Controls.Remove(dtp);
            log.Debug("Ends-dtp_ValueChanged()");
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click()");
            this.DialogResult = DialogResult.Cancel;
            Close();
            log.Debug("Ends-btnClose_Click()");
        }
    }
}
