/********************************************************************************************
* Project Name - Parafait Report
* Description  -UI of frmParameterView 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80       23-Aug-2019      Jinto Thomas        Added logger into methods
 * 2.80        18-Sep-2019     Dakshakh raj        Modified : Added logs
********************************************************************************************/
using System;
using Semnox.Parafait.Reports;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.Reports;
//using ReportsLibrary;

namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    /// frmParameterView Class
    /// </summary>
    public partial class frmParameterView : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int _reportId;
        int _reportScheduleReportId;
        /// <summary>
        /// _parameters property
        /// </summary>
        public clsReportParameters _parameters; // externally visible list of parameters

        /// <summary>
        ///  parameter reportScheduleReportId is passed when called for saving parameters for scheduled reports 
        ///  </summary>
        public frmParameterView(int ReportId, int reportScheduleReportId = -1)
        {
            log.LogMethodEntry(ReportId, reportScheduleReportId);
            InitializeComponent();
            _reportId = ReportId;
            _reportScheduleReportId = reportScheduleReportId;
            Common.Utilities.setLanguage(this);
            _parameters = new clsReportParameters(_reportId);
            _parameters.getScheduleParameters(_reportId);
        }

        void updateData()
        {
            log.LogMethodEntry(0);
            foreach (clsReportParameters.ReportParameter param in _parameters.lstParameters)
            {
                if (param.SQLParameter.StartsWith("@") == false)
                    param.SQLParameter = "@" + param.SQLParameter;

                if (param.Operator == clsReportParameters.Operator.Default)
                {
                    switch (param.DataSourceType)
                    {
                        case clsReportParameters.DataSourceType.CONSTANT:
                            {
                                if (string.IsNullOrEmpty(param.UIControl.Text.Trim()))
                                    param.Value = DBNull.Value;
                                else
                                    param.Value = param.UIControl.Text.Trim();
                                break;
                            }
                        case clsReportParameters.DataSourceType.STATIC_LIST:
                        case clsReportParameters.DataSourceType.QUERY:
                            {
                                param.Value = (param.UIControl as ComboBox).SelectedValue;
                                break;
                            }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                updateData();
                _parameters.getParameterList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.LogMethodExit("Caught an exception " + ex.Message);
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();

        }

        private void frmParameterView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (_reportScheduleReportId != -1)
                {
                    _parameters.getScheduleParameters(_reportScheduleReportId);
                }

                foreach (clsReportParameters.ReportParameter param in _parameters.lstParameters)
                {
                    string paramName = param.ParameterName;
                    string dataSource = param.DataSource.ToString();

                    Label lbl = new Label();
                    lbl.AutoSize = false;
                    lbl.Text = paramName + ":";
                    lbl.TextAlign = ContentAlignment.MiddleRight;
                    lbl.Width = flpParameters.Width / 3;
                    lbl.Height = 26;

                    flpParameters.Controls.Add(lbl);
                    FlowLayoutPanel controlPanel = new FlowLayoutPanel();
                    controlPanel.Width = flpParameters.Width - lbl.Width - 100;
                    controlPanel.Height = lbl.Height;
                    flpParameters.Controls.Add(controlPanel);

                    log.Debug("param.DataSourceType" + param.DataSourceType);

                    switch (param.DataSourceType)
                    {
                        case clsReportParameters.DataSourceType.CONSTANT:
                            {
                                switch (param.DataType)
                                {
                                    case clsReportParameters.DataType.TEXT:
                                        {
                                            TextBox txt = new TextBox();
                                            txt.Width = controlPanel.Width - 5;
                                            txt.Height = lbl.Height;
                                            txt.Name = paramName;
                                            if (param.Value != null)
                                                txt.Text = param.Value.ToString();
                                            else
                                                txt.Text = dataSource;
                                            controlPanel.Controls.Add(txt);
                                            param.UIControl = txt;
                                            break;
                                        }
                                    case clsReportParameters.DataType.NUMBER:
                                        {
                                            TextBox txt = new TextBox();
                                            txt.Width = controlPanel.Width - 5;
                                            txt.Name = paramName;
                                            try
                                            {
                                                if (param.Value != null)
                                                    txt.Text = param.Value.ToString();
                                                else
                                                    txt.Text = Convert.ToDecimal(dataSource).ToString();
                                            }
                                            catch { }
                                            controlPanel.Controls.Add(txt);
                                            param.UIControl = txt;

                                            txt.KeyPress += new KeyPressEventHandler(delegate(object s, KeyPressEventArgs ev)
                                                {
                                                    char decimalChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
                                                    if (!Char.IsNumber(ev.KeyChar) && !Char.IsControl(ev.KeyChar) && !(ev.KeyChar == decimalChar))
                                                    {
                                                        ev.Handled = true;
                                                    }
                                                });

                                            break;
                                        }
                                    case clsReportParameters.DataType.DATETIME:
                                        {
                                            TextBox txt = new TextBox();
                                            txt.Width = controlPanel.Width - 35;
                                            txt.Name = paramName;
                                            if (param.Value != null && param.Value.ToString().Trim() != "")
                                                txt.Text = Convert.ToDateTime(param.Value).ToString(Common.ParafaitEnv.DATETIME_FORMAT);
                                            else
                                                txt.Text = dataSource;
                                            controlPanel.Controls.Add(txt);
                                            param.UIControl = txt;

                                            DateTimePicker dtPicker = new DateTimePicker();
                                            dtPicker.Width = 17;
                                            dtPicker.Height = txt.Height;
                                            dtPicker.Format = DateTimePickerFormat.Short;
                                            controlPanel.Controls.Add(dtPicker);

                                            dtPicker.ValueChanged += new EventHandler(delegate(object o, EventArgs ev)
                                            {
                                                txt.Text = dtPicker.Value.Date.AddHours(6).ToString(dtPicker.CustomFormat);
                                            });
                                            break;
                                        }
                                }
                                break;
                            }
                        case clsReportParameters.DataSourceType.STATIC_LIST:
                            {
                                ComboBox cmb = new ComboBox();
                                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                                cmb.Width = controlPanel.Width - 5;
                                cmb.Name = paramName;
                                cmb.DataSource = param.ListDataSource;
                                cmb.DisplayMember = cmb.ValueMember = param.ListDataSource.Columns[0].ColumnName;

                                controlPanel.Controls.Add(cmb);

                                if (param.Value != null)
                                {
                                    try
                                    {
                                        cmb.SelectedValue = param.Value;
                                    }
                                    catch { }
                                }
                                
                                param.UIControl = cmb;
                                break;
                            }
                        case clsReportParameters.DataSourceType.QUERY:
                            {
                                ComboBox cmb = new ComboBox();
                                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                                cmb.Width = controlPanel.Width - 5;
                                cmb.Name = paramName;
                                cmb.DataSource = param.ListDataSource;
                                cmb.ValueMember = param.ListDataSource.Columns[0].ColumnName;
                                if (param.ListDataSource.Columns.Count > 1)
                                    cmb.DisplayMember = param.ListDataSource.Columns[1].ColumnName;
                                else
                                    cmb.DisplayMember = cmb.ValueMember;

                                controlPanel.Controls.Add(cmb);

                                if (param.Value != null)
                                {
                                    try
                                    {
                                        cmb.SelectedValue = param.Value;
                                    }
                                    catch { }
                                }
                                param.UIControl = cmb;
                                break;
                            }
                    }

                    log.Debug("param.Operator" + param.Operator);

                    switch (param.Operator)
                    {
                        case clsReportParameters.Operator.INLIST:
                            {
                                Button addButton = new Button();
                                addButton.Font = new System.Drawing.Font(addButton.Font.FontFamily, addButton.Font.Size - 0.5f, FontStyle.Bold);
                                addButton.Text = "Add";
                                addButton.Width = 42;
                                addButton.Height = controlPanel.Controls[0].Height;
                                addButton.FlatStyle = FlatStyle.Flat;
                                addButton.FlatAppearance.CheckedBackColor =
                                    addButton.FlatAppearance.MouseDownBackColor =
                                    addButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                                addButton.TextAlign = ContentAlignment.TopCenter;
                                controlPanel.Controls[0].Width -= 50;
                                controlPanel.Controls.Add(addButton);

                                controlPanel.Height += 100;

                                DataGridView dgvInList = new DataGridView();
                                DataGridViewButtonColumn dcRemoveValue = new System.Windows.Forms.DataGridViewButtonColumn();
                                dcRemoveValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                                dcRemoveValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                dcRemoveValue.HeaderText = "X";
                                dcRemoveValue.Name = "dcRemoveValue";
                                dcRemoveValue.Text = "X";
                                dcRemoveValue.UseColumnTextForButtonValue = true;
                                dcRemoveValue.ToolTipText = "Remove";
                                dcRemoveValue.Width = 24;
                                dgvInList.Columns.Add(dcRemoveValue);

                                dgvInList.Height = controlPanel.Height - controlPanel.Controls[0].Height - 10;
                                dgvInList.Width = controlPanel.Width - 5;
                                dgvInList.ReadOnly = true;
                                dgvInList.AllowUserToAddRows = false;
                                dgvInList.AllowUserToDeleteRows = false;
                                dgvInList.RowHeadersVisible = false;
                                Common.Utilities.setupDataGridProperties(ref dgvInList);
                                dgvInList.BorderStyle = BorderStyle.Fixed3D;
                                dgvInList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                                dgvInList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                                dgvInList.Name = "dgvInList";
                                controlPanel.Controls.Add(dgvInList);

                                var source = new BindingSource();
                                source.DataSource = param.InListValue.InListValueDT;
                                dgvInList.DataSource = source;
                                dgvInList.CellContentClick += new DataGridViewCellEventHandler(delegate(object se, DataGridViewCellEventArgs ev)
                                    {
                                        if (ev.RowIndex >= 0 && ev.ColumnIndex == 0)
                                        {
                                            param.InListValue.remove(dgvInList[1, ev.RowIndex].Value);
                                            dgvInList.Refresh();
                                        }
                                    });

                                dgvInList.Columns[1].Visible = false;
                                dgvInList.Columns[2].HeaderText = "In List";
                                dgvInList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                dgvInList.BackgroundColor = Color.White;

                                addButton.Click += new EventHandler(delegate(object s, EventArgs ev)
                                {
                                    if (controlPanel.Controls[0].GetType().ToString().Contains("TextBox"))
                                    {
                                        if (!string.IsNullOrEmpty(controlPanel.Controls[0].Text.Trim()))
                                            param.InListValue.addValue(controlPanel.Controls[0].Text, controlPanel.Controls[0].Text);
                                    }
                                    else if (controlPanel.Controls[0].GetType().ToString().Contains("DateTime"))
                                    {
                                        if (!string.IsNullOrEmpty(controlPanel.Controls[0].Text.Trim()))
                                            param.InListValue.addValue(controlPanel.Controls[0].Text, controlPanel.Controls[0].Text);
                                    }
                                    else
                                    {
                                        if ((controlPanel.Controls[0] as ComboBox).SelectedValue != DBNull.Value)
                                            param.InListValue.addValue((controlPanel.Controls[0] as ComboBox).SelectedValue, (controlPanel.Controls[0] as ComboBox).Text);
                                    }
                                });
                                break;
                            }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void lnkCheck_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                updateData();
                string s = _parameters.getParameterString();
                if (string.IsNullOrEmpty(s.Trim()))
                    s = "No value specified for any of the parameters";
                MessageBox.Show(s);
            }
            catch (Exception ex)
            {
                log.Error("caught exception " + ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
